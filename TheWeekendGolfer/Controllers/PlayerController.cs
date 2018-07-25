using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheWeekendGolfer.Data;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Web.Models;

namespace TheWeekendGolfer.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PlayerController : Controller
    {
        PlayerAccessLayer _playerAccessLayer;

        public PlayerController(PlayerAccessLayer playerAccessLayer)
        {
            _playerAccessLayer = playerAccessLayer;
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
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromQuery]Player player)
        {
            if (_playerAccessLayer.AddPlayer(player))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }


        // POST: Player/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Player player)
        {
            if (_playerAccessLayer.UpdatePlayer(player))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        //[HttpDelete]
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

    }
}