using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class Weather_Data
    {
        public string City { get; set; }
        public double Temp { get; set; }
        public double Temp_Min { get; set; }
        public double Temp_Max { get; set; }
        public string Description { get; set; }

    }
}
