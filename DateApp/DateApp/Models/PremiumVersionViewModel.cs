using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class PremiumVersionViewModel
    {

        public PremiumVersionViewModel(bool PremiumVersion=false,bool TestVersion=false,DateTime TimePremium=new DateTime(),DateTime TimeTest=new DateTime())
        {
            this.PremiumVersion = PremiumVersion;
            this.TestVersion = TestVersion;
            this.Code = Code;
            this.TimePremium = TimePremium;
            this.TimeTest = TimeTest;
            potentialPairView = new PotentialPairViewModel();
    }


       public PotentialPairViewModel potentialPairView { get; set; }
        public bool PremiumVersion { get; set; }
        public bool TestVersion { get; set; }
        public int Code { get; set; }
        public DateTime TimePremium { get; set; }
        public DateTime TimeTest { get; set; }
        public string Message { get; set; }


    }
}
