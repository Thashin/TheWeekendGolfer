using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Data
{
    public interface ICourseAccessLayer
    {

        Task<Course> GetCourse(Guid id);


        Task<IEnumerable<Course>> GetAllCourses();


        Task<IEnumerable<string>> GetCourseNames();


        Task<IEnumerable<string>> GetCourseTees(string courseName);


        Task<IEnumerable<Course>> GetCourseHoles(string courseName, string courseTee);



        Task<IDictionary<string, int>> GetCourseStats(IList<Guid> courseIds);

        Task<Boolean> AddCourse(Course course);


        Task<Boolean> UpdateCourse(Course course);


        Task<Boolean> DeleteCourse(Course course);
    }
}
