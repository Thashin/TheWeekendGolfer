using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheWeekendGolfer.Controllers;
using TheWeekendGolfer.Data;
using TheWeekendGolfer.Models;
using TheWeekendGolfer.Web.Data;

namespace TheWeekendGolfer.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PlayerController : Controller
    {
        HandicapAccessLayer _handicapAccessLayer;
        PlayerAccessLayer _playerAccessLayer;
        CourseAccessLayer _courseAccessLayer;
        ScoreAccessLayer _scoreAccessLayer;
        GolfRoundAccessLayer _golfRoundAccessLayer;


        public PlayerController(HandicapAccessLayer handicapAccessLayer, PlayerAccessLayer playerAccessLayer, CourseAccessLayer courseAccessLayer, ScoreAccessLayer scoreAccessLayer, GolfRoundAccessLayer golfRoundAccessLayer)
        {
            _handicapAccessLayer = handicapAccessLayer;
            _playerAccessLayer = playerAccessLayer;
            _courseAccessLayer = courseAccessLayer;
            _scoreAccessLayer = scoreAccessLayer;
            _golfRoundAccessLayer = golfRoundAccessLayer;
        }
        



        [HttpGet]
        public IEnumerable<Handicap> GetOrderedHandicaps(Guid PlayerId)
        {
            return _handicapAccessLayer.GetOrderedHandicaps(PlayerId);
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                return Ok(_playerAccessLayer.GetAllPlayers());
            }
            catch
            {
                return BadRequest();
            }
        }



        [HttpGet]
        // GET: Player/Details/5
        public ActionResult Details(Guid id)
        {
            try
            {
                return Ok(_playerAccessLayer.GetPlayer(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        // POST: Player/Create
        [HttpPost]
   //     [ValidateAntiForgeryToken]
        public ActionResult Create([FromBody]Player player)
        {
            Guid playerId = _playerAccessLayer.AddPlayer(player);
            if (playerId != null)
            {
                if(player.Handicap==null)
                {
                    return Ok();
                }
                else if(!_handicapAccessLayer.AddHandicap(new Handicap { Date = DateTime.Now, PlayerId = playerId, Value = player.Handicap.Value, CurrentHandicap = player.Handicap.Value }))
                {
                    return BadRequest();
                }
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet]
        public IActionResult GetAllPlayerRoundCourses(Guid playerId)
        {
            var rounds = _scoreAccessLayer.GetAllPlayerScores(playerId).Select(s => s.GolfRoundId).ToList();
            var courses = _golfRoundAccessLayer.GetAllGolfRoundCourseIds(rounds).ToList();
            return Ok(_courseAccessLayer.GetCourseStats(courses));
        }







    }
}