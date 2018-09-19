using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Configuration;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DNSUpdater
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GoDaddyDomain domain = null;
            List<GoDaddyDNSRecord> record = null;

            string accessKey = ConfigurationManager.AppSettings["AccessKey"];
            string secretKey = ConfigurationManager.AppSettings["SecretKey"];
            string URL = ConfigurationManager.AppSettings["URL"];
            string requestHeaders = ConfigurationManager.AppSettings["RequestHeaders"];

            var goDaddyAPI = new GoDaddyAPICalls();

            using (var client = new GoDaddyHttpClient(accessKey, secretKey, URL, requestHeaders))
            {
                try
                {
                    domain = await goDaddyAPI.GetDomain(client, "hernanfam.com");
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                try
                {
                    record = await goDaddyAPI.GetDomainRecords(client, domain);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                //try
                //{
                //    await goDaddyAPI.UpdateDNSRecord(client, domain, record);
                //}
                //catch (HttpRequestException ex)
                //{
                //    Console.WriteLine(ex.Message);
                //}
            }

            //await goDaddyAPI.GetDomainRecords(domain, DNSRecordType.A, "@");

            //var ip = await goDaddyAPI.GetPublicIP();

            //var record = new List<GoDaddyDNSRecord>()
            //{
            //    {
            //        new GoDaddyDNSRecord
            //        {
            //            data = ip,
            //            name = "@",
            //            ttl = 3600,
            //            type = "A"
            //        }
            //    }
            //};

            //await goDaddyAPI.UpdateDNSRecord(domain, record);

            //await goDaddyAPI.GetDomainRecords(domain, DNSRecordType.A, "@");
        }
    }
}
