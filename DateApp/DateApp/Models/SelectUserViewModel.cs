using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class SelectUserViewModel
    {
        public SelectUserViewModel()
        {
            user = new AppUser();
        }

        public AppUser user { get; set; }
        public string term { get; set; }
    }
}
