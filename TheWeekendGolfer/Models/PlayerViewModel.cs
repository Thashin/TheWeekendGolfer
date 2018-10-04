using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWeekendGolfer.Models
{    
    /// <summary>  
     ///  Model for the Player View class.  
     /// </summary>  
     /// <remarks>
     /// This is only used in the GolfRoundViewModel
     /// </remarks>
    public class PlayerViewModel
    {
        public Player Player { get; set; }
        public int Score { get; set; }
    }
}
