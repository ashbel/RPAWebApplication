using RpaData.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RpaData.Models
{
    public class tblEventsHistory : BaseData
    {
        [Required]
        [Display(Name = "Client")]
        public int tblPharmacistsId { get; set; }

        [Required]
        [ForeignKey("tblEvents")]
        [Display(Name = "Meeting")]
        public int EventId { get; set; }

        [DisplayName("Will Attend ?")]
        public bool Attending { get; set; }

        [Display(Name = "Attended")]
        public bool AttendedEvent { get; set; }
     
        [Display(Name = "Comment")]
        public string EventHistoryComment { get; set; }
        public virtual tblPharmacists tblPharmacists { get; set; }
        public  virtual tblEvents Event { get; set; }
    }
}
