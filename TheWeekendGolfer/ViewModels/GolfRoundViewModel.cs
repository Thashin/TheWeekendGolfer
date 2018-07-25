using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheWeekendGolfer.Web.Models.GolfRoundViewModels
{
    public class GolfRoundViewModel
    {
        [Required]
        public IEnumerable<string> Courses { get; set; }

        [Required]
        public IEnumerable<string> Players { get; set; }
    }
}
