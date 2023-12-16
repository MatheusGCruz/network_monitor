using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMonitor.Objects
{
    internal class CsvObject
    {
        public string executedAt { get; set; }
        public string totalTime { get; set; }

        public List<MonitorObject> monitors { get; set; }

        public CsvObject() {
            this.monitors = new List<MonitorObject>();
            this.executedAt = string.Empty; 
            this.totalTime = string.Empty;
        }
    }
}
