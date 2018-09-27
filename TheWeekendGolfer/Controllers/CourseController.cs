using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWeekendGolfer.Data;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CourseController : Controller
    {
        ICourseAccessLayer _courseAccessLayer;

        public CourseController(ICourseAccessLayer courseAccessLayer)
        {
            _courseAccessLayer = courseAccessLayer;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Index()
        {
            var courses = await _courseAccessLayer.GetAllCourses();
            if (courses.Count() > 0)
            {
                return Ok(courses);
            }
            else
            {
                return NotFound("No courses were found");
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCourseNames()
        {
            var courseNames = await _courseAccessLayer.GetCourseNames();

            if (courseNames.Count() > 0)
            {
                return Ok(courseNames);
            }
            else
            {
                return NotFound("Could not get course names");
            }
        }


        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCourseDetails(string courseName, string tee = null)
        {

            if (tee != null)
            {
                var holes = await _courseAccessLayer.GetCourseHoles(courseName, tee);
                if (holes.Count() > 0)
                {
                    return Ok(holes);
                }
                else
                {
                    return NotFound("Could not find holes for course");
                }
            }
            else
            {
                var tees = await _courseAccessLayer.GetCourseTees(courseName);
                if (tees.Count() > 0)
                {
                    return Ok(tees);
                }
                else
                {
                    return NotFound("Could not find tees for course");
                }
            }


        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Details(Guid id)
        {
            var details = await _courseAccessLayer.GetCourse(id);
            if (details != null)
            {
                return Ok(details);
            }
            else
            {
                return NotFound("Could not find course");
            }
        }
    }
}