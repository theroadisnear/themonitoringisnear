using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace themonitoringisnear.Models
{
    public class CreateSectionModel
    {
        [Required]
        public Guid MajorId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Section*")]
        public string SectionName { get; set; }
    }
}