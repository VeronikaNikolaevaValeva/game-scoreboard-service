using game_scoreboard_service.Models.Responses;

namespace game_scoreboard_service.Services.Interfaces
{
    /// <summary>
    /// Service to perform operations on game options
    /// </summary>
    public interface IPlayerScoreService
    {
        /// <summary>
        /// Checks if there are any new users in the message queue. 
        /// If there are and they are not yet added to the db - add them.
        /// </summary>
        /// <returns>Result from processing</returns>
        Task<ServiceProduct<string>> CheckQueueForNewUsers();

        /// <summary>
        /// Checks if there are any new user score updates in the message queue. 
        /// If there are and they are not yet added to the db - add them.
        /// </summary>
        /// <returns>Result from processing</returns>
        Task<ServiceProduct<string>> UpdateUserScore();

        /// <summary>
        /// Gets a list of user scores per page and total count
        /// </summary>
        /// <returns>A ScoreBoardResponse Entity Model</returns>
        Task<ServiceProduct<ScoreBoardResponse>> GetListOfUserScores(int page);
    }
}
