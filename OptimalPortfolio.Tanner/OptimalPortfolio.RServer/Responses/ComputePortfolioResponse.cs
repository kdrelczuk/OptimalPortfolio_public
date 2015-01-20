using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalPortfolio.RServer.Responses
{
    public class ComputePortfolioResponse : IRServerResponseMessage
    {
        public int TickerID1 { get; set; }
        public int TickerID2 { get; set; }

        public string Server
        {
            get;
            set;
        }
    }
}
