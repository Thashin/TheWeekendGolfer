using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Web.Models;

namespace TheWeekendGolfer.Data
{
    public class CourseAccessLayer
    {
        GolfDbContext _context;

        public CourseAccessLayer(GolfDbContext context)
        {
            _context = context;
        }

        public Course GetCourse(Guid id)
        {
            try
            {
                return (Course)_context.Courses.Select(s => s.Id.Equals(id));
            }
            catch
            {
                throw new Exception("Could not retrieve Course for " + id.ToString());
            }
        }

        public IEnumerable<Course> GetAllCourses()
        {
            try
            {
                return _context.Courses.Select(s => s);
            }
            catch
            {
                throw new Exception("Could not retrieve all Courses");
            }
        }

        public Boolean AddCourse(Course Course)
        {
            try
            {
                _context.Courses.Add(Course);

                return true;
            }
            catch
            {
                throw new Exception("Could not add Course for " + Course.Id.ToString());
            }
        }

        public Boolean UpdateCourse(Course Course)
        {
            try
            {
                _context.Courses.Update(Course);

                return true;
            }
            catch
            {
                throw new Exception("Could not update Course for " + Course.Id.ToString());
            }
        }

        public Boolean DeleteCourse(Course Course)
        {
            try
            {
                _context.Courses.Remove(Course);

                return true;
            }
            catch
            {
                throw new Exception("Could not delete Course for " + Course.Id.ToString());
            }
        }
    }
}
