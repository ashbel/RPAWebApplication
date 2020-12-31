using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RpaData.Models
{
    public class tblInvoicesClient :BaseData
    {
        [Display(Name = "Invoice")]
        public int tblInvoicesId { get; set; }

        [Display(Name = "Client")]
        public int tblPharmacistsId{ get; set; }

        [Display(Name = "Paid")]
        public bool paid { get; set; }
        public tblInvoices tblInvoices { get; set; }
        public tblPharmacists tblPharmacists { get; set; }
    }
}
