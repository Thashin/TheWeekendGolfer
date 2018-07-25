using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWeekendGolfer.Data;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Web.Models;
using TheWeekendGolfer.Web.Models.GolfRoundViewModels;

namespace TheWeekendGolfer.Web.Controllers
{
    public class GolfRoundController : Controller
    {
        GolfRoundAccessLayer _golfRoundAccessLayer;

        public GolfRoundController(GolfRoundAccessLayer playerAccessLayer)
        {
            _golfRoundAccessLayer = playerAccessLayer;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromQuery]GolfRound player)
        {
            if (_golfRoundAccessLayer.AddGolfRound(player))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }


        // POST: GolfRound/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GolfRound player)
        {
            if (_golfRoundAccessLayer.UpdateGolfRound(player))
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