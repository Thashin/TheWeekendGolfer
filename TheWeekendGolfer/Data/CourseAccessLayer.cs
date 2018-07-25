using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Web.Data;

namespace TheWeekendGolfer.Data
{
    public class CourseAccessLayer
    {
        GolfDbContext _context;

        public CourseAccessLayer(GolfDbContext context)
        {
            _context = context;
        }

    }
}
