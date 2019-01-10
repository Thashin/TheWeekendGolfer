using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class GolfRoundAccessLayer : IGolfRoundAccessLayer
    {
        GolfDbContext _context;

        public GolfRoundAccessLayer(GolfDbContext context)
        {
            _context = context;
        }

        public GolfRound GetGolfRound(Guid id)
        {
            var round = _context.GolfRounds.Where(s => s.Id.Equals(id)).First();

            if (round != null)
            {
                return round;
            }
            else
            {
                throw new Exception("Could not retrieve Golf Round for " + id.ToString());
            }
        }

        public IEnumerable<Guid> GetAllGolfRoundCourseIds(IList<Guid> golfRoundIds)
        {
            try
            {
                return _context.GolfRounds.Where(g => golfRoundIds.Contains(g.Id)).Select(c => c.CourseId);
            }
            catch
            {
                throw new Exception("Could not retrieve Golf Rounds for " + golfRoundIds.ToString());
            }
        }

        public IEnumerable<GolfRound> GetAllGolfRounds()
        {
            try
            {
                return _context.GolfRounds.Select(s => s);
            }
            catch
            {
                throw new Exception("Could not retrieve all Golf Rounds");
            }
        }



        public async Task<Guid> AddGolfRound(GolfRound golfRound)
        {
            try
            {
                await _context.GolfRounds.AddAsync(golfRound);
                await _context.SaveChangesAsync();
                return golfRound.Id;
            }
            catch
            {
                throw new Exception("Could not add Golf Round for " + golfRound.Id.ToString());
            }
        }

        public async Task<Boolean> UpdateGolfRound(GolfRound golfRound)
        {
            try
            {
                _context.GolfRounds.Update(golfRound);
                return(0< await _context.SaveChangesAsync());
                
            }
            catch
            {
                throw new Exception("Could not update Golf Round for " + golfRound.Id.ToString());
            }
        }

        public async Task<Boolean> DeleteGolfRound(GolfRound golfRound)
        {
            try
            {
                _context.GolfRounds.Remove(golfRound);
                return (0 < await _context.SaveChangesAsync());
            }
            catch
            {
                throw new Exception("Could not delete Golf Round for " + golfRound.Id.ToString());
            }
        }

    }
}
