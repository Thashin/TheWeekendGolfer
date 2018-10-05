using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Data
{


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ScoreAccessLayer : IScoreAccessLayer
  {
        GolfDbContext _context;

        public ScoreAccessLayer(GolfDbContext context)
        {
            _context = context;
        }


        public Score GetScore(Guid id)
        {
            try
            {
                return  _context.Scores.Where(s => s.Id.Equals(id)).First();
            }
            catch
            {
                throw new Exception("Could not retrieve score for " + id.ToString());
            }
        }


        public IEnumerable<Score> GetAllScores()
        {
            try
            {
                return _context.Scores.Select(s => s);
            }
            catch
            {
                throw new Exception("Could not retrieve all scores");
            }
        }


        public IEnumerable<Score> GetAllPlayerScores(Guid playerId)
        {
            
            try 
            {
                var scores = _context.Scores.Where(s => s.PlayerId.Equals(playerId));
                return scores;
            }
            catch
            {
                throw new Exception("Could not retrieve all scores");
            }
        }


        public async Task<Boolean> AddScore(Score score)
        {
            try
            {
                if (_context.Scores.Where(s => s.GolfRoundId.Equals(score.GolfRoundId)).Count() < 4 &&
                    _context.Scores.Where(s => s.PlayerId.Equals(score.PlayerId) && s.GolfRoundId.Equals(score.GolfRoundId)).Count() < 1)
                {
                    await _context.Scores.AddAsync(score);
                    return 0 < await _context.SaveChangesAsync();
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw new Exception("Could not add score for " + score.Id.ToString());
            }
        }



        public async Task<Boolean> AddScores(IEnumerable<Score> scores)
        {
            try
            {
                foreach (Score score in scores)
                {
                    if(!await AddScore(score))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                throw new Exception("Could not add score any scores");
            }

        }

    }
}
