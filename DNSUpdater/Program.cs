using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DNSUpdater
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var goDaddyAPI = new GoDaddyAPI();

            var domain = new GoDaddyDomain()
            {
                Domain = "hernanfam.com"
            };
            await goDaddyAPI.GetDomainRecords(domain, DNSRecordType.A, "@");

            var ip = await goDaddyAPI.GetPublicIP();

            var record = new List<GoDaddyDNSRecord>()
            {
                {
                    new GoDaddyDNSRecord
                    {
                        data = ip,
                        name = "@",
                        ttl = 3600,
                        type = "A"
                    }
                }
            };

            await goDaddyAPI.UpdateDNSRecord(domain, record);

            await goDaddyAPI.GetDomainRecords(domain, DNSRecordType.A, "@");
        }
    }
}
