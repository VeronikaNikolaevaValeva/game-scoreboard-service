using game_scoreboard_service.Models;

namespace game_scoreboard_service.Repositories.Interfaces
{
    /// <summary>
    /// Repository for the playerScore CosmosDB Container
    /// </summary>
    public interface IPlayerScoreRepository : IBaseRepository<PlayerScore>
    {
        /// <summary>
        /// Returns an enumerable of PlayerScore entity models - page sized
        /// </summary>
        /// <returns>An Enumerable of PlayerScore Entity Models.</returns>
        Task<IEnumerable<PlayerScore>> GetPageSizePlayerScores(int page);

        /// <summary>
        /// Returns the number of player scores 
        /// </summary>
        /// <returns>The count of player scores - int.</returns>
        Task<int?> GetCountOfPlayerScores();
    }
}
