using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace RpaData.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        [PersonalData]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: yyyy-MM-dd}")]
        public DateTime DateOfBirth { get; set; }

        [PersonalData]
        public string Gender { get; set; }

        [PersonalData]
        public string FullName { get; set; }

        [PersonalData]
        public string RpaNumber { get; set; }
    }
}
