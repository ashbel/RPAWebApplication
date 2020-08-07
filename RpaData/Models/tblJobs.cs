using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RpaData.Models
{
    public class tblJobs : BaseData
    {
        [Display(Name = "Job")]
        public string JobName { get; set; }

        [Display(Name = "Status")]
        public bool JobStatus { get; set; }

        [Display(Name = "Frequency")]
        public string JobFrequency { get; set; }

    }
}
