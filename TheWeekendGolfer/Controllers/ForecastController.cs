using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TheWeekendGolfer.Controllers;
using TheWeekendGolfer.Data;
using TheWeekendGolfer.Web.Data;
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
                    var playerForecast = forecast(player, playerScoresForCourse, course);
                    forecasts.Add(playerForecast);
                }

                return Ok(forecasts);
            }
            catch
            {
                return BadRequest();
            }
        }

        private Forecast forecast(Player player, IEnumerable<int> scores, Course course)
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

            var playedToinHandicapsTargetScore = _handicapAccessLayer.GetPlayedTos(player.Id);



            Decimal sixty = (120 - course.ScratchRating) * Decimal.Parse("113") / course.Slope * Decimal.Parse("0.93");
            Decimal fiftyFive = (110 - course.ScratchRating) * Decimal.Parse("113") / course.Slope * Decimal.Parse("0.93");
            Decimal fifty = (100 - course.ScratchRating) * Decimal.Parse("113") / course.Slope * Decimal.Parse("0.93");
            Decimal fortyFive = (90 - course.ScratchRating) * Decimal.Parse("113") / course.Slope * Decimal.Parse("0.93");
            Decimal forty = (80 - course.ScratchRating) * Decimal.Parse("113") / course.Slope * Decimal.Parse("0.93");
            Decimal thirtyFive = (70 - course.ScratchRating) * Decimal.Parse("113") / course.Slope * Decimal.Parse("0.93");



            if (course.Holes.Contains("-"))
            {
                highestPlayedTo = highestPlayedTo / 2;
                sixty = sixty * 2;
                fiftyFive = fiftyFive * 2;
                fifty = fifty * 2;
                fortyFive = fortyFive * 2;
                forty = forty * 2;
                thirtyFive = thirtyFive * 2;
            }


            int numberOfHandicapsConsidered = 8;

            if (numberofRoundsPlayed < 6)
            {
                numberOfHandicapsConsidered = 1;
            }
            else if (numberofRoundsPlayed > 5 && numberofRoundsPlayed < 19)
            {
                numberOfHandicapsConsidered = (int)Math.Floor((double)(numberofRoundsPlayed / 2)) - 1;
            }

            Handicap handicap = new Handicap { Value = sixty };
            playedToinHandicapsTargetScore.Add(handicap);
            sixty = playedToinHandicapsTargetScore.OrderBy(h => h.Value).Take(numberOfHandicapsConsidered).Average(h => h.Value);
            playedToinHandicapsTargetScore.Remove(handicap);
            handicap.Value = fiftyFive;
            playedToinHandicapsTargetScore.Add(handicap);
            fiftyFive = playedToinHandicapsTargetScore.OrderBy(h => h.Value).Take(numberOfHandicapsConsidered).Average(h => h.Value);
            playedToinHandicapsTargetScore.Remove(handicap);
            handicap.Value = fifty;
            playedToinHandicapsTargetScore.Add(handicap);
            fifty = playedToinHandicapsTargetScore.OrderBy(h => h.Value).Take(numberOfHandicapsConsidered).Average(h => h.Value);
            playedToinHandicapsTargetScore.Remove(handicap);
            handicap.Value = fortyFive;
            playedToinHandicapsTargetScore.Add(handicap);
            fortyFive = playedToinHandicapsTargetScore.OrderBy(h => h.Value).Take(numberOfHandicapsConsidered).Average(h => h.Value);
            playedToinHandicapsTargetScore.Remove(handicap);
            handicap.Value = forty;
            playedToinHandicapsTargetScore.Add(handicap);
            forty = playedToinHandicapsTargetScore.OrderBy(h => h.Value).Take(numberOfHandicapsConsidered).Average(h => h.Value);
            playedToinHandicapsTargetScore.Remove(handicap);
            handicap.Value = thirtyFive;
            playedToinHandicapsTargetScore.Add(handicap);
            thirtyFive = playedToinHandicapsTargetScore.OrderBy(h => h.Value).Take(numberOfHandicapsConsidered).Average(h => h.Value);
            playedToinHandicapsTargetScore.Remove(handicap);


            Decimal toLowerScore = (highestPlayedTo / (Decimal.Parse("113") / course.Slope * Decimal.Parse("0.93"))) + course.ScratchRating;

            return new Forecast
            {
                Player = player,
                Average = averageScore,
                PB = personalBestScore,
                HS = highestScore,
                ToLowerHandicap = Math.Floor(toLowerScore),
                Sixty = sixty,
                FiftyFive = fiftyFive,
                Fifty = fifty,
                FortyFive = fortyFive,
                Forty = forty,
                ThirtyFive = thirtyFive

            };
        }



    }


}