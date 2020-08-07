using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RpaData.Models
{
    public class tblCertificates : BaseData
    {
        [Required]
        [Display(Name = "Client")]
        public string ClientId { get; set; }

        [Required]
        [Display(Name = "Meeting")]
        public int EventId { get; set; }

        [Required]
        [Display(Name = "Points")]
        public int EventPoints { get; set; }

        [Required]
        [Display(Name = "Certificate Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime CertificateDate { get; set; }

        public ApplicationUser Client { get; set; }
        public tblEvents Event { get; set; }

        //1. Name of pharmacist
        //2. Topic of meeting attended
        //3. Number of points awarded
        //4. Date of meeting for which certificate is being awarded
        //5. Certificate number
    }
}
