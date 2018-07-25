using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheWeekendGolfer.Web.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string Holes { get; set; }
        public string TeeName { get; set; }
        public int Par { get; set; }
        public Decimal ScratchRating { get; set; }
        public Decimal Slope { get; set; }
    }
}
