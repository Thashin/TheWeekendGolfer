using System;
using System.Collections.Generic;

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
        public IDictionary<int, Decimal> RangeOfPredictedHandicaps {get;set ;}
}
}
