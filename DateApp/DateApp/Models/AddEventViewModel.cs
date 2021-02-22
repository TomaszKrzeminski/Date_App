using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{

  public interface IExtension
    {
        string GetExtension();
    }

    public class PictureJPG : IExtension
    {
        public string GetExtension()
        {
            return ".jpg";
        }
    }

    public class MovieMP4 : IExtension
    {
        public string GetExtension()
        {
            return ".mp4";
        }
    }



    public class AddEventViewModel
    {
        public AddEventViewModel()
        {
            Event = new Event();
        }


        public bool CheckExtension(IFormFile file, IExtension extension)
        {
            if (file!=null&&Path.GetExtension(file.FileName) == extension.GetExtension())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Event Event { get; set; }

        [Required(ErrorMessage ="Plik jest wymagany")]
        public IFormFile PictureFile_1 { get; set; }
        [Required(ErrorMessage = "Plik jest wymagany")]
        public IFormFile PictureFile_2 { get; set; }
        [Required(ErrorMessage = "Plik jest wymagany")]
        public IFormFile PictureFile_3 { get; set; }
        [Required(ErrorMessage = "Plik jest wymagany")]
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
