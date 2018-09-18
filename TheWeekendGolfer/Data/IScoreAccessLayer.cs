using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Data
{
    public interface IScoreAccessLayer
    {
        Task<Score> GetScore(Guid id);

        Task<IEnumerable<Score>> GetAllScores();

        Task<IEnumerable<Score>> GetAllPlayerScores(Guid playerId);

        Task<Boolean> AddScore(Score score);

        Task<Boolean> AddScores(IEnumerable<Score> scores);

        Task<Boolean> UpdateScore(Score score);

        Task<Boolean> DeleteScore(Score score);
    }
}
