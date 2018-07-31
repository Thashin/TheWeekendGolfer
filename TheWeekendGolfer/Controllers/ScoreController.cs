using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheWeekendGolfer.Controllers;
using TheWeekendGolfer.Data;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ScoreController : Controller
    {
        ScoreAccessLayer _scoreAccessLayer;
        HandicapController _handicapController;

        public ScoreController(ScoreAccessLayer scoreAccessLayer, HandicapController handicapController)
        {
            _scoreAccessLayer = scoreAccessLayer;
            _handicapController = handicapController;
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

        public Boolean AddScore(DateTime date, Score score, Guid courseId)
        {

            if (!_scoreAccessLayer.AddScore(score))
            {
                return false;
            }
            else
            {
                _handicapController.CalculateHandicap(date, score, courseId);

                return true;
            }

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