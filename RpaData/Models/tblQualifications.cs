using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RpaData.Models
{
    public class tblQualifications : BaseData
    {
        [Display(Name = "Qualification")]
        public string qName { get; set; }

        [Display(Name = "Description")]
        public string qDescription { get; set; }
    }
}
