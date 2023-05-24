using game_scoreboard_service.Hubs.Clients;
using game_scoreboard_service.Models.Responses;
using game_scoreboard_service.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace game_scoreboard_service.Hubs
{
    public class ScoreboardHub : Hub<IScoreboardClient>
    {
        private readonly IPlayerScoreService _playerScoreService;
        public ScoreboardHub(IPlayerScoreService playerScoreService)
        {
            _playerScoreService = playerScoreService;
        }

        public async Task RequestListOfUserScoreResponse(int page)
        {
            await _playerScoreService.CheckQueueForNewUsers();
            await _playerScoreService.UpdateUserScore();
            var result = await _playerScoreService.GetListOfUserScores(page);
            if (result is null) await Clients.All.ReceiveListOfUserScores(new ScoreBoardResponse());
            await Clients.All.ReceiveListOfUserScores(result);
        }
    }
}
