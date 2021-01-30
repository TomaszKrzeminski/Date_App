using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class LoggingWarningViewModel
    {
        public LoggingWarningViewModel(string email,int minutes,int attempts)
        {
            Email = email;
            Minutes = minutes;
            Attempts = attempts;
        }


        public string Email { get; set; }
        public int Minutes { get; set; }
        public int Attempts { get; set; }
    }
}
