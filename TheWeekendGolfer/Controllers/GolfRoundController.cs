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
    public class GolfRoundController : Controller
    {
        IGolfRoundAccessLayer _golfRoundAccessLayer;
        IScoreAccessLayer _scoreAccessLayer;
        IPlayerAccessLayer _playerAccessLayer;
        ICourseAccessLayer _courseAccessLayer;
        IHandicapAccessLayer _handicapAccessLayer;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public GolfRoundController(IGolfRoundAccessLayer golfRoundAccessLayer, IScoreAccessLayer scoreAccessLayer, IPlayerAccessLayer playerAccessLayer, ICourseAccessLayer courseAccessLayer, IHandicapAccessLayer handicapAccessLayer)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _golfRoundAccessLayer = golfRoundAccessLayer;
            _scoreAccessLayer = scoreAccessLayer;
            _playerAccessLayer = playerAccessLayer;
            _courseAccessLayer = courseAccessLayer;
            _handicapAccessLayer = handicapAccessLayer;
        }


        /// <summary>
        /// Retrieves all golf rounds in a human-readable format. This is done by resolvng all Guid's to names.
        /// </summary>
        /// <returns>List of human-readable golf rounds</returns>
        [HttpGet]
        public IActionResult Index()
        {
            try
            {

                var courses = _courseAccessLayer.GetAllCourses();
                var players = _playerAccessLayer.GetAllPlayers();
                var rounds = _golfRoundAccessLayer.GetAllGolfRounds();
                var scores = _scoreAccessLayer.GetAllScores();


                var joinRoundScores = from round in rounds
                                      join score in scores on round.Id equals score.GolfRoundId
                                      where score.GolfRoundId.Equals(round.Id)
                                      select new { round.Id, round.Date, round.CourseId, score.PlayerId, score.Value };

                var resolveCourses = from round in joinRoundScores
                                     join course in courses on round.CourseId equals course.Id
                                     where course.Id.Equals(round.CourseId)
                                     select new { round.Id, round.Date, course.Name, course.TeeName, course.Holes, course.Par, course.ScratchRating, course.Slope, round.PlayerId, round.Value };

                var resolvePlayers = from round in resolveCourses
                                     join player in players on round.PlayerId equals player.Id
                                     where player.Id.Equals(round.PlayerId)
                                     select new { round.Id, round.Date, round.Name, round.TeeName, round.Holes, round.Par, round.ScratchRating, round.Slope, player.FirstName, player.LastName, player.Handicap, round.Value };

                Dictionary<Guid, GolfRoundViewModel> roundsforDisplay = new Dictionary<Guid, GolfRoundViewModel>();

                foreach (var round in resolvePlayers)
                {
                    var roundId = round.Id;
                    if (!roundsforDisplay.ContainsKey(roundId))
                    {
                        roundsforDisplay.Add(roundId, new GolfRoundViewModel
                        {
                            Date = round.Date,
                            Course = new Course
                            {
                                Name = round.Name,
                                TeeName = round.TeeName,
                                Holes = round.Holes,
                                Par = round.Par,
                                ScratchRating = round.ScratchRating,
                                Slope = round.Slope
                            },
                            Players = new List<PlayerViewModel>()
                        });
                    }
                    roundsforDisplay[roundId].Players.Add(new PlayerViewModel
                    {
                        Player = new Player
                        {
                            FirstName = round.FirstName,
                            LastName = round.LastName,
                            Handicap = round.Handicap
                        },
                        Score = round.Value
                    });
                }

                foreach (var roundId in roundsforDisplay.Keys)
                {
                    roundsforDisplay[roundId].Players = roundsforDisplay[roundId].Players.OrderBy(p => p.Score).ToList();
                }


                return Ok(roundsforDisplay.Values);
            }
            catch
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Adds a golf round.
        /// Only users who are logged in can create a golf round
        /// </summary>
        /// <param name="golfRound">
        /// Cannot contain multiple scores for the same player
        /// Cannot contain more than 4 scores
        /// </param>
        /// <returns>Whether the round was created successfully or not</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]AddGolfRound golfRound)
        {
            try
            {
                Guid golfRoundId = await _golfRoundAccessLayer.AddGolfRound(new GolfRound { Date = golfRound.Date, CourseId = golfRound.CourseId });
                foreach (Score score in golfRound.Scores)
                {
                    score.GolfRoundId = golfRoundId;
                    if (!await AddScore(golfRound.Date, score, golfRound.CourseId))
                        return BadRequest();
                }

                return Ok(true);
            }
            catch
            {
                return BadRequest(false);
            }
        }



        private async Task<Boolean> AddScore(DateTime date, Score score, Guid courseId)
        {

            if (!await _scoreAccessLayer.AddScore(score))
            {
                return false;
            }
            else
            {

                return await CalculateHandicap(date, score, courseId);
            }

        }

        private async Task<Boolean> AddHandicap(Handicap handicap)
        {
            return await _handicapAccessLayer.AddHandicap(handicap);
        }

        private async Task<Boolean> CalculateHandicap(DateTime date, Score score, Guid courseId)
        {
            Course course = _courseAccessLayer.GetCourse(courseId);
            Handicap handicap = new Handicap
            {
                Date = date,
                PlayerId = score.PlayerId
            };
            handicap.Value = (score.Value - course.ScratchRating) * Decimal.Parse("113")
                        / course.Slope * Decimal.Parse("0.93");
            if (course.Holes.Contains("-"))
            {
                handicap.Value = handicap.Value * 2;
            }
            return await RecalculateHandicap(handicap);
        }

        private async Task<Boolean> RecalculateHandicap(Handicap handicap)
        {
            IEnumerable<Handicap> allHandicaps = _handicapAccessLayer.GetOrderedHandicaps(handicap.PlayerId);
            var handicapsList = allHandicaps.Take(19).ToList();
            handicapsList.Add(handicap);
            handicapsList = handicapsList.OrderBy(h => h.Value).ToList();
            int roundsPlayed = handicapsList.Count();
            Decimal newHandicap;

            if (roundsPlayed >= 19)
            {
                newHandicap = handicapsList.Take(8).Sum(h => h.Value) / 8;
            }
            else if (roundsPlayed >= 17)
            {
                newHandicap = handicapsList.Take(7).Sum(h => h.Value) / 7;
            }
            else if (roundsPlayed >= 15)
            {
                newHandicap = handicapsList.Take(6).Sum(h => h.Value) / 6;
            }
            else if (roundsPlayed >= 13)
            {
                newHandicap = handicapsList.Take(5).Sum(h => h.Value) / 5;
            }
            else if (roundsPlayed >= 11)
            {
                newHandicap = handicapsList.Take(4).Sum(h => h.Value) / 4;
            }
            else if (roundsPlayed >= 9)
            {
                newHandicap = handicapsList.Take(3).Sum(h => h.Value) / 3;
            }
            else if (roundsPlayed >= 7)
            {
                newHandicap = handicapsList.Take(2).Sum(h => h.Value) / 2;
            }
            else
            {
                newHandicap = handicapsList.Take(1).Sum(h => h.Value) / 1;
            }
            handicap.CurrentHandicap = newHandicap;
            await AddHandicap(handicap);
            return await Edit(handicap.PlayerId, newHandicap);

        }

        private async Task<Boolean> Edit(Guid playerId, Decimal handicap)
        {
            Player player = _playerAccessLayer.GetPlayer(playerId);
            player.Handicap = handicap;
            player.Modified = DateTime.Now;
            return await _playerAccessLayer.UpdatePlayer(player);
        }



    }


}