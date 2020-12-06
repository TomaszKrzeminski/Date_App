using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class EventsInRangeDates
    {

        public EventsInRangeDates()
        {
            From = new DateTime();
            To = new DateTime();
        }

        public DateTime From { get; set; }
        public DateTime To { get; set; }


    }
}
