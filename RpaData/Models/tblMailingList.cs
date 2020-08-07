using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RpaData.Models
{
    public class tblMailingList :BaseData
    {
        public tblMailingList()
        {
            tblMailingListClients = new HashSet<tblMailingListClients>();
        }

        [Display(Name = "List Name")]
        public string ListName { get; set; }

        [Display(Name = "Description")]
        public string ListDescription { get; set; }

        public virtual ICollection<tblMailingListClients> tblMailingListClients { get; set; }
    }
}
