using RpaData.Interfaces;
using System;
using System.ComponentModel;

namespace RpaData.Models
{
    public class tblCommunication : BaseData
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

    }
}
