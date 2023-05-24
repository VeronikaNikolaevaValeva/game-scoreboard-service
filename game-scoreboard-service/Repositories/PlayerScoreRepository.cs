using game_scoreboard_service.DataContext;
using game_scoreboard_service.Models;
using game_scoreboard_service.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace game_scoreboard_service.Repositories
{
    public class PlayerScoreRepository : BaseRepository<PlayerScore>, IPlayerScoreRepository
    {
        public PlayerScoreRepository(DatabaseContext db)
           : base(db)
        {
        }

        public async Task<IEnumerable<PlayerScore>> GetPageSizePlayerScores(int page)
        {
            return await _db.PlayerScore
                .OrderByDescending(x=>x.OverallScore)
                .ToListAsync();
        }

        public async Task<int?> GetCountOfPlayerScores()
        {
            return await _db.PlayerScore.CountAsync();
        }

    }
}
