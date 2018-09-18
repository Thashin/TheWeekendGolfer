using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Data
{
    public interface IHandicapAccessLayer
    {
        Task<Handicap> GetLatestHandicap(Guid playerId);

        /// <summary>
        /// Get the latest 20 handicaps for a player
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<IEnumerable<Handicap>> GetOrderedHandicaps(Guid playerId);

        Task<Boolean> AddHandicap(Handicap handicap);
    }
}
