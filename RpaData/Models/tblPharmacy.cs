using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RpaData.Models
{
    public class tblPharmacy : IDataModel
    {
        public tblPharmacy()
        {
            tblPharmacist = new HashSet<tblPharmacists>();
        }

        public int id { get; set; }
        public DateTime created { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Pharmacy")]
        public string pharmacyName { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Address")]
        public string pharmacyAddress { get; set; }

  
        [Display(Name = "Phone No.")]
        public string pharmacyPhone { get; set; }

        [Display(Name = "Website")]
        public string pharmacyWebsite { get; set; }

     
        public virtual ICollection<tblPharmacists> tblPharmacist { get; set; }
    }
}
