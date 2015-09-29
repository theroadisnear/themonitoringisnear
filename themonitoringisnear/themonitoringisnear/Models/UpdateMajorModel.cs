using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace themonitoringisnear.Models
{
    public class UpdateMajorModel
    {
        [Required]
        public Guid MajorId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        [Display(Name = "Major Name*")]
        public string MajorName { get; set; }
    }
}