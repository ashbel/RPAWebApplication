using System;
using System.Collections.Generic;
using System.Text;

namespace RpaData.Interfaces
{
    interface IDataModel
    {
        int id { get; set; }
        DateTime created { get; set; }
    }
}
