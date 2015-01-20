using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalPortfolio.RServer
{
    public class RServerEntity
    {
        public string IP { get; set; }
        public RServerEntityType Type { get; set; }
        public bool IsBusy { get; set; }
    }
}
