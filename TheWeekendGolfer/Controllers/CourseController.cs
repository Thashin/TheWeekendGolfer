using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWeekendGolfer.Data;

namespace TheWeekendGolfer.Controllers
{
    public class CourseController : Controller
    {
        CourseAccessLayer _courseAccessLayer;

        public CourseController(CourseAccessLayer courseAccessLayer)
        {
            _courseAccessLayer = courseAccessLayer;
        }

        [HttpGet]
        public IActionResult GetAllCourses()
        {
            try
            {
                return Ok(_courseAccessLayer.GetAllCourses());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        // GET: Course/Details/5
        public ActionResult Details(Guid id)
        {
            try
            {
                return Ok(_courseAccessLayer.GetCourse(id));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}