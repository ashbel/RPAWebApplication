using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RpaData.Models
{
    public class tblCodes : BaseData
    {
        [Display(Name = "Name")]
        public string CodeName { get; set; }

        [Display(Name = "Applies To")]
        public string CodeEntity { get; set; }
    }
}
