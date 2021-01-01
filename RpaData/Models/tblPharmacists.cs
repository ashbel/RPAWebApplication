using RpaData.Interfaces;
using RpaData.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Newtonsoft.Json;

namespace RpaData.Models
{
    public class tblPharmacists : BaseData
    {
        public tblPharmacists()
        {
            tblMailingListClients = new HashSet<tblMailingListClients>();
            tblMembershipClients = new HashSet<tblMembershipClient>();
            tblEventsHistory = new HashSet<tblEventsHistory>();
            tblCertificates = new HashSet<tblCertificates>();
        }
        [Required]
        [Display(Name = "Client")]
        public string ApplicationUserId { get; set; }
       
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
        [Display(Name = "Are you in good standing with Pharmacists Council of Zimbabwe")]
        public bool goodStandingPCZ { get; set; }


        [StringLength(500)]
        [Display(Name = "If no, kindly explain")]
        public string goodStandingReasonPCZ { get; set; }

        [Required]
        [Display(Name = "Are you in good standing with the Medicines Control Authority of Zimbabwe")]
        public bool goodStandingMCAZ { get; set; }


        [StringLength(500)]
        [Display(Name = "If no, kindly explain")]
        public string goodStandingReasonMCAZ { get; set; }

        [StringLength(500)]
        [Display(Name = "Other Qualifications")]
        public string otherQualification { get; set; }

        public bool profileComplete { get; set; }

        public bool status { get; set; }

        [Display(Name = "Renewal Date")]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime dateOfRenewal { get; set; }

        [DisplayName("Pharmacy")]
        public virtual tblPharmacy tblPharmacy { get; set; }

        [DisplayName("Name")]
        public ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<tblMailingListClients> tblMailingListClients { get; set; }
        [JsonIgnore]
        public virtual ICollection<tblMembershipClient> tblMembershipClients { get; set; }
        public virtual ICollection<tblInvoicesClient> tblInvoiceClients { get; set; }
        public virtual ICollection<tblEventsHistory> tblEventsHistory { get; set; }
        public virtual ICollection<tblCertificates> tblCertificates { get; set; }
      
    }
}
