using game_scoreboard_service.Messaging.Interfaces;
using game_scoreboard_service.Models;
using game_scoreboard_service.Models.Enum;
using game_scoreboard_service.Models.Responses;
using game_scoreboard_service.Repositories.Interfaces;
using game_scoreboard_service.Services.Interfaces;

namespace game_scoreboard_service.Services
{
    public class PlayerScoreService : BaseService, IPlayerScoreService
    {
        private readonly IPlayerScoreRepository _playerScoreRepository;
        private readonly IMessagingSubscriber _messagingSubscriber;

        public PlayerScoreService(
            IPlayerScoreRepository playerScoreRepository,
            IMessagingSubscriber messagingSubscriber)
        {
            _playerScoreRepository = playerScoreRepository ?? throw new ArgumentNullException(nameof(playerScoreRepository));
            _messagingSubscriber = messagingSubscriber ?? throw new ArgumentNullException(nameof(messagingSubscriber));
        }

        public async Task<ServiceProduct<string>> CheckQueueForNewUsers()
        {
            var result = _messagingSubscriber.NewRegisteredUser();
            if (result == null) return Reject<string>(RejectionCode.General, "Something went wrong.");
            if (result.EmailAddress == null) return "No new users";
            var existing = await _playerScoreRepository.GetByPartitionKeyAsync(result.EmailAddress);
            if (existing != null) return "New user in the queue (v), Already exists (v).";
            var addUserResult = await _playerScoreRepository.AddAsync(new PlayerScore()
            {
                PartitionKey = result.EmailAddress,
                Nickname = result.Username,
                EmailAddress = result.EmailAddress,
                OverallScore = 0,
                AmountAnsweredQuestions = 0,
                AmountOfPlayedGames = 0,
                CorrectAnswerCount = 0,
                IncorrectAnswerCount = 0,
                NonAnsweredCount = 0
            });
            if (addUserResult is null) return Reject<string>(RejectionCode.General, "New user in the queue (v), Added to db (x).");
            return "New user added.";
        }

        public async Task<ServiceProduct<string>> UpdateUserScore()
        {
            var result = _messagingSubscriber.UpdateUserScore();
            if (result is null) return Reject<string>(RejectionCode.General, "Something went wrong.");
            if (String.IsNullOrEmpty(result.EmailAddress) || result is null) return Reject<string>(RejectionCode.General, "Queue was empty.");
            var existingRecord = await _playerScoreRepository.GetByPartitionKeyAsync(result.EmailAddress);
            if (existingRecord is null) return Reject<string>(RejectionCode.General, "User record not found.");
            existingRecord.AmountOfPlayedGames += result.AmountOfPlayedGames;
            existingRecord.AmountAnsweredQuestions += result.AmountAnsweredQuestions;
            existingRecord.CorrectAnswerCount += result.CorrectAnswerCount;
            existingRecord.IncorrectAnswerCount += result.IncorrectAnswerCount;
            existingRecord.NonAnsweredCount += result.NonAnsweredCount;
            existingRecord.OverallScore = (existingRecord.CorrectAnswerCount * 100) / existingRecord.AmountAnsweredQuestions;
            var updatedScore = await _playerScoreRepository.UpdateAsync(existingRecord);
            if (updatedScore is null) return Reject<string>(RejectionCode.General, "Something went wrong.");
            return $"Updated score for user {updatedScore.Nickname}.";
        }

        public async Task<ServiceProduct<ScoreBoardResponse>> GetListOfUserScores(int page)
        {
            var userScores = new ScoreBoardResponse();
            var playerScores = await _playerScoreRepository.GetPageSizePlayerScores(page);
            var playerScoresCount = await _playerScoreRepository.GetCountOfPlayerScores();
            if (playerScores is null) return userScores;
            userScores.UserScoreResponses = new List<UserScoreResponse>();
            for (int i = 0; i < playerScores.Count(); i++)
            {
                userScores.UserScoreResponses.Add(new UserScoreResponse()
                {
                    Nickname = playerScores.ToList()[i].Nickname,
                    ChartPosition = i,
                    OverallScore = playerScores.ToList()[i].OverallScore,
                    AmountAnsweredQuestions = playerScores.ToList()[i].AmountAnsweredQuestions,
                    AmountOfPlayedGames = playerScores.ToList()[i].AmountOfPlayedGames,
                    CorrectAnswerCount = playerScores.ToList()[i].CorrectAnswerCount,
                    IncorrectAnswerCount = playerScores.ToList()[i].IncorrectAnswerCount,
                    NonAnsweredCount = playerScores.ToList()[i].NonAnsweredCount
                });
            }
            userScores.TotalUserScores = playerScoresCount;
            userScores.PageUserScores = page;
            return userScores ?? new ScoreBoardResponse();
        }

        public async Task<ServiceProduct<bool>> DeleteProfileInformation(string partitionKey)
        {
            var existingUser = await _playerScoreRepository.GetByPartitionKeyAsync(partitionKey);
            if (existingUser is null) return Reject<bool>(RejectionCode.General, "User data not found.");
            var deletedUserResult = await _playerScoreRepository.DeleteAsync(existingUser);
            if(!deletedUserResult ?? true) return Reject<bool>(RejectionCode.General, "User data could not be deleted.");
            return true;
        }
    }
}
