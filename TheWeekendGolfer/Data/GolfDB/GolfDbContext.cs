using Microsoft.EntityFrameworkCore;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Web.Data
{


    /// <summary>
    /// Used to create structure for golf DB
    /// </summary>
    public class GolfDbContext : DbContext
    {
        public GolfDbContext(DbContextOptions<GolfDbContext> options) : base(options)
        {

        }


        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Score> Scores { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<GolfRound> GolfRounds { get; set; }
        public virtual DbSet<Handicap> Handicaps { get; set; }
        public virtual DbSet<Partner> Partners { get; set; }

    }
}
