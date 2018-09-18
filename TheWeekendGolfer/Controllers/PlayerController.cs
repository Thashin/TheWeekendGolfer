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
        IHandicapAccessLayer _handicapAccessLayer;
        IPlayerAccessLayer _playerAccessLayer;
        ICourseAccessLayer _courseAccessLayer;
        IScoreAccessLayer _scoreAccessLayer;
        IGolfRoundAccessLayer _golfRoundAccessLayer;


        public PlayerController(IHandicapAccessLayer handicapAccessLayer, IPlayerAccessLayer playerAccessLayer, ICourseAccessLayer courseAccessLayer, IScoreAccessLayer scoreAccessLayer, IGolfRoundAccessLayer golfRoundAccessLayer)
        {
            _handicapAccessLayer = handicapAccessLayer;
            _playerAccessLayer = playerAccessLayer;
            _courseAccessLayer = courseAccessLayer;
            _scoreAccessLayer = scoreAccessLayer;
            _golfRoundAccessLayer = golfRoundAccessLayer;
        }
        



        [HttpGet]
        public async Task<IEnumerable<Handicap>> GetOrderedHandicaps(Guid PlayerId)
        {
            return await _handicapAccessLayer.GetOrderedHandicaps(PlayerId);
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
        public async Task<IActionResult> Create([FromBody]Player player)
        {
            Guid playerId = await _playerAccessLayer.AddPlayer(player);
            if (playerId != null)
            {
                if(player.Handicap==null)
                {
                    return Ok();
                }
                else if(! await _handicapAccessLayer.AddHandicap(new Handicap { Date = DateTime.Now, PlayerId = playerId, Value = player.Handicap.Value, CurrentHandicap = player.Handicap.Value }))
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
        public async Task<IActionResult> GetAllPlayerRoundCourses(Guid playerId)
        {
            var rounds = await _scoreAccessLayer.GetAllPlayerScores(playerId);
            var courses = await  _golfRoundAccessLayer.GetAllGolfRoundCourseIds(rounds.Select(s => s.GolfRoundId).ToList());
            return Ok(_courseAccessLayer.GetCourseStats(courses.ToList()));
        }







    }
}