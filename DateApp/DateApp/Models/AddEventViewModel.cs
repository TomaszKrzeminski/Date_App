using Microsoft.AspNetCore.Http;
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
        public Event Event { get; set; }
        public IFormFile PictureFile_1 { get; set; }
        public IFormFile PictureFile_2 { get; set; }
        public IFormFile PictureFile_3 { get; set; }
        public IFormFile MovieFile { get; set; }
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
