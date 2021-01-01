using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RpaData.Models
{
    public class tblResources : BaseData
    {
        public tblResources()
        {
            tblResourcesMembers = new HashSet<tblResourcesMember>();
        }

        [Display(Name = "Resource Name")]
        public string ResourceName { get; set; }

        [Display(Name = "Category")]
        public int tblResourceCategoryId { get; set; }

        [Display(Name = "Description")]
        public string ResourceDescription { get; set; }

        public string FileName { get; set; }
        [Display(Name = "Category")]
        public virtual tblResourcesCategory tblResourceCategory { get; set; }

        public ICollection<tblResourcesMember> tblResourcesMembers { get; set; }
    }
}
