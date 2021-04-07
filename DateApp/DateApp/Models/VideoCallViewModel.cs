using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class VideoCallViewModel
    {

        public VideoCallViewModel(string ReceiverId,string CallerId,string CallerEmail="",string CallerPicturePath="")
        {
            this.ReceiverId = ReceiverId;
            this.CallerId = CallerId;
            this.CallerEmail = CallerEmail;
            this.CallerPicturePath = CallerPicturePath;

        }

        public string ReceiverId { get; set; }
        public string CallerEmail { get; set; }
        public string CallerPicturePath { get; set; }
        public string CallerId { get; set; }
    }
}
