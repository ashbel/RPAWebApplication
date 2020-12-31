using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RpaData.Models
{
    public class tblMembership : BaseData
    {
        public tblMembership()
        {
            tblMembershipClients = new HashSet<tblMembershipClient>();
            tblResourcesMembers = new HashSet<tblResourcesMember>();
        }

        [Required]
        [DisplayName("Name")]
        public string name { get; set; }

        [DisplayName("Description")]
        public string description { get; set; }

        public virtual ICollection<tblMembershipClient> tblMembershipClients { get; set; }
        public virtual ICollection<tblResourcesMember> tblResourcesMembers { get; set; }
    }
}
