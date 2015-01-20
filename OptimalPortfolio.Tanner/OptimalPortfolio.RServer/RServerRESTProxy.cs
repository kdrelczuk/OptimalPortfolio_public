using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OptimalPortfolio.RServer.Requests;
using OptimalPortfolio.RServer.Responses;

namespace OptimalPortfolio.RServer
{
    internal static class RServerRESTProxy
    {
        public static ComputePortfolioResponse ComputePortfolioFor2Assets(ComputePortfolioFor2AssetsRequest request)
        {
            Thread.Sleep(1500);
            return new ComputePortfolioResponse() { TickerID1 = request.TickerID1, TickerID2 = request.TickerID2 };
        }
    }
}
