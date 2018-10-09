using DNSUpdater.Base;

namespace DNSUpdater
{
    public class GoDaddyDnsRecord : IDnsRecord
    {
        public string data { get; set; }
        public string name { get; set; }
        public int ttl { get; set; }
        public string type { get; set; }

        //"data": "98.114.169.221",
        //"name": "@",
        //"ttl": 3600,
        //"type": "A"

        public override string ToString()
        {
            var str =
                $"Type: {type} - " +
                $"Name: {name} - " +
                $"Data: {data} - " +
                $"TTL: {ttl}";

            return str;
        }
    }
}
