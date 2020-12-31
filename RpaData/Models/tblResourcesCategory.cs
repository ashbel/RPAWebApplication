using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RpaData.Models
{
    public class tblResourcesCategory : BaseData
    {
        public tblResourcesCategory()
        {
           // tblResources = new HashSet<tblResources>();
        }

        [Required]
        [DisplayName("Category Name")]
        public string catName { get; set; }

        [DisplayName("Descriptions")]
        public string catDescription { get; set; }

        //public ICollection<tblResources> tblResources { get; set; }
    }
}
