using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMonitor.Objects
{
    internal class RecordObject
    {
        public string key { get; set; }
        public string value { get; set; }

        public RecordObject(string key, string value) { 
            this.key = key;
            this.value = value;
        }
    }
}
