using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class AddEventViewModel
    {
        public AddEventViewModel()
        {
            Event = new Event();
        }
        public Event Event{ get;set; }
        public AppUser User { get; set; }

    }

    public class EventViewModel
    {
        public EventViewModel()
        {
            Event = new Event();           
        }
        public Event Event { get; set; } 
        public AppUser User { get; set; }

    }

}
