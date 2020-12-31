using RpaData.Interfaces;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RpaData.Models
{
    public class tblPayments : BaseData
    {
        [Required]
        [Display(Name = "Client")]
        public int tblPharmacistsId { get; set; }

        [Required]
        [Display(Name = "Amount Paid")]
        [DisplayFormat(DataFormatString = "{0:N2}" , ApplyFormatInEditMode =true)]
        public Decimal AmountPaid { get; set; }

        [Required]
        [Display(Name = "Payment Type")]
        public int PaymentTypeId { get; set; }

        [Display(Name = "Invoice")]
        public int InvoiceId { get; set; }

        [Required]
        [Display(Name = "Payment Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime PayDate { get; set; }
        
        [Display(Name = "Comment")]
        public string PaymentComment { get; set; }

        [DisplayName("Proof of Payment")]
        public string ProofOfPayment { get; set; }

        [DisplayName("Payment Status")]
        public bool PaymentStatus { get; set; }

        public tblPharmacists tblPharmacists { get; set; }
        public tblCodes PaymentType { get; set; }
        public tblInvoices Invoice { get; set; }
    }
}
