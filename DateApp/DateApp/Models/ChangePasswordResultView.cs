using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class ChangePasswordResultView
    {
        public string Message { get; set; }
        public string RedirectAction { get; set; }
        public string RedirectController { get; set; }
        public string RedirectValue { get; set; }
    }
}





