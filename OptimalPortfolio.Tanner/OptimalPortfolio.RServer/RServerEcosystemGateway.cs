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
        private static readonly RServerEcosystemGateway instance = new RServerEcosystemGateway();

        private RServerEcosystemGateway()
        {
            servers = new ConcurrentDictionary<string, bool>();
            servers.TryAdd("111.111", true);
            servers.TryAdd("222.222", true);
            servers.TryAdd("333.333", true);
            servers.TryAdd("444.111", true);
            servers.TryAdd("555.222", true);
            servers.TryAdd("666.333", true);
            servers.TryAdd("777.111", true);
            servers.TryAdd("888.222", true);
            servers.TryAdd("999.333", true);
            servers.TryAdd("aaa.111", true);
            servers.TryAdd("bbb.222", true);
            servers.TryAdd("ccc.333", true);
        }

        public static RServerEcosystemGateway Instance { get { return instance; } }

        static object serversAccess = new object();

        private static ConcurrentDictionary<string, bool> servers;
        private Func<bool> ServerAreAvailable = () => { return servers.Values.Where(v => v).Any(); };
        private Func<string> ServerAvailable = () => { return servers.Where(i => i.Value).Select(i => i.Key).First(); };

        public int QueueSize { get { return queue.Count; } }

        private ConcurrentQueue<Tuple<IRServerRequestMessage, Action<IRServerResponseMessage>>> queue =
            new ConcurrentQueue<Tuple<IRServerRequestMessage, Action<IRServerResponseMessage>>>();

        public void Enqueue(IRServerRequestMessage request, Action<IRServerResponseMessage> responseCallback)
        {
            // add new request to awaiting queue
            var callback = responseCallback as Action<IRServerResponseMessage>;
            queue.Enqueue(Tuple.Create<IRServerRequestMessage, Action<IRServerResponseMessage>>(request, responseCallback));

            // TODO: check number of queueas, log if hight and do not enqueue if too many

            // do the job
            Thread newThread = new Thread(new ThreadStart(DoTheJob));
            newThread.Start();

        }

        private string AcuireServer()
        {
            lock (serversAccess)
            {
                while (!ServerAreAvailable())
                {
                    Thread.Sleep(250);
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
