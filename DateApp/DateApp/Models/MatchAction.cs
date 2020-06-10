using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class MatchAction
    {

        public MatchAction()
        {

        }

        public MatchAction(string Message,bool Like,bool SuperLike,bool Error)
        {
            this.Message = Message;
            this.LikeAvailable = Like;
            this.SuperLikeAvailable = SuperLike;
            this.Error = Error;
        }

        public string Message { get; set; }
        public bool LikeAvailable { get; set; }
        public bool SuperLikeAvailable { get; set; }
        public bool Error { get; set; }
    }
}
