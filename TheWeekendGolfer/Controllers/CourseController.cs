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
        CourseAccessLayer _courseAccessLayer;

        public CourseController(CourseAccessLayer courseAccessLayer)
        {
            _courseAccessLayer = courseAccessLayer;
        }

        [HttpGet]
        [Route("/api/Course/Index")]
        public IActionResult Index()
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
        public IActionResult GetCourseNames()
        {
            try
            {
                return Ok(_courseAccessLayer.GetCourseNames());
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet]
        public IActionResult GetAllCoursesOrderedSlope()
        {
            try
            {
                return Ok(_courseAccessLayer.GetAllCoursesOrderedSlope());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetCourseDetails(string courseName,string tee = null)
        {
            try
            {
                if(tee!=null)
                { 
                    return Ok(_courseAccessLayer.GetCourseHoles(courseName, tee));
                }
                else
                {
                    return Ok(_courseAccessLayer.GetCourseTees(courseName));
                }
                

            }
            catch
            {
                return BadRequest();
            }
        }
        
        public Course Details(Guid id)
        {
                return _courseAccessLayer.GetCourse(id);
        }
    }
}