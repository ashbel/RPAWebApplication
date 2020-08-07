using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RpaData.Models
{
    public class tblPharmacists : BaseData
    {
        public tblPharmacists()
        {
            tblMailingListClients = new HashSet<tblMailingListClients>();
        }
        [Required]
        [Display(Name = "Client")]
        public string ClientId { get; set; }


        
        [Display(Name = "Pharmacy")]
        public int tblPharmacyid { get; set; }

        
        [Display(Name = "Work Address")]
        public string workAddress { get; set; }

       
        [Display(Name = "Qualifications (Tick all that apply)")]
        public string qualifications { get; set; }

      
        [Display(Name = "Years in practice post pre-registration qualification ")]
        public string yearsInPractice { get; set; }

        
        [Display(Name = "When did you join RPA")]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime dateOfJoiningRPA { get; set; }


        [Required]
        [Display(Name = "Are you in good standing with Pharmacists Council of Zimbabwe ")]
        public bool goodStandingPCZ { get; set; }


        [StringLength(500)]
        [Display(Name = "If no, kindly explain")]
        public string goodStandingReasonPCZ { get; set; }

        [Required]
        [Display(Name = "Are you in good standing with Pharmacists Council of Zimbabwe")]
        public bool goodStandingMCAZ { get; set; }


        [StringLength(500)]
        [Display(Name = "If no, kindly explain")]
        public string goodStandingReasonMCAZ { get; set; }

        public bool profileComplete { get; set; }

        public bool status { get; set; }

        [Display(Name = "Renewal Date")]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime dateOfRenewal { get; set; }

        public virtual tblPharmacy tblPharmacy { get; set; }
        public ApplicationUser Client { get; set; }

        public virtual ICollection<tblMailingListClients> tblMailingListClients { get; set; }
    }
}
