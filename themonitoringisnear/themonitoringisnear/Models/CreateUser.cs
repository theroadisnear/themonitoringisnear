using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace themonitoringisnear.Models
{
    public class CreateUser
    {
        [Required]
        public Guid StudentId { get; set; }
        
        [Required]
        [EmailAddress]
        [Display(Name = ("Email Address*"))]
        public string Email { get; set; }

        [Required]
        [Display(Name = ("Role*"))]
        public string Role { get; set; }
    }
}