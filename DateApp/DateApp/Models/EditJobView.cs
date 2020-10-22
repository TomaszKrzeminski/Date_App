using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage = "Podaj ilość dni")]
        [Range(1,31,ErrorMessage ="Wybierz wartość pomiędzy 1-31")]
        public int Days { get; set; }
        [Required(ErrorMessage = "Podaj ilość godzin")]
        [Range(0, 23, ErrorMessage = "Wybierz wartość pomiędzy 0-23")]
        public int Hours { get; set; }
        [Required(ErrorMessage = "Podaj ilość minut")]
        [Range(0, 59, ErrorMessage = "Wybierz wartość pomiędzy 0-59")]
        public int Minutes { get; set; }
        [Required(ErrorMessage = "Podaj ilość sekund")]
        [Range(0, 59, ErrorMessage = "Wybierz wartość pomiędzy 0-59")]
        public int Seconds { get; set; }

    } 

   
  

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
