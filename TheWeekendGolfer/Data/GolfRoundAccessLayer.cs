using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Web.Data;

namespace TheWeekendGolfer.Data
{
    public class GolfRoundAccessLayer
    {
        GolfDbContext _context;

        public GolfRoundAccessLayer(GolfDbContext context)
        {
            _context = context;
        }

    }
}
