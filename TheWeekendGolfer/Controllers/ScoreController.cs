using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheWeekendGolfer.Data;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Web.Models;

namespace TheWeekendGolfer.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ScoreController : Controller
    {
        ScoreAccessLayer _scoreAccessLayer;

        public ScoreController(ScoreAccessLayer scoreAccessLayer)
        {
            _scoreAccessLayer = scoreAccessLayer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                return Ok(_scoreAccessLayer.GetAllScores());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        // GET: Score/Details/5
        public ActionResult Details(Guid id)
        {
            try
            {
                return Ok(_scoreAccessLayer.GetScore(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        // POST: Score/Create
        [Authorize]
        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult AddScores(Score[] scores)
        {
            foreach (Score score in scores)
            {
                if (!_scoreAccessLayer.AddScore(score))
                {
                    return BadRequest();
                }
            }

            return Ok();
        }


        // POST: Score/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Score score)
        {
            if (_scoreAccessLayer.UpdateScore(score))
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