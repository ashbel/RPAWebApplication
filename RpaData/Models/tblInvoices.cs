using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RpaData.Models
{
    public class tblInvoices : BaseData
    {
        public tblInvoices()
        {
            tblPayments = new HashSet<tblPayments>();
        }


        [Required]
        [Display(Name = "Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Invoice Type")]
        public int InvoiceTypeId { get; set; }

        [Required]
        [Display(Name = "Due Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DueDate { get; set; }

        [Display(Name = "Description")]
        public string InvoiceComment { get; set; }

        [DisplayName("Attachment")]
        public string invoiceAttachment { get; set; }
        public tblCodes InvoiceType { get; set; }

        public virtual ICollection<tblPayments> tblPayments { get; set; }
    }
}
