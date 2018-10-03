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
        /// <summary>
        /// This is an automatically generated property 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        /// <summary>
        /// The id for the player who owns this handicap
        /// </summary>
        public Guid PlayerId { get; set; }
        /// <summary>
        /// The calculated handicap after this round was played
        /// </summary>
        public Decimal CurrentHandicap { get; set; }
        /// <summary>
        /// The "played to" score for the current round
        /// </summary>
        public Decimal Value { get; set; }
        /// <summary>
        /// The date that this round was played
        /// </summary>
        public DateTime Date { get; set; }
    }
}
