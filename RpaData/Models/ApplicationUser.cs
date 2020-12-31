using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RpaData.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [DisplayName("Name")]
        public string FirstName { get; set; }

        [PersonalData]
        [DisplayName("Surname")]
        public string LastName { get; set; }

        [PersonalData]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: yyyy-MM-dd}")]
        [DisplayName("Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [PersonalData]
        [DisplayName("Gender")]
        public string Gender { get; set; }

        [PersonalData]
        [DisplayName("Name")]
        public string FullName { get; set; }

        [PersonalData]
        [DisplayName("RPA No.")]
        public string RpaNumber { get; set; }
    }
}
