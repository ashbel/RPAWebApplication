using System;
using System.Collections.Generic;
using System.Text;

namespace RpaData.Models
{
    public class ChartModel
    {
        public string[] labels { get; set; }
        public List<Datasets> datasets { get; set; }
        public List<Datasets_> datasets_ { get; set; }

    }

    public class Datasets
    {
        public string label { get; set; }
        public string[] backgroundColor { get; set; }
        public string[] borderColor { get; set; }
        public string borderWidth { get; set; }
        public decimal?[] data { get; set; }
    }

    public class Datasets_
    {
        public string label { get; set; }
        public string[] backgroundColor { get; set; }
        public string[] borderColor { get; set; }
        public string borderWidth { get; set; }
        public int[] data { get; set; }
    }
}
