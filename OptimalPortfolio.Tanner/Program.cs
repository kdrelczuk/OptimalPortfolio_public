using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimalPortfolio.RServer;
using OptimalPortfolio.RServer.Requests;
using OptimalPortfolio.RServer.Responses;

namespace OptimalPortfolio.Tanner
{
    class Program
    {
        static object screen = new object();

        static Action<IRServerResponseMessage> computePortfolioResponse = r =>
             {
                 lock (screen)
                 {
                     var rr = r as ComputePortfolioResponse;
                     Console.WriteLine("({0},{1}) at {2}",rr.TickerID1, rr.TickerID2, rr.Server);
                 }
             };

        static void Main(string[] args)
        {
            // requestServer
            var q = new RServerEcosystemGateway();

            //var request = new ComputePortfolioFor2AssetsRequest() { TickerID1 = 23, TickerID2 = 57 };
            Enumerable.Range(0, 130).ToList().ForEach(t =>
                q.Enqueue(new ComputePortfolioFor2AssetsRequest() { TickerID1 = t, TickerID2 = t }, computePortfolioResponse));

            //q.Enqueue(request, computePortfolioResponse);
            Console.ReadKey();
        }
    }
}
