using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Web.Models;

namespace TheWeekendGolfer.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class PlayerController : Controller
    {
        GolfDbContext _context;

        public PlayerController(GolfDbContext context)
        {
            _context = context;
        }

        // GET: Player
        public ActionResult Index()
        {
            return View();
        }

        // GET: Player/Details/5
        public ActionResult Details(Guid id)
        {
            return View();
        }

        // GET: Player/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Player/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlayerViewModel player)
        {
            try
            {
                _context.Players.Add(
                    new Player {
                       FirstName = player.FirstName,
                       LastName = player.LastName,
                       Handicap = player.Handicap
                    });

                _context.SaveChanges();
                Guid id = _context.Players.LastOrDefault().Id;
                return RedirectToAction(nameof(Details),id);
            }
            catch
            {
                return View();
            }
        }

        // GET: Player/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Player/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Player/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Player/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}