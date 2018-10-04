using System;
using System.Collections.Generic;
using System.Linq;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Models;
using System.Threading.Tasks;

namespace TheWeekendGolfer.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class CourseAccessLayer : ICourseAccessLayer
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
                return _context.Courses.Where(s => s.Id.Equals(id)).First();
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

        public IEnumerable<string> GetCourseNames()
        {
            var names = _context.Courses.GroupBy(c => c.Name).Select(c => c.First().Name);

            if (names != null)
            {
                return names;
            }
            else
            {
                throw new Exception("Could not retrieve all course names");
            }
        }

        public IEnumerable<string> GetCourseTees(string courseName)
        {
            var tees = _context.Courses.Where(c => c.Name.Equals(courseName)).
                                        GroupBy(c => c.TeeName).
                                        Select(c => c.First().TeeName);
            if (0 < tees.Count())
            {
                return tees;
            }
            else
            {
                throw new Exception("Could not retrieve all course tees");
            }
        }

        public IEnumerable<Course> GetCourseHoles(string courseName, string courseTee)
        {
            var holes = _context.Courses.Where(c => c.Name.Equals(courseName) &&
                                                   c.TeeName.Equals(courseTee))
                                       .Select(c => c);
            if (0 < holes.Count())
            {
                return holes;
            }
            else
            {
                throw new Exception("Could not retrieve all course holes");
            }
        }



        public IDictionary<string, int> GetCourseStats(IList<Guid> courseIds)
        {
            try
            {
                var a = from c in _context.Courses
                        join i in courseIds on c.Id equals i
                        select c.Name;
                return a.GroupBy(c => c).OrderByDescending(c => c.Count()).ToDictionary(c => c.Key, c => c.Count());
            }
            catch
            {
                throw new Exception("Could not retrieve course stats");
            }
        }

        public async Task<Boolean> AddCourse(Course course)
        {
            try
            {
                _context.Courses.Add(course);
                return (0 < await _context.SaveChangesAsync());
            }
            catch
            {
                throw new Exception("Could not add Course for " + course.Id.ToString());
            }
        }

        public async Task<Boolean> UpdateCourse(Course course)
        {
            try
            {
                _context.Courses.Update(course);
                return (0 < await _context.SaveChangesAsync());
            }
            catch
            {
                throw new Exception("Could not update Course for " + course.Id.ToString());
            }
        }

        public async Task<Boolean> DeleteCourse(Course course)
        {
            try
            {
                _context.Courses.Remove(course);
                return (0 < await _context.SaveChangesAsync());
            }
            catch
            {
                throw new Exception("Could not delete Course for " + course.Id.ToString());
            }
        }
    }
}
