using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheWeekendGolfer.Models
{
    /// <summary>  
    ///  Model for the Score class  
    /// </summary>  
    public class Score
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid PlayerId { get; set; }
        [Required]
        public int Value { get; set; }
        [Required]
        public Guid GolfRoundId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
