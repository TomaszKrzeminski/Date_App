using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public interface IRepositoryQuartz
    {
        SchedulerViewModel GetQuartzReport();
        int CheckJobCount();

    }

    public class RepositoryQuartz : IRepositoryQuartz
    {

        QuartzContext context;

        public RepositoryQuartz(QuartzContext context)
        {
            this.context = context;
        }

        public int CheckJobCount()
        {
            int count = 0;
            try
            {
                count = context.QrtzJobDetails.Count();
                return count;
            }
            catch (Exception ex)
            {
                return count;
            }
        }

        public SchedulerViewModel GetQuartzReport()
        {
            SchedulerViewModel model = new SchedulerViewModel();

            try
            {

                List<QrtzTriggers> qtriggerslist = context.QrtzTriggers.ToList();

                foreach (var trigger in qtriggerslist)
                {






                    DateTime date = new DateTime(trigger.StartTime);
                    date = date.ToLocalTime();
                    DateTime enddate = new DateTime(trigger.EndTime ?? trigger.EndTime ?? 0);

                    if (enddate != new DateTime())
                    {
                        enddate = enddate.ToLocalTime();
                    }


                    DateTime nextTime = new DateTime(trigger.NextFireTime ?? trigger.NextFireTime ?? 0);

                    if (nextTime != new DateTime())
                    {
                        nextTime=nextTime.ToLocalTime();
                    }


                    SchedulerDetails details = new SchedulerDetails() { JobName = trigger.JobName, Group=trigger.JobGroup,TriggerGroup=trigger.TriggerGroup,TriggerName=trigger.TriggerName,Start = date, End = enddate, NextStart = nextTime, State = trigger.TriggerState };

                    model.schedulerList.Add(details);


                }



                return model;
            }
            catch (Exception ex)
            {
                return model;
            }
        }
    }


}
