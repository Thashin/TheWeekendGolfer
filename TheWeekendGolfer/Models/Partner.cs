using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWeekendGolfer.Models
{
    public class Partner
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid PartnerId { get; set; }
    }
}
