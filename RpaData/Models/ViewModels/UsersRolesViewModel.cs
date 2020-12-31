using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RpaData.Models.ViewModels
{
    public class UsersRolesViewModel
    {
            public string Id { get; set; }

            [Display(Name = "User Name")]
            public string UserName { get; set; }

            [Display(Name = "Email")]
            public string Email { get; set; }

            [Display(Name = "User Role")]
            public string Role { get; set; }

            [Display(Name = "Name")]
            public string FullName { get; set; }

            [Display(Name = "Status")]
            public bool Status { get; set; }

            [Display(Name = "Email Confirmed")]
            public bool confirmed { get; set; }
        
    }
}
