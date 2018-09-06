﻿using System;
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
        public DateTime Date { get; set; }
        public Guid CourseId { get; set; }
        public List<Score> Scores { get; set; }
    }
}
