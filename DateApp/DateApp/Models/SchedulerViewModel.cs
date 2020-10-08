using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class SchedulerDetails
    {
        public string JobName { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime NextStart { get; set; }

        public SchedulerDetails()
        {
            JobName = "";
            Start = new DateTime();
            End = new DateTime();
            NextStart = new DateTime();
        }

    }



    public class SchedulerViewModel
    {
        IList<SchedulerDetails> schedulerList { get; set; }

        public SchedulerViewModel()
        {
            this.schedulerList = new List<SchedulerDetails>();
        }



    }
}
