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
using TheWeekendGolfer.Web.Models;
using TheWeekendGolfer.Web.Models.GolfRoundViewModels;

namespace TheWeekendGolfer.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class GolfRoundController : Controller
    {
        GolfRoundAccessLayer _golfRoundAccessLayer;
        ScoreAccessLayer _scoreAccessLayer;
        PlayerAccessLayer _playerAccessLayer;
        CourseAccessLayer _courseAccessLayer;
        ScoreController _scoreController;

        public GolfRoundController(GolfRoundAccessLayer golfRoundAccessLayer,ScoreAccessLayer scoreAccessLayer, PlayerAccessLayer playerAccessLayer, CourseAccessLayer courseAccessLayer,ScoreController scoreController)
        {
            _golfRoundAccessLayer = golfRoundAccessLayer;
            _scoreAccessLayer = scoreAccessLayer;
            _playerAccessLayer = playerAccessLayer;
            _courseAccessLayer = courseAccessLayer;
            _scoreController = scoreController;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var rounds = _golfRoundAccessLayer.GetAllGolfRounds();
                var scores = _scoreAccessLayer.GetAllScores();
                var courses = _courseAccessLayer.GetAllCourses();
                var players = _playerAccessLayer.GetAllPlayers();

                var joinRoundScores = from round in rounds
                            join score in scores on round.Id equals score.GolfRoundId
                            where score.GolfRoundId.Equals(round.Id)
                            select new { round.Date,round.CourseId, score .PlayerId,score.Value};

                var resolveCourses = from round in joinRoundScores
                                     join course in courses on round.CourseId equals course.Id
                                     where course.Id.Equals(round.CourseId)
                                     select new { round.Date, course.Name, course.TeeName, course.Holes, round.PlayerId, round.Value };

                var resolvePlayers = from round in resolveCourses
                                     join player in players on round.PlayerId equals player.Id
                                     where player.Id.Equals(round.PlayerId)
                                     select new { round.Date, round.Name, round.TeeName, round.Holes, player.FirstName,player.LastName,player.Handicap, round.Value };


                return Ok(resolvePlayers);
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
     //   [ValidateAntiForgeryToken]
        //TODO: Figure out how to separate responsibilities
        public ActionResult Create([FromBody]AddGolfRound golfRound)
        {
            try
            {
                Guid golfRoundId = _golfRoundAccessLayer.AddGolfRound(new GolfRound { Date = golfRound.Date, CourseId = golfRound.CourseId });
                foreach(Score score in golfRound.Scores)
                {
                    score.GolfRoundId = golfRoundId;
                    if (!_scoreController.AddScore(golfRound.Date,score, golfRound.CourseId))
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
    }
}