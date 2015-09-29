using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace themonitoringisnear.Models
{
    public class ActivationModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 6)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Pincode")]
        public string Pincode { get; set; }
    }
}