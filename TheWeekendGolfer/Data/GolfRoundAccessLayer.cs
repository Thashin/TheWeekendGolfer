using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Web.Models;

namespace TheWeekendGolfer.Data
{
    public class GolfRoundAccessLayer
    {
        GolfDbContext _context;

        public GolfRoundAccessLayer(GolfDbContext context)
        {
            _context = context;
        }

        public GolfRound GetGolfRound(Guid id)
        {
            try
            {
                return (GolfRound)_context.GolfRounds.Select(s => s.Id.Equals(id));
            }
            catch
            {
                throw new Exception("Could not retrieve Golf Round for " + id.ToString());
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

        public Boolean AddGolfRound(GolfRound GolfRound)
        {
            try
            {
                _context.GolfRounds.Add(GolfRound);

                return true;
            }
            catch
            {
                throw new Exception("Could not add Golf Round for " + GolfRound.Id.ToString());
            }
        }

        public Boolean UpdateGolfRound(GolfRound GolfRound)
        {
            try
            {
                _context.GolfRounds.Update(GolfRound);

                return true;
            }
            catch
            {
                throw new Exception("Could not update Golf Round for " + GolfRound.Id.ToString());
            }
        }

        public Boolean DeleteGolfRound(GolfRound GolfRound)
        {
            try
            {
                _context.GolfRounds.Remove(GolfRound);

                return true;
            }
            catch
            {
                throw new Exception("Could not delete Golf Round for " + GolfRound.Id.ToString());
            }
        }

    }
}
