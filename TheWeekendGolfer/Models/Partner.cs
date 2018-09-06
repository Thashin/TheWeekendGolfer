using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWeekendGolfer.Models
{
    /// <summary>  
    ///  Model for the Partner class.  
    /// </summary>  
    public class Partner
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid PartnerId { get; set; }
    }
}
