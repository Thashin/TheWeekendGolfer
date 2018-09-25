using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Data
{
    public class ScoreAccessLayer:IScoreAccessLayer
    {
        GolfDbContext _context;

        public ScoreAccessLayer(GolfDbContext context)
        {
            _context = context;
        }

        public async Task<Score> GetScore(Guid id)
        {
            try
            {
                return _context.Scores.Where(s => s.Id.Equals(id)).First();
            }
            catch
            {
                throw new Exception("Could not retrieve score for " + id.ToString());
            }
        }

        public async Task<IEnumerable<Score>> GetAllScores()
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

        public async Task<IEnumerable<Score>> GetAllPlayerScores(Guid playerId)
        {
            var scores  = _context.Scores.Where(s => s.PlayerId.Equals(playerId));
            if(0<scores.Count())
            {
                return scores;
            }
            else
            {
                throw new Exception("Could not retrieve all scores");
            }
        }

        public async Task<Boolean> AddScore(Score score)
        {
            try
            {
                if (_context.Scores.Where(s => s.GolfRoundId.Equals(score.GolfRoundId)).Count() < 4)
                {
                    _context.Scores.Add(score);
                    _context.SaveChanges();
                }
                    return true;
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
                    AddScore(score);
                }
                return true;
            }
            catch
            {
                throw new Exception("Could not add score any scores");
            }

        }

        public async Task<Boolean> UpdateScore(Score score)
        {
            try
            {
                _context.Scores.Update(score);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw new Exception("Could not update score for " + score.Id.ToString());
            }
        }

        public async Task<Boolean> DeleteScore(Score score)
        {
            try
            {
                _context.Scores.Remove(score);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw new Exception("Could not delete score for " + score.Id.ToString());
            }
        }

    }
}
