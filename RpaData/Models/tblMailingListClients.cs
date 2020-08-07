using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpaData.Models
{
    public class tblMailingListClients : BaseData
    {
        public int tblMailingListId { get; set; }
        public int tblPharmacistId { get; set; }
        public virtual tblMailingList tblMailingList { get; set; }
        public virtual tblPharmacists tblPharmacist { get; set; }
    }
}
