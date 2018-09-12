using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Data
{
    public class ScoreAccessLayer
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
                return (Score)_context.Scores.Select(s => s.Id.Equals(id));
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
                return _context.Scores.Where(s=>s.PlayerId.Equals(playerId));
            }
            catch
            {
                throw new Exception("Could not retrieve all scores");
            }
        }

        public Boolean AddScore(Score score)
        {
            try
            {
                _context.Scores.Add(score);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw new Exception("Could not add score for " + score.Id.ToString());
            }
        }

        public Boolean AddScores(IEnumerable<Score> scores)
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

        public Boolean UpdateScore(Score score)
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

        public Boolean DeleteScore(Score score)
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
