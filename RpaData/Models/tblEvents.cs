using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RpaData.Models
{
    public class tblEvents : BaseData
    {
        public tblEvents()
        {
            tblEventsHistory = new HashSet<tblEventsHistory>();
            tblCertificates = new HashSet<tblCertificates>();
        }


        [Required]
        [Display(Name = "Name")]
        public string EventName { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public DateTime EventStartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public DateTime EventEndDate { get; set; }

        [Required]
        [Display(Name = "Venue")]
        public string EventVenue { get; set; }

        [Required]
        [Display(Name = "Points")]
        public int EventPoints { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string EventDescription { get; set; }

        [Display(Name = "Sponsor")]
        public string EventSponsor { get; set; }

        public bool EventComplete { get; set; }

        public virtual ICollection<tblEventsHistory> tblEventsHistory { get; set; }
        public virtual ICollection<tblCertificates> tblCertificates { get; set; }

    }
}
