using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface ICourseAccessLayer
    {

        Course GetCourse(Guid id);


        IEnumerable<Course> GetAllCourses();


        IEnumerable<string> GetCourseNames();


        IEnumerable<string> GetCourseTees(string courseName);


        IEnumerable<Course> GetCourseHoles(string courseName, string courseTee);



        IDictionary<string, int> GetCourseStats(IList<Guid> courseIds);

        Task<Boolean> AddCourse(Course course);


        Task<Boolean> UpdateCourse(Course course);


        Task<Boolean> DeleteCourse(Course course);
    }
}
