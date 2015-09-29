using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace themonitoringisnear.Models
{
    public class CreateCollegeModel
    {
        [Required]
        [StringLength(50, MinimumLength = 6)]
        [Display(Name = "College Name*")]
        public string CollegeName { get; set; }
    }
}