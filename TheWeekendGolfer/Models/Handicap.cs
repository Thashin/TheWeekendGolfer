using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheWeekendGolfer.Models
{
    /// <summary>  
    ///  Model for the Handicap class.  
    /// </summary>  
    public class Handicap
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Decimal CurrentHandicap { get; set; }
        public Decimal Value { get; set; }
        public DateTime Date { get; set; }
    }
}
