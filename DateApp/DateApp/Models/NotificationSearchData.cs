using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class NotificationSearchData
    {
        public string ReceiverEmail { get; set; }
        public string LastPersonEmail { get; set; }
        public string Time { get; set; }
        public int Count { get; set; }
        public string PersonPicturePath { get; set; }
        public string PagePicturePath { get; set; }

    }
}
