using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimalPortfolio.RServer;
using OptimalPortfolio.RServer.Requests;
using OptimalPortfolio.RServer.Responses;
using Quartz;
using OptimalPortfolio.Tanner.Jobs;
using Quartz.Impl;

namespace OptimalPortfolio.Tanner
{
    class Program
    {




        static void Main(string[] args)
        {
            // construct a scheduler factory  
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler, start the schedular before triggers or anything else  
            IScheduler sched = schedFact.GetScheduler();
            sched.Start();

            // create job  
            IJobDetail job = JobBuilder.Create<RServerWatcherJob>().Build();

            // create trigger  
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(1).RepeatForever())
                .Build();

            // Schedule the job using the job and trigger   
            sched.ScheduleJob(job, trigger);
            sched.ScheduleJob(JobBuilder.Create<RServerPortfolioJob>().Build(), TriggerBuilder.Create()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(3).RepeatForever())
                .Build());


            // requestServer


            //q.Enqueue(request, computePortfolioResponse);

            Console.ReadKey();
        }
    }
}
