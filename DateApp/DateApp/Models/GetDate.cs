using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class DateDetails
    {

        public DateDetails(List<LoginHistory> l)
        {
            list = l;
        }

        public List<LoginHistory> list { get; set; }
    }
}
