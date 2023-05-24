using game_scoreboard_service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Cosmos;
using System.Xml.Linq;

namespace game_scoreboard_service.DataContext
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
           : base(options)
        {
        }

        public DbSet<PlayerScore> PlayerScore { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlayerScore>(entity =>
            {
                entity.ToContainer("PlayerScore");
            });
            modelBuilder.Entity<PlayerScore>()
                .HasPartitionKey(p => p.PartitionKey);

        }
    }
}
