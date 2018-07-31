using System;
using System.Collections.Generic;
using System.Linq;
using TheWeekendGolfer.Models;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Web.Models;

namespace TheWeekendGolfer.Data
{
    public class HandicapAccessLayer
    {
        GolfDbContext _context;

        public HandicapAccessLayer(GolfDbContext context)
        {
            _context = context;
        }

        public Handicap GetLatestHandicap(Guid playerId)
        {
            try
            {
                return GetOrderedHandicaps(playerId)
                                  .FirstOrDefault();
            }
            catch
            {
                throw new Exception("Could not retrieve Handicap for " + playerId.ToString());
            }
        }

        public IEnumerable<Handicap> GetOrderedHandicaps(Guid playerId)
        {
            try
            {
                return _context.Handicaps.Where(h => h.PlayerId.Equals(playerId))
                                  .OrderByDescending(h => h.Date).Take(20);
            }
            catch
            {
                throw new Exception("Could not retrieve Handicap for " + playerId.ToString());
            }
        }

        public Boolean AddHandicap(Handicap handicap)
        {
            try
            {
                _context.Handicaps.Add(handicap);
                return true;
            }
            catch
            {
                throw new Exception("Could not add Handicap");
            }
        }
    }
}
