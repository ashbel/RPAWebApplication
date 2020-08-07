using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RpaData.Models
{
    public class tblCommunicationLogs : BaseData
    {
        [DisplayName("Communication")]
        public int CommunicationId { get; set; }

        [DisplayName("Email")]
        public string Receipient { get; set; }

        [DisplayName("Client Name")]
        public string ClientId { get; set; }

        public ApplicationUser Client { get; set; }
        public tblCommunication Communication { get; set; }
    }
}
