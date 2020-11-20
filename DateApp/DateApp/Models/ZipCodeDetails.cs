using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{

    public class ZipDistanceDetails
    {

        public string code { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public double distance { get; set; }

    }





    public class ZipCodeDetails
    {


        public ZipCodeDetails()
        {
            postal_code = "";
            country_code = "";
            latitude = "";
            longitude = "";
            city = "";
            state = "";
            state_code = "";
            province = "";
            province_code = "";
        }


        public string postal_code { get; set; }
        public string country_code { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string state_code { get; set; }
        public string province { get; set; }
        public string province_code { get; set; }
    }
}
