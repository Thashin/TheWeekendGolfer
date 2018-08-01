using System;
using System.Collections.Generic;
using System.Linq;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Models;

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
                return _context.Courses.Where(s => s.Id.Equals(id)).FirstOrDefault(); ;
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
        public IEnumerable<Course> GetAllCoursesOrderedSlope()
        {
            try
            {
                return _context.Courses.Select(s => s).OrderBy(s=>s.Slope);
            }
            catch
            {
                throw new Exception("Could not retrieve all Courses");
            }
        }

        public IEnumerable<string> GetCourseNames()
        {
            try
            {
                return _context.Courses.GroupBy(c=>c.Name).Select(c=>c.First().Name);
            }
            catch
            {
                throw new Exception("Could not retrieve all course names");
            }
        }

        public IEnumerable<Course> GetCourseHoles(string courseName,string courseTee)
        {
            try
            {
                return _context.Courses.Where(c => c.Name.Equals(courseName) &&
                                                   c.TeeName.Equals(courseTee))
                                       .Select(c => c) ;
            }
            catch
            {
                throw new Exception("Could not retrieve all course holes");
            }
        }

        public IEnumerable<string> GetCourseTees(string courseName)
        {
            try
            {
                return _context.Courses.Where(c => c.Name.Equals(courseName)).
                                        GroupBy(c => c.TeeName).
                                        Select(c=>c.First().TeeName);
            }
            catch
            {
                throw new Exception("Could not retrieve all course tees");
            }
        }

        public Boolean AddCourse(Course course)
        {
            try
            {
                _context.Courses.Add(course);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw new Exception("Could not add Course for " + course.Id.ToString());
            }
        }

        public Boolean UpdateCourse(Course course)
        {
            try
            {
                _context.Courses.Update(course);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw new Exception("Could not update Course for " + course.Id.ToString());
            }
        }

        public Boolean DeleteCourse(Course course)
        {
            try
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw new Exception("Could not delete Course for " + course.Id.ToString());
            }
        }
    }
}
