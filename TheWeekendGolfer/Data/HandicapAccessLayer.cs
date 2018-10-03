using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Models;
using TheWeekendGolfer.Web.Data;

namespace TheWeekendGolfer.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class HandicapAccessLayer : IHandicapAccessLayer
    {
        GolfDbContext _context;

        public HandicapAccessLayer(GolfDbContext context)
        {
            _context = context;
        }

        public async Task<Handicap> GetLatestHandicap(Guid playerId)
        {
            try
            {
                var orderedHandicaps = await GetOrderedHandicaps(playerId);

                return orderedHandicaps.FirstOrDefault();
            }
            catch
            {
                throw new Exception("Could not retrieve Handicap for " + playerId.ToString());
            }
        }

        /// <summary>
        /// Get the latest handicaps for a player
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Handicap>> GetOrderedHandicaps(Guid playerId)
        {
            try
            {
                var orderedHandicaps = _context.Handicaps.Where(h => h.PlayerId.Equals(playerId));
                return orderedHandicaps.OrderByDescending(h => h.Date);
            }
            catch
            {
                throw new Exception("Could not retrieve Handicap for " + playerId.ToString());
            }
        }

        public async Task<Boolean> AddHandicap(Handicap handicap)
        {
            try
            {
                _context.Handicaps.Add(handicap);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw new Exception("Could not add Handicap");
            }
        }
    }
}
