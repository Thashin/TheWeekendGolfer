using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWeekendGolfer.Data;
using TheWeekendGolfer.Models;
using TheWeekendGolfer.Web.Controllers;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Controllers
{
    public class HandicapController : Controller
    {
        CourseController _courseController;
        PlayerController _playerController;
        HandicapAccessLayer _handicapAccessLayer;

        public HandicapController(HandicapAccessLayer handicapAccessLayer,CourseController courseController, PlayerController playerController)
        {
            _handicapAccessLayer = handicapAccessLayer;
            _courseController = courseController;
            _playerController = playerController;

        }

        public Boolean AddHandicap(Handicap handicap)
        {
             _handicapAccessLayer.AddHandicap(handicap);
            return _playerController.RecalculateHandicap(handicap.PlayerId);
        }

        public Boolean CalculateHandicap(DateTime date, Score score, Guid courseId)
        {
            Course course = _courseController.Details(courseId);
            Handicap handicap = new Handicap {
                Date = date,
                PlayerId = score.PlayerId };
            handicap.Value = (score.Value - course.ScratchRating) * Decimal.Parse("113")
                        / course.Slope * Decimal.Parse("0.93");
            if (course.Holes.Contains("-"))
            {
                handicap.Value = handicap.Value * 2;
            }
            return AddHandicap(handicap);
        }

        public IEnumerable<Handicap> GetOrderedHandicaps(Guid playerId)
        {
            return _handicapAccessLayer.GetOrderedHandicaps(playerId);
        }
    }
}