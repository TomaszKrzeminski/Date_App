using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models

{
    public class PotentialPair
    {
        public string PairId { get; set; }
        public string MainPhotoPath { get; set; }
        public string Email { get; set; }
    }



    public class PotentialPairViewModel
    {
        public List<PotentialPair> list { get; set; }
        public PotentialPairViewModel()
        {
            list = new List<PotentialPair>();
        }


    }
}
