using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public abstract class FactoryMethodIJobDetail
    {
        public string JobName { get; set; }
        public string JobGroup { get; set; }
        public abstract IJobDetail GetJobDetail();
    }



    class NotificationJobFactory : FactoryMethodIJobDetail
    {


        public NotificationJobFactory(string JobName,string JobGroup)
        {
            this.JobGroup = JobGroup;
            this.JobName = JobName;
        }


        public override IJobDetail GetJobDetail()
        {
           return JobBuilder.Create<NotificationJob>().WithIdentity(JobName, JobGroup).StoreDurably().RequestRecovery().Build();
        }
    }

    class TestJob1MinuteFactory : FactoryMethodIJobDetail
    {

        public TestJob1MinuteFactory(string JobName, string JobGroup)
        {
            this.JobGroup = JobGroup;
            this.JobName = JobName;
        }



        public override IJobDetail GetJobDetail()
        {
            return JobBuilder.Create<TestJob1Minute>().WithIdentity(JobName, JobGroup).StoreDurably().RequestRecovery().Build();
        }
    }


    class TestJob2MinutesFactory : FactoryMethodIJobDetail
    {


        public TestJob2MinutesFactory(string JobName, string JobGroup)
        {
            this.JobGroup = JobGroup;
            this.JobName = JobName;
        }

        public override IJobDetail GetJobDetail()
        {
            return JobBuilder.Create<TestJob2Minutes>().WithIdentity(JobName, JobGroup).StoreDurably().RequestRecovery().Build();
        }
    }


   


}
