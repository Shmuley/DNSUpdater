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
        static void Main(string[] args)
        {
            var goDaddyAPI = new GoDaddyAPI();

            //goDaddyAPI.GetDomains().GetAwaiter().GetResult();

            var domain = new GoDaddyDomain()
            {
                Domain = "hernanfam.com"
            };
            goDaddyAPI.GetDomainRecords(domain, DNSRecordType.A, "@").GetAwaiter().GetResult();

            //var ip = goDaddyAPI.GetPublicIP().GetAwaiter().GetResult();

            //Console.WriteLine(ip);

            goDaddyAPI.UpdatePrimaryARecord(domain).GetAwaiter().GetResult();

            goDaddyAPI.GetDomainRecords(domain, DNSRecordType.A, "@").GetAwaiter().GetResult();
        }
    }
}
