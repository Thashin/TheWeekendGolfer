using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Web.Data;

namespace TheWeekendGolfer.Data
{
    public class PlayerAccessLayer
    {
        GolfDbContext _context;

        public PlayerAccessLayer(GolfDbContext context)
        {
            _context = context;
        }

    }
}
