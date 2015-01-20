using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalPortfolio.RServer
{
    public interface IRServerRequestMessage
    {
        string Server { get; set; }
    }
}
