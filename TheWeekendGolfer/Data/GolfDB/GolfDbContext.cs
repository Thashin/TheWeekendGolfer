using Microsoft.EntityFrameworkCore;
using TheWeekendGolfer.Web.Models;

namespace TheWeekendGolfer.Web.Data
{
    public class GolfDbContext: DbContext
    {
        public GolfDbContext(DbContextOptions options):base(options)
        {

        }


        public DbSet<Course> Courses { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<GolfRound> GolfRounds { get; set; }

    }
}
