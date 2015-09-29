using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace themonitoringisnear.Models
{
    public class AccountActivationModel
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 10)]
        [Display(Name = "Password*")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 10)]
        [Display(Name = "Confirm Password*")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 6)]
        [Display(Name = "Secret Question*")]
        public string SecretQuestion { get; set; }

        [Required]
        [StringLength(60)]
        [Display(Name = "Secret Answer*")]
        public string SecretAnswer { get; set; }
    }
}