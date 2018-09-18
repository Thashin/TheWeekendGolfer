using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Data
{
    public interface IGolfRoundAccessLayer
    {
         Task<GolfRound> GetGolfRound(Guid id);

         Task<IEnumerable<Guid>> GetAllGolfRoundCourseIds(IList<Guid> golfRoundIds);

         Task<IEnumerable<GolfRound>> GetAllGolfRounds();
         Task<Guid> AddGolfRound(GolfRound golfRound);

         Task<Boolean> UpdateGolfRound(GolfRound golfRound);

         Task<Boolean> DeleteGolfRound(GolfRound golfRound);
    }
}
