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
    public class Forecast
    {
        public Player Player { get; set; }
        public Decimal Average { get; set; }
        public Decimal PB { get; set; }
        public Decimal HS { get; set; }
        public Decimal ToLowerHandicap { get; set; }
        public Decimal Sixty { get; set; }
        public Decimal FiftyFive { get; set; }
        public Decimal Fifty { get; set; }
        public Decimal FortyFive { get; set; }
        public Decimal Forty { get; set; }
        public Decimal ThirtyFive { get; set; }
    }
}
