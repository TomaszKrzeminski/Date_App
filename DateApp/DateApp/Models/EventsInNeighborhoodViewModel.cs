using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class EventsInNeighborhoodViewModel
    {

        public EventsInNeighborhoodViewModel()
        {
            listEventsInWeek = new List<Event>();
            listEventsInDays = new List<Event>();
        }

        public List<Event> listEventsInWeek { get; set; }
        public List<Event> listEventsInDays { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public DateTime Date { get; set; }
        public int Days { get; set; }


    }
}
