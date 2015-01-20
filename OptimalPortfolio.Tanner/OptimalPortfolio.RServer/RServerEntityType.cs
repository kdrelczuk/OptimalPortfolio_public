using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalPortfolio.RServer
{
    [Flags]
    public enum RServerEntityType
    {
        APIWorker = 1, PortfolioWorker = 2
    }
}
