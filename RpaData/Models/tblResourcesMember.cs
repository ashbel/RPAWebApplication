using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RpaData.Models
{
    public class tblResourcesMember: BaseData
    {
        [DisplayName("Resource")]
        public int tblResourcesId { get; set; }

        [DisplayName("Membership")]
        public int tblMembershipId { get; set; }

        public tblResources tblResources { get; set; }
        public tblMembership tblMembership { get; set; }
    }
}
