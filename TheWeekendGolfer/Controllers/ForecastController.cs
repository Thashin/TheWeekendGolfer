using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TheWeekendGolfer.Data;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Web.Controllers
{
    /// <summary>
    /// Manages the CRUD operations for GolfRounds
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class ForecastController : Controller
    {
        IGolfRoundAccessLayer _golfRoundAccessLayer;
        IScoreAccessLayer _scoreAccessLayer;
        IPlayerAccessLayer _playerAccessLayer;
        ICourseAccessLayer _courseAccessLayer;
        IHandicapAccessLayer _handicapAccessLayer;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ForecastController(IGolfRoundAccessLayer golfRoundAccessLayer, IScoreAccessLayer scoreAccessLayer, IPlayerAccessLayer playerAccessLayer, ICourseAccessLayer courseAccessLayer, IHandicapAccessLayer handicapAccessLayer)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _golfRoundAccessLayer = golfRoundAccessLayer;
            _playerAccessLayer = playerAccessLayer;
            _courseAccessLayer = courseAccessLayer;
            _scoreAccessLayer = scoreAccessLayer;
            _handicapAccessLayer = handicapAccessLayer;
        }


        /// <summary>
        /// Retrieves all golf rounds in a human-readable format. This is done by resolvng all Guid's to names.
        /// </summary>
        /// <returns>List of human-readable golf rounds</returns>
        [HttpGet]
        public IActionResult Index(Guid CourseId)
        {
            try
            {

                var course = _courseAccessLayer.GetCourse(CourseId);
                var highestPossibleScore = course.Par + 50;
                var lowestPossibleScore = course.Par - 10;
                var players = _playerAccessLayer.GetAllPlayers();
                var rounds = _golfRoundAccessLayer.GetAllGolfRounds().Where(r => r.CourseId.Equals(CourseId));

                IList<Forecast> forecasts = new List<Forecast>();

                foreach (var player in players)
                {
                    var playerScores = _scoreAccessLayer.GetAllPlayerScores(player.Id);
                    IEnumerable<int> playerScoresForCourse = playerScores.Join(rounds,
                        score => score.GolfRoundId,
                        round => round.Id,
                        (score, round) => score.Value);
                    if (playerScoresForCourse.Count() == 0)
                    {
                        playerScoresForCourse = playerScores.Select(s => s.Value);
                    }
                    var playerForecast = forecast(player, playerScoresForCourse, course, lowestPossibleScore, highestPossibleScore);
                    forecasts.Add(playerForecast);
                }

                return Ok(forecasts);
            }
            catch
            {
                return BadRequest();
            }
        }

        private Forecast forecast(Player player, IEnumerable<int> scores, Course course, int lowestPossibleScore, int highestPossibleScore)
        {
            int numberofRoundsPlayed = scores.Count();
            Decimal averageScore = 0;
            Decimal personalBestScore = 0;
            Decimal highestScore = 0;

            if (numberofRoundsPlayed > 0)
            {
                averageScore = (decimal)scores.Average(s => s);
                personalBestScore = scores.Min(s => s);
                highestScore = scores.Max(s => s);
            }
            var highestPlayedTo = _handicapAccessLayer.GetHighestPlayedTo(player.Id).Value;
            Decimal toLowerScore = (highestPlayedTo / (Decimal.Parse("113") / course.Slope * Decimal.Parse("0.93"))) + course.ScratchRating;
            var playedToinHandicapsTargetScore = _handicapAccessLayer.GetPlayedTos(player.Id);
            int numberOfHandicapsConsidered = 8;

            if (numberofRoundsPlayed < 6)
            {
                numberOfHandicapsConsidered = 1;
            }
            else if (numberofRoundsPlayed > 5 && numberofRoundsPlayed < 19)
            {
                numberOfHandicapsConsidered = (int)Math.Floor((double)(numberofRoundsPlayed / 2)) - 1;
            }
            Dictionary<int, Decimal> rangeOfPredictedHandicaps = new Dictionary<int, Decimal>();
            for (int expectedScore = lowestPossibleScore; expectedScore < highestPossibleScore; expectedScore++)
            {
                var expectedPlayedTo = (expectedScore - course.ScratchRating) * Decimal.Parse("113") / course.Slope * Decimal.Parse("0.93");
                Handicap handicap = new Handicap { Value = expectedPlayedTo };
                playedToinHandicapsTargetScore.Add(handicap);
                var expectedHandicap = Math.Round(playedToinHandicapsTargetScore.OrderBy(h => h.Value).Take(numberOfHandicapsConsidered).Average(h => h.Value),2);
                playedToinHandicapsTargetScore.Remove(handicap);
                rangeOfPredictedHandicaps.Add(expectedScore, expectedHandicap);
            }


            return new Forecast
            {
                Player = player,
                Average = averageScore,
                PB = personalBestScore,
                HS = highestScore,
                ToLowerHandicap = Math.Floor(toLowerScore),
                RangeOfPredictedHandicaps = rangeOfPredictedHandicaps,

            };
        }



    }


}