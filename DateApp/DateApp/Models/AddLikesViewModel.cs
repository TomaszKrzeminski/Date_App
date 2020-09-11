using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class AddLikesViewModel
    {
        public string Email { get; set; }
        public int NumberOfLikes { get; set; }
        public int LikesToAdd { get; set; }
        public string UserId { get; set; }
    }
}
