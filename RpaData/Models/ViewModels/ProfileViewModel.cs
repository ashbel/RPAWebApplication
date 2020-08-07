using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RpaData.Models.ViewModels
{
    public class ProfileViewModel : BaseData
    {
        [Required]
        [Display(Name = "Client")]
        public string ClientId { get; set; }


        [Required]
        [Display(Name = "Pharmacy")]
        public int tblPharmacyid { get; set; }

        [Required]
        [Display(Name = "Work Address")]
        public string workAddress { get; set; }

        [Required]
        [Display(Name = "Qualifications (Tick all that apply)")]
        public string qualifications { get; set; }

        [Required]
        [Display(Name = "Years in practice post pre-registration qualification ")]
        public string yearsInPractice { get; set; }

        [Required]
        [Display(Name = "When did you join RPA")]
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

        public virtual tblPharmacy tblPharmacy { get; set; }
        public ApplicationUser Client { get; set; }
    }
}
