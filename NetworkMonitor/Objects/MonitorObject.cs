using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMonitor.Objects
{
    internal class MonitorObject
    {
        public string url {  get; set; }
        public string reponse { get; set; }
        public string timeElapsed { get; set; }

        public MonitorObject() { 
            this.url = string.Empty;
            this.reponse = string.Empty;
            this.timeElapsed = string.Empty;
        }
    }
}
