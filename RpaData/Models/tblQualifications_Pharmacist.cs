using RpaData.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpaData.Models
{
    public class tblQualifications_Pharmacist : BaseData
    {
        public int PharmacistId { get; set; }
        public int QualificationId { get; set; }

        public tblPharmacists Pharmacist { get; set; }
        public tblQualifications Qualification { get; set; }
    }
}
