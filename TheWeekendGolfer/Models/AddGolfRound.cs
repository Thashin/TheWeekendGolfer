using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheWeekendGolfer.Models
{
    /// <summary>
    /// This is the model to add a golf round.
    /// </summary>
    public class AddGolfRound
    {
        /// <summary>
        /// Date when the round was played
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// The id of the course played
        /// </summary>
        public Guid CourseId { get; set; }
        /// <summary>
        /// List of scores for the round
        /// </summary>
        public List<Score> Scores { get; set; }
    }
}
