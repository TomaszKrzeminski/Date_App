using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class VideoCallViewModel
    {

        public VideoCallViewModel(string ReceiverId,string CallerId)
        {
            this.ReceiverId = ReceiverId;
            this.CallerId = CallerId;

        }

        public string ReceiverId { get; set; }
        public string CallerId { get; set; }
    }
}
