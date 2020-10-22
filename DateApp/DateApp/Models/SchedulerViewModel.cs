using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class SchedulerDetails
    {
        public string JobName { get; set; }
        public string Group { get; set; }
        public string TriggerName { get; set; }
        public string TriggerGroup { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string State { get; set; }
        public DateTime NextStart { get; set; }

        public SchedulerDetails()
        {
            JobName = "";
            Start = new DateTime();
            End = new DateTime();
            NextStart = new DateTime();
        }

        public SchedulerDetails(string Test)
        {

            JobName = "JobName"+Test;
            Group = "Group"+Test;
            TriggerName = "TriggerName"+Test;
            TriggerGroup = "TriggerGroup"+Test;
            Start = new DateTime();
            End = new DateTime();
            NextStart = new DateTime();



        }



    }



    public class SchedulerViewModel
    {
       public  IList<SchedulerDetails> schedulerList { get; set; }

        public SchedulerViewModel()
        {
            this.schedulerList = new List<SchedulerDetails>();
        }



    }
}
