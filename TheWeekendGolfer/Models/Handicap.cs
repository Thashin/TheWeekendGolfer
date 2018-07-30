using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheWeekendGolfer.Models
{
    public class Handicap
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Decimal Value { get; set; }
        public DateTime Date { get; set; }
    }
}
