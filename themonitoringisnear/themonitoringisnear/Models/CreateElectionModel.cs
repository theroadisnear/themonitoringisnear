using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace themonitoringisnear.Models
{
    public class CreateElectionModel
    {
        [Required]
        [Display(Name = "Election Name*")]
        public string ElectionName { get; set; }

        [Display(Name = "Group*")]
        public Guid GroupId { get; set; }

        [Required]
        [Display(Name = "Start Date*")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Start Time*")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End Date*")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "End Time*")]
        public DateTime EndTime { get; set; }


    }
}