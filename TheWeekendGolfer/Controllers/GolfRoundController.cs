using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Web.Models;
using TheWeekendGolfer.Web.Models.GolfRoundViewModels;

namespace TheWeekendGolfer.Web.Controllers
{
    public class GolfRoundController : Controller
    {
        GolfDbContext _context;

        public GolfRoundController(GolfDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            GolfRoundViewModel golfRound = new GolfRoundViewModel
            {
                Courses = _context.Courses.Select(c => c.Name),
                Players = _context.Players.Select(p => p.FirstName + " " + p.LastName)
            };
            return View(golfRound);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Add(GolfRound golfRound)
        {
            return View();
        }
    }
}