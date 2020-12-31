using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RpaData.Models.ViewModels
{
    public class CommunicationViewModel : BaseData
    {
        [DisplayName("Message Date")]
        public DateTime MessageDate { get; set; }

        [DisplayName("Subject")]
        public string Subject { get; set; }

        [DisplayName("Message")]
        public string Message { get; set; }

        [DisplayName("Send")]
        public bool emailsent { get; set; }

        [DisplayName("Attachment")]
        public string communicationAttachment { get; set; }

        [DisplayName("Attachment Name")]
        public string fileName { get; set; }

        [Required]
        [Display(Name = "Mailing Lists (Tick all that apply)")]
        public string mailinglist { get; set; }

        public List<tblCommunicationLogs> tblCommunicationLogs { get; set; }

    }
}
