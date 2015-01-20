using OptimalPortfolio.RServer;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalPortfolio.Tanner.Jobs
{
    class RServerWatcherJob : IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine(RServerEcosystemGateway.Instance.QueueSize);
        }
    }
}
