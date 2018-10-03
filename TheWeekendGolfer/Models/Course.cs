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
        /// <summary>
        /// This is an automatically generated property 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// The state in which the course is located
        /// </summary>
        [Required]
        public string Location { get; set; }
        /// <summary>
        /// The name of the course
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// The hole configurations for this course (1-9, 10-18, 18)
        /// </summary>
        [Required]
        public string Holes { get; set; }
        /// <summary>
        /// The tee names for this course(Red Woman etc.)
        /// </summary>
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
