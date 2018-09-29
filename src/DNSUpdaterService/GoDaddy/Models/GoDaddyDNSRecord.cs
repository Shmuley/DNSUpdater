using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNSUpdater
{
    public class GoDaddyDNSRecord
    {

        public string data { get; set; }
        public string name { get; set; }
        public int ttl { get; set; }
        public string type { get; set; }

        //"data": "98.114.169.221",
        //"name": "@",
        //"ttl": 3600,
        //"type": "A"
    }
}
