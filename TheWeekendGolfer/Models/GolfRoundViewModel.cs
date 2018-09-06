using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Models
{
    /// <summary>  
    ///  Model for the GolfRoundView class.  
    /// </summary>  
    public class GolfRoundViewModel
    {
        public DateTime Date { get; set; }
        public Course Course { get; set; }
        public IList<PlayerViewModel> players { get; set; }
    }
}
