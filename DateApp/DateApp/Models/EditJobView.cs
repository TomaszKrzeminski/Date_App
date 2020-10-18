using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{



    public class CroneDate
    {


        public CroneDate()
        {

        }


        public CroneDate(int Days, int Hours, int Minutes, int Seconds)
        {
            this.Days = Days;
            this.Hours = Hours;
            this.Minutes = Minutes;
            this.Seconds = Seconds;
        }
        public int Days { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }

    }

    

    //public enum Action
    //{
    //    Stop,Start
    //}


    //public enum Time
    //{
    //    Once,FewTimes,Forever
    //}

  

    public class EditJobView
    {

        public CroneDate Crone{ get; set; }
        public string JobName { get; set; }
        public string Group { get; set; }
        public string TriggerName { get; set; }
        public string TriggerGroup { get; set; }

        public EditJobView()
        {
           
        }

        public EditJobView(string JobName, string Group, string TriggerName, string TriggerGroup,CroneDate Crone)
        {
            
            this.JobName = JobName;
            this.Group = Group;
            this.TriggerName = TriggerName;
            this.TriggerGroup = TriggerGroup;
            this.Crone = Crone;

          
        }  
               
      

    }
}
