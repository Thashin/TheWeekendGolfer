using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public GolfRoundController(GolfRoundAccessLayer golfRoundAccessLayer,ScoreAccessLayer scoreAccessLayer)
        {
            _golfRoundAccessLayer = golfRoundAccessLayer;
            _scoreAccessLayer = scoreAccessLayer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                return Ok(_golfRoundAccessLayer.GetAllGolfRounds());
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
        public ActionResult Create([FromBody]AddGolfRound golfRound)
        {
            try
            {
                Guid golfRoundId = _golfRoundAccessLayer.AddGolfRound(new GolfRound { Date = golfRound.Date, CourseId = golfRound.CourseId });
                return Ok(golfRoundId);
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