using Microsoft.EntityFrameworkCore;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Web.Data
{
    /// <summary>
    /// Used to create structure for golf DB
    /// </summary>
    public class GolfDbContext: DbContext
    {
        public GolfDbContext(DbContextOptions options):base(options)
        {

        }


        public DbSet<Course> Courses { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<GolfRound> GolfRounds { get; set; }
        public DbSet<Handicap> Handicaps { get; set; }
        public DbSet<Partner> Partners { get; set; }

    }
}
