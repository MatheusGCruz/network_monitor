using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMonitor.Objects
{
    internal class ConfigObject
    {
        public int refreshTime { get; set; }
        public List<string> consultUrls { get; set; }

        public ConfigObject() { 
            this.consultUrls = new List<string>();
            this.refreshTime = 0;
        }

    }
}
