using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RpaData.Models
{
    public class tblEmail : BaseData
    {
        [Required]
        [Display(Name = "SMTP")]
        public string email_smtp { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string email_address { get; set; }

        [Required]
        [Display(Name = "Port")]
        public string email_port { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string email_username { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string email_password { get; set; }

        [Required]
        [Display(Name = "SSL")]
        public bool email_ssl { get; set; }
    }
}
