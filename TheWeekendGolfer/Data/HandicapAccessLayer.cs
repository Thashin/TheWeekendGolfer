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

        public Handicap GetLatestHandicap(Guid playerId)
        {
            try
            {
                var orderedHandicaps = GetOrderedHandicaps(playerId);

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
        public IEnumerable<Handicap> GetOrderedHandicaps(Guid playerId)
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

        /// <summary>
        /// Get playedTo scores used in handicap calculation
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public IList<Handicap> GetPlayedTos(Guid playerId)
        {
            try
            {
                var orderedbyDateHandicaps = GetOrderedHandicaps(playerId).Take(20);
                var orderedByPlayedTos = orderedbyDateHandicaps.OrderBy(h=>h.Value);
                int roundsPlayed = orderedByPlayedTos.Count();
                IList<Handicap> usedPlayedTos = new List<Handicap>();
                if (roundsPlayed >= 19)
                {
                    if(roundsPlayed > 19 && orderedByPlayedTos.Take(8).Contains(orderedbyDateHandicaps.ElementAt(19)))
                    {
                        orderedByPlayedTos = orderedByPlayedTos.Where(p => !p.Equals(orderedbyDateHandicaps.ElementAt(19))).OrderBy(h=>h.Value);
                    }
                    usedPlayedTos = orderedByPlayedTos.Take(8).ToList();
                }
                else if (roundsPlayed >= 17)
                {
                    usedPlayedTos = orderedByPlayedTos.Take(7).ToList();
                }
                else if (roundsPlayed >= 15)
                {
                    usedPlayedTos = orderedByPlayedTos.Take(6).ToList();
                }
                else if (roundsPlayed >= 13)
                {
                    usedPlayedTos = orderedByPlayedTos.Take(5).ToList();
                }
                else if (roundsPlayed >= 11)
                {
                    usedPlayedTos = orderedByPlayedTos.Take(4).ToList();
                }
                else if (roundsPlayed >= 9)
                {
                    usedPlayedTos = orderedByPlayedTos.Take(3).ToList();
                }
                else if (roundsPlayed >= 7)
                {
                    usedPlayedTos = orderedByPlayedTos.Take(2).ToList();
                }
                else
                {
                    usedPlayedTos = orderedByPlayedTos.Take(1).ToList();
                }              

                return usedPlayedTos;
            }
            catch
            {
                throw new Exception("Could not retrieve Handicap for " + playerId.ToString());
            }
        }

        public Handicap GetHighestPlayedTo(Guid playerId)
        {
            var orderedByDateHandicaps = GetOrderedHandicaps(playerId).Take(20);
            var usedPlayedTos = GetPlayedTos(playerId).OrderByDescending(p=>p.Value);
            return usedPlayedTos.First();
        }

        public async Task<Boolean> AddHandicap(Handicap handicap)
        {
            try
            {
                await _context.Handicaps.AddAsync(handicap);
                return(0<await _context.SaveChangesAsync());
            }
            catch
            {
                throw new Exception("Could not add Handicap");
            }
        }
    }
}
