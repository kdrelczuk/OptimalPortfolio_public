using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OptimalPortfolio.RServer.Requests;
using OptimalPortfolio.RServer.Responses;

namespace OptimalPortfolio.RServer
{
    public class RServerEcosystemGateway
    {
        static object serversAccess = new object();

        private static Dictionary<string, bool> servers = new Dictionary<string, bool>();
        private Func<bool> ServerAreAvailable = () => { return servers.Values.Where(v => v).Any(); };
        private Func<string> ServerAvailable = () => { return servers.Where(i => i.Value).Select(i => i.Key).First(); };

        private ConcurrentQueue<Tuple<IRServerRequestMessage, Action<IRServerResponseMessage>>> queue =
            new ConcurrentQueue<Tuple<IRServerRequestMessage, Action<IRServerResponseMessage>>>();

        public void Enqueue(IRServerRequestMessage request, Action<IRServerResponseMessage> responseCallback)
        {
            // add new request to awaiting queue
            var callback = responseCallback as Action<IRServerResponseMessage>;
            queue.Enqueue(Tuple.Create<IRServerRequestMessage, Action<IRServerResponseMessage>>(request, responseCallback));

            // do the job
            Thread newThread = new Thread(new ThreadStart(DoTheJob));
            newThread.Start();

        }

        public RServerEcosystemGateway()
        {
            servers.Add("111.111", true);
            servers.Add("222.222", true);
            servers.Add("333.333", true);
            servers.Add("444.111", true);
            servers.Add("555.222", true);
            servers.Add("666.333", true);
            servers.Add("777.111", true);
            servers.Add("888.222", true);
            servers.Add("999.333", true);
            servers.Add("aaa.111", true);
            servers.Add("bbb.222", true);
            servers.Add("ccc.333", true);
        }

        private string AcuireServer()
        {
            lock (serversAccess)
            {
                while (!ServerAreAvailable())
                {
                    Thread.Sleep(100);
                }
                var server = ServerAvailable();
                servers[server] = false;
                return server;
            }
        }

        private void ReleaseServer(string server)
        {
            servers[server] = true;
        }

        private void DoTheJob()
        {
            lock (jobIsRunning)
            {
                string server = AcuireServer();

                Tuple<IRServerRequestMessage, Action<IRServerResponseMessage>> item;
                if (queue.TryDequeue(out item))
                {
                    if (item.Item1.GetType() == typeof(ComputePortfolioFor2AssetsRequest))
                    {
                        var concreteItem1 = item.Item1 as ComputePortfolioFor2AssetsRequest;
                        concreteItem1.Server = server;
                        var response = RServerRESTProxy.ComputePortfolioFor2Assets(concreteItem1) as ComputePortfolioResponse;
                        response.Server = server;
                        item.Item2.Invoke(response);
                        ReleaseServer(server);
                    }
                }
            }

        }
    }
}
