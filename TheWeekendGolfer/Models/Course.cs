using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheWeekendGolfer.Models
{
    /// <summary>  
    ///  Model for the Course class.  
    /// </summary>  
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Holes { get; set; }
        [Required]
        public string TeeName { get; set; }
        [Required]
        public int Par { get; set; }
        [Required]
        public Decimal ScratchRating { get; set; }
        [Required]
        public Decimal Slope { get; set; }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
