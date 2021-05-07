using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{

    public class TimeToWait
    {


        public int Check(int value)
        {
            if(value<0)
            {
                return 0;
            }
            else
            {
                return value;
            }
        }


        public TimeToWait(int Days, int Hours, int Minutes)
        {
            this.Hours =Check( Hours);
            this.Minutes =Check( Minutes);
            this.Days =Check( Days);
        }

        public TimeToWait()
        {
            Hours = 0;
            Minutes = 0;
            Days = 0;
        }

        public int Days { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
    }



    public class NotificationViewModel
    {


        public NotificationViewModel()
        {
            NewPairs = 0;
            NewMessages = 0;
            NewLikes = new TimeToWait();
            NewSuperLikes = new TimeToWait();
            PotentialMatches = 0;

        }

        public int PotentialMatches { get; set; }
        public int NewPairs { get; set; }
        public int NewMessages { get; set; }
        public TimeToWait NewLikes { get; set; }
        public TimeToWait NewSuperLikes { get; set; }


    }
}
