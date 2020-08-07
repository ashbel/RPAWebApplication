using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RpaData.Models
{
    public class UserModel
    {
            [Required]

            [Display(Name = "Name")]
            public string firstName { get; set; }

            [Required]

            [Display(Name = "Surname")]
            public string lastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Phone]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Date of Birth")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: YYYY-MM-DD}")]
            public DateTime dateOfBirth { get; set; }

            [Required]
            [Display(Name = "Gender")]
            public string gender { get; set; }

            [Required]
            [Display(Name = "Pharmacists Council of Zimbabwe Practice Number")]
            public string RpaNumber { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

        [Display(Name = "Roles")]
        public string Roles { get; set; }
        
    }
}
