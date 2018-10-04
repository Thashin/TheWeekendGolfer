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

    /// <summary>
    /// Manages CRUD operations for Courses
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class CourseController : Controller
    {
        ICourseAccessLayer _courseAccessLayer;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public CourseController(ICourseAccessLayer courseAccessLayer)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _courseAccessLayer = courseAccessLayer;
        }


        /// <summary>
        /// Retrieves all available courses
        /// </summary>
        /// <returns>
        /// All available courses.
        /// Throws an exception if there are no courses found
        /// </returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Index()
        {
            var courses = _courseAccessLayer.GetAllCourses();
            if (courses.Count() > 0)
            {
                return Ok(courses);
            }
            else
            {
                return NotFound("No courses were found");
            }
        }


        /// <summary>
        /// Retrieves all available course names
        /// </summary>
        /// <returns>
        /// All available courses.
        /// Throws an exception if there are no courses found
        /// </returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetCourseNames()
        {
            var courseNames = _courseAccessLayer.GetCourseNames();

            if (courseNames.Count() > 0)
            {
                return Ok(courseNames);
            }
            else
            {
                return NotFound("Could not get course names");
            }
        }

        /// <summary>
        /// Given a course name, retrieve the tees available for that course(Red Women, Blue Men etc)
        /// Given a course name and a tee name, retrive all possible hole configurations(18, 1-9, 10-18)
        /// </summary>
        /// <param name="courseName">Name of the course</param>
        /// <param name="tee">(Optional) Name of the tee</param>
        /// <returns>List of tees or hole configurations</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetCourseDetails(string courseName, string tee = null)
        {
            if (tee != null)
            {
                var holes = _courseAccessLayer.GetCourseHoles(courseName, tee);
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
                var tees =  _courseAccessLayer.GetCourseTees(courseName);
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

        /// <summary>
        /// Retrieve details of a course by Id
        /// Name, Location, Tee Name, Holes, Slope, Scratch Rating, Par
        /// </summary>
        /// <param name="id">Guid of course</param>
        /// <returns>A course by Id</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Details(Guid id)
        {
            var details = _courseAccessLayer.GetCourse(id);
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