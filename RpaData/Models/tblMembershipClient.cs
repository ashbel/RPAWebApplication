using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RpaData.Models
{
    public class tblMembershipClient : BaseData
    {
        [DisplayName("Membership Type")]
        public int tblMembershipId { get; set; }

        [DisplayName("Pharmacists")]
        public int tblPharmacistsId { get; set; }

        public tblMembership tblMembership { get; set; }
        public tblPharmacists tblPharmacists { get; set; }
    }
}
