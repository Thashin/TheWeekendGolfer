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
                var rounds = _golfRoundAccessLayer.GetAllGolfRounds();
                var scores = _scoreAccessLayer.GetAllScores();
                var joinRoundScores = from round in rounds
                                      join score in scores on round.Id equals score.GolfRoundId
                                      where score.GolfRoundId.Equals(round.Id) && CourseId.Equals(round.CourseId)
                                      select new { round.Id, round.Date, round.CourseId, score.PlayerId, score.Value };

                IList<Forecast> forecasts = new List<Forecast>();

                foreach (var player in players)
                {
                    var numberOfRounds = scores.Where(s => s.PlayerId.Equals(player.Id)).Count();
                    Decimal average = 0;
                    Decimal pb = 0;
                    Decimal hs = 0;
              
                        var playerScores = joinRoundScores.Where(s => s.PlayerId.Equals(player.Id));
                    if (playerScores.Count() > 0)
                    {
                        average = (decimal)playerScores.Average(s => s.Value);
                        pb = playerScores.Min(s => s.Value);
                        hs = playerScores.Max(s => s.Value);
                    }

                    var highestPlayedTo = _handicapAccessLayer.GetHighestPlayedTo(player.Id).Value;

                    var playedTosinHandicapSixty = _handicapAccessLayer.GetPlayedTos(player.Id);
                    var playedTosinHandicapFiftyFive = _handicapAccessLayer.GetPlayedTos(player.Id);
                    var playedTosinHandicapFifty = _handicapAccessLayer.GetPlayedTos(player.Id);
                    var playedTosinHandicapFortyFive = _handicapAccessLayer.GetPlayedTos(player.Id);
                    var playedTosinHandicapForty = _handicapAccessLayer.GetPlayedTos(player.Id);
                    var playedTosinHandicapThirtyFive = _handicapAccessLayer.GetPlayedTos(player.Id);



                    Decimal sixty = (60 - course.ScratchRating) * Decimal.Parse("113") / course.Slope * Decimal.Parse("0.93");
                    Decimal fiftyFive = (55 - course.ScratchRating) * Decimal.Parse("113") / course.Slope * Decimal.Parse("0.93");
                    Decimal fifty = (50 - course.ScratchRating) * Decimal.Parse("113") / course.Slope * Decimal.Parse("0.93");
                    Decimal fortyFive = (45 - course.ScratchRating) * Decimal.Parse("113") / course.Slope * Decimal.Parse("0.93");
                    Decimal forty = (40 - course.ScratchRating) * Decimal.Parse("113") / course.Slope * Decimal.Parse("0.93");
                    Decimal thirtyFive = (35 - course.ScratchRating) * Decimal.Parse("113") / course.Slope * Decimal.Parse("0.93");

  

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

                    playedTosinHandicapSixty.Add(new Handicap { Value = sixty });
                    playedTosinHandicapFiftyFive.Add(new Handicap { Value = fiftyFive });
                    playedTosinHandicapFifty.Add(new Handicap { Value = fifty });
                    playedTosinHandicapFortyFive.Add(new Handicap { Value = fortyFive });
                    playedTosinHandicapForty.Add(new Handicap { Value = forty });
                    playedTosinHandicapThirtyFive.Add(new Handicap { Value = thirtyFive });

                    if (numberOfRounds < 6)
                    {
                        sixty = playedTosinHandicapSixty.OrderBy(h => h.Value).Min(h => h.Value);
                        fiftyFive = playedTosinHandicapFiftyFive.OrderBy(h => h.Value).Min(h => h.Value);
                        fifty = playedTosinHandicapFifty.OrderBy(h => h.Value).Min(h => h.Value);
                        fortyFive = playedTosinHandicapFortyFive.OrderBy(h => h.Value).Min(h => h.Value);
                        forty = playedTosinHandicapForty.OrderBy(h => h.Value).Min(h => h.Value);
                        thirtyFive = playedTosinHandicapThirtyFive.OrderBy(h => h.Value).Min(h => h.Value);
                    }
                    else if(numberOfRounds < 8)
                    {
                        sixty = playedTosinHandicapSixty.OrderBy(h => h.Value).Take(2).Average(h => h.Value);
                        fiftyFive = playedTosinHandicapFiftyFive.OrderBy(h => h.Value).Take(2).Average(h => h.Value);
                        fifty = playedTosinHandicapFifty.OrderBy(h => h.Value).Take(2).Average(h => h.Value);
                        fortyFive = playedTosinHandicapFortyFive.OrderBy(h => h.Value).Take(2).Average(h => h.Value);
                        forty = playedTosinHandicapForty.OrderBy(h => h.Value).Take(2).Average(h => h.Value);
                        thirtyFive = playedTosinHandicapThirtyFive.OrderBy(h => h.Value).Take(2).Average(h => h.Value);
                    }
                    else if (numberOfRounds < 10)
                    {
                        sixty = playedTosinHandicapSixty.OrderBy(h => h.Value).Take(3).Average(h => h.Value);
                        fiftyFive = playedTosinHandicapFiftyFive.OrderBy(h => h.Value).Take(3).Average(h => h.Value);
                        fifty = playedTosinHandicapFifty.OrderBy(h => h.Value).Take(3).Average(h => h.Value);
                        fortyFive = playedTosinHandicapFortyFive.OrderBy(h => h.Value).Take(3).Average(h => h.Value);
                        forty = playedTosinHandicapForty.OrderBy(h => h.Value).Take(3).Average(h => h.Value);
                        thirtyFive = playedTosinHandicapThirtyFive.OrderBy(h => h.Value).Take(3).Average(h => h.Value);
                    }
                    else if (numberOfRounds < 12)
                    {
                        sixty = playedTosinHandicapSixty.OrderBy(h => h.Value).Take(4).Average(h => h.Value);
                        fiftyFive = playedTosinHandicapFiftyFive.OrderBy(h => h.Value).Take(4).Average(h => h.Value);
                        fifty = playedTosinHandicapFifty.OrderBy(h => h.Value).Take(4).Average(h => h.Value);
                        fortyFive = playedTosinHandicapFortyFive.OrderBy(h => h.Value).Take(4).Average(h => h.Value);
                        forty = playedTosinHandicapForty.OrderBy(h => h.Value).Take(4).Average(h => h.Value);
                        thirtyFive = playedTosinHandicapThirtyFive.OrderBy(h => h.Value).Take(4).Average(h => h.Value);
                    }
                    else if (numberOfRounds < 14)
                    {
                        sixty = playedTosinHandicapSixty.OrderBy(h => h.Value).Take(5).Average(h => h.Value);
                        fiftyFive = playedTosinHandicapFiftyFive.OrderBy(h => h.Value).Take(5).Average(h => h.Value);
                        fifty = playedTosinHandicapFifty.OrderBy(h => h.Value).Take(5).Average(h => h.Value);
                        fortyFive = playedTosinHandicapFortyFive.OrderBy(h => h.Value).Take(5).Average(h => h.Value);
                        forty = playedTosinHandicapForty.OrderBy(h => h.Value).Take(5).Average(h => h.Value);
                        thirtyFive = playedTosinHandicapThirtyFive.OrderBy(h => h.Value).Take(5).Average(h => h.Value);
                    }
                    else if (numberOfRounds < 16)
                    {
                        sixty = playedTosinHandicapSixty.OrderBy(h => h.Value).Take(6).Average(h => h.Value);
                        fiftyFive = playedTosinHandicapFiftyFive.OrderBy(h => h.Value).Take(6).Average(h => h.Value);
                        fifty = playedTosinHandicapFifty.OrderBy(h => h.Value).Take(6).Average(h => h.Value);
                        fortyFive = playedTosinHandicapFortyFive.OrderBy(h => h.Value).Take(6).Average(h => h.Value);
                        forty = playedTosinHandicapForty.OrderBy(h => h.Value).Take(6).Average(h => h.Value);
                        thirtyFive = playedTosinHandicapThirtyFive.OrderBy(h => h.Value).Take(6).Average(h => h.Value);
                    }
                    else if (numberOfRounds < 18)
                    {
                        sixty = playedTosinHandicapSixty.OrderBy(h => h.Value).Take(7).Average(h => h.Value);
                        fiftyFive = playedTosinHandicapFiftyFive.OrderBy(h => h.Value).Take(7).Average(h => h.Value);
                        fifty = playedTosinHandicapFifty.OrderBy(h => h.Value).Take(7).Average(h => h.Value);
                        fortyFive = playedTosinHandicapFortyFive.OrderBy(h => h.Value).Take(7).Average(h => h.Value);
                        forty = playedTosinHandicapForty.OrderBy(h => h.Value).Take(7).Average(h => h.Value);
                        thirtyFive = playedTosinHandicapThirtyFive.OrderBy(h => h.Value).Take(7).Average(h => h.Value);
                    }
                    else
                    {
                        sixty = playedTosinHandicapSixty.OrderBy(h => h.Value).Take(8).Average(h => h.Value);
                        fiftyFive = playedTosinHandicapFiftyFive.OrderBy(h => h.Value).Take(8).Average(h => h.Value);
                        fifty = playedTosinHandicapFifty.OrderBy(h => h.Value).Take(8).Average(h => h.Value);
                        fortyFive = playedTosinHandicapFortyFive.OrderBy(h => h.Value).Take(8).Average(h => h.Value);
                        forty = playedTosinHandicapForty.OrderBy(h => h.Value).Take(8).Average(h => h.Value);
                        thirtyFive = playedTosinHandicapThirtyFive.OrderBy(h => h.Value).Take(8).Average(h => h.Value);
                    }


                    Decimal toLowerScore = (highestPlayedTo / (Decimal.Parse("113") / course.Slope * Decimal.Parse("0.93"))) + course.ScratchRating;

                    forecasts.Add(new Forecast
                    {
                        Player = player,
                        Average = average,
                        PB = pb,
                        HS = hs,
                        ToLowerHandicap = Math.Floor(toLowerScore),
                        Sixty = sixty,
                        FiftyFive = fiftyFive,
                        Fifty = fifty,
                        FortyFive = fortyFive,
                        Forty = forty,
                        ThirtyFive = thirtyFive

                    });
                }

                return Ok(forecasts);
            }
            catch
            {
                return BadRequest();
            }
        }





    }


}