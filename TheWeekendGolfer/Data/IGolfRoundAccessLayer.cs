using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IGolfRoundAccessLayer
    {
        GolfRound GetGolfRound(Guid id);

        IEnumerable<Guid> GetAllGolfRoundCourseIds(IList<Guid> golfRoundIds);

        IEnumerable<GolfRound> GetAllGolfRounds();
        Task<Guid> AddGolfRound(GolfRound golfRound);

        Task<Boolean> UpdateGolfRound(GolfRound golfRound);

        Task<Boolean> DeleteGolfRound(GolfRound golfRound);
    }
}
