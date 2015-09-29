using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace themonitoringisnear.Models
{
    public class DeleteMajorModel
    {
        [Required]
        public Guid MajorId { get; set; }
    }
}