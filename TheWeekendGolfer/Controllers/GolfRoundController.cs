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
    [Route("api/[controller]/[action]")]
    public class GolfRoundController : Controller
    {
        GolfRoundAccessLayer _golfRoundAccessLayer;
        ScoreAccessLayer _scoreAccessLayer;
        PlayerAccessLayer _playerAccessLayer;
        CourseAccessLayer _courseAccessLayer;
        HandicapAccessLayer _handicapAccessLayer;

        public GolfRoundController(GolfRoundAccessLayer golfRoundAccessLayer, ScoreAccessLayer scoreAccessLayer, PlayerAccessLayer playerAccessLayer, CourseAccessLayer courseAccessLayer, HandicapAccessLayer handicapAccessLayer)
        {
            _golfRoundAccessLayer = golfRoundAccessLayer;
            _scoreAccessLayer = scoreAccessLayer;
            _playerAccessLayer = playerAccessLayer;
            _courseAccessLayer = courseAccessLayer;
            _handicapAccessLayer = handicapAccessLayer;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {

                var coursesTask = _courseAccessLayer.GetAllCourses();
                var rounds = _golfRoundAccessLayer.GetAllGolfRounds();
                var scores = _scoreAccessLayer.GetAllScores();
                var players = _playerAccessLayer.GetAllPlayers();


                var joinRoundScores = from round in rounds
                                      join score in scores on round.Id equals score.GolfRoundId
                                      where score.GolfRoundId.Equals(round.Id)
                                      select new { round.Id, round.Date, round.CourseId, score.PlayerId, score.Value };

                var courses = await coursesTask;

                var resolveCourses = from round in joinRoundScores
                                     join course in courses on round.CourseId equals course.Id
                                     where course.Id.Equals(round.CourseId)
                                     select new { round.Id, round.Date, course.Name, course.TeeName, course.Holes,course.Par,course.ScratchRating,course.Slope, round.PlayerId, round.Value };

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
                            players = new List<PlayerViewModel>()
                        });
                    }
                    roundsforDisplay[roundId].players.Add(new PlayerViewModel
                    {
                        player = new Player
                        {
                            FirstName = round.FirstName,
                            LastName = round.LastName,
                            Handicap = round.Handicap
                        },
                        score = round.Value
                    });
                }

                foreach (var roundId in roundsforDisplay.Keys)
                {
                    roundsforDisplay[roundId].players = roundsforDisplay[roundId].players.OrderBy(p => p.score).ToList();
                }


                    return Ok(roundsforDisplay.Values);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        // GET: GolfRound/Details/5
        public ActionResult Details(Guid id)
        {
            try
            {
                return Ok(_golfRoundAccessLayer.GetGolfRound(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        // POST: GolfRound/Create
        [Authorize]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //TODO: Figure out how to separate responsibilities
        public ActionResult Create([FromBody]AddGolfRound golfRound)
        {
            try
            {
                Guid golfRoundId = _golfRoundAccessLayer.AddGolfRound(new GolfRound { Date = golfRound.Date, CourseId = golfRound.CourseId });
                foreach (Score score in golfRound.Scores)
                {
                    score.GolfRoundId = golfRoundId;
                    if (!AddScore(golfRound.Date, score, golfRound.CourseId))
                        return BadRequest();
                }

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }



        // POST: GolfRound/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GolfRound golfRound)
        {
            if (_golfRoundAccessLayer.UpdateGolfRound(golfRound))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }


        private Boolean AddScore(DateTime date, Score score, Guid courseId)
        {

            if (!_scoreAccessLayer.AddScore(score))
            {
                return false;
            }
            else
            {
                CalculateHandicap(date, score, courseId);
                return true;
            }

        }

        private Boolean AddHandicap(Handicap handicap)
        {
            return _handicapAccessLayer.AddHandicap(handicap);
        }

        private async Task<Boolean> CalculateHandicap(DateTime date, Score score, Guid courseId)
        {
            Course course = await _courseAccessLayer.GetCourse(courseId);
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
            return RecalculateHandicap(handicap);
        }

        private Boolean RecalculateHandicap(Handicap handicap)
        {
            IEnumerable<Handicap> handicaps = _handicapAccessLayer.GetOrderedHandicaps(handicap.PlayerId)
                                                                 .OrderBy(h => h.Value);

            int roundsPlayed = handicaps.Count();
            Decimal newHandicap;

            if (roundsPlayed >= 19)
            {
                newHandicap = handicaps.Take(8).Sum(h => h.Value) / 8;
            }
            else if (roundsPlayed >= 17)
            {
                newHandicap = handicaps.Take(7).Sum(h => h.Value) / 7;
            }
            else if (roundsPlayed >= 15)
            {
                newHandicap = handicaps.Take(6).Sum(h => h.Value) / 6;
            }
            else if (roundsPlayed >= 13)
            {
                newHandicap = handicaps.Take(5).Sum(h => h.Value) / 5;
            }
            else if (roundsPlayed >= 11)
            {
                newHandicap = handicaps.Take(4).Sum(h => h.Value) / 4;
            }
            else if (roundsPlayed >= 9)
            {
                newHandicap = handicaps.Take(3).Sum(h => h.Value) / 3;
            }
            else if (roundsPlayed >= 7)
            {
                newHandicap = handicaps.Take(2).Sum(h => h.Value) / 2;
            }
            else
            {
                newHandicap = handicaps.Take(1).Sum(h => h.Value) / 1;
            }
            handicap.CurrentHandicap = newHandicap;
            AddHandicap(handicap);
            return Edit(handicap.PlayerId, newHandicap);

        }

        private Boolean Edit(Guid playerId, Decimal handicap)
        {
            Player player = _playerAccessLayer.GetPlayer(playerId);
            player.Handicap = handicap;
            player.Modified = DateTime.Now;
            return _playerAccessLayer.UpdatePlayer(player);
        }



    }


}