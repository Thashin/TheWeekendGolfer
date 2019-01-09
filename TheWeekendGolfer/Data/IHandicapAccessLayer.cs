using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IHandicapAccessLayer
    {
        Handicap GetLatestHandicap(Guid playerId);

        /// <summary>
        /// Get the latest 20 handicaps for a player
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        IEnumerable<Handicap> GetOrderedHandicaps(Guid playerId);

        /// <summary>
        /// Get the highest played to value that is included in the handicap calculation from the last 19 rounds.
        /// 
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Handicap GetHighestPlayedTo(Guid playerId);

        IList<Handicap> GetPlayedTos(Guid playerId);

        Task<Boolean> AddHandicap(Handicap handicap);
    }
}
