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
        }


        [Required]
        [Display(Name = "Name")]
        public string EventName { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        public DateTime EventStartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EventEndDate { get; set; }

        [Required]
        [Display(Name = "Venue")]
        public string EventVenue { get; set; }

        [Required]
        [Display(Name = "Points to be Awarded")]
        public int EventPoints { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string EventDescription { get; set; }

        public virtual ICollection<tblEventsHistory> tblEventsHistory { get; set; }

    }
}
