using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace themonitoringisnear.Models
{
    public class UpdateSectionModel
    {
        [Required]
        public Guid SectionId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Section Name*")]
        public string SectionName { get; set; }
    }
}