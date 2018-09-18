using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Net.Http.Formatting;

namespace DNSUpdater
{
    public class GoDaddyAPI
    {
        private const string URL = "https://api.godaddy.com/v1/";
        private readonly string accessKey = "AEdBCuyrDvW_6XVDcvdAc7MbSoQbUkqkwq";
        private readonly string secretKey = "6XVGwL6a88WW2V1G1fDiHd";
        private readonly MediaTypeWithQualityHeaderValue requestHeaders = new MediaTypeWithQualityHeaderValue("application/json");

        public async Task GetDomains()
        {
            using (var client = new HttpClient())
            {
                var authorization = new AuthenticationHeaderValue("sso-key", $"{accessKey}:{secretKey}");

                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = authorization;

                var domainCall = "domains";

                var response = await client.GetAsync(domainCall);
                if (response.IsSuccessStatusCode)
                {
                    var domains = await response.Content.ReadAsAsync<List<GoDaddyDomain>>();
                    foreach (var domain in domains)
                    {
                        Console.WriteLine(domain.Domain);
                    }
                }
            }
        }

        public async Task GetDomainRecords(GoDaddyDomain domain, DNSRecordType type, string name)
        {
            using (var client = new HttpClient() {
                BaseAddress = new Uri(URL),
            })
            {
                var authorization = new AuthenticationHeaderValue("sso-key", $"{accessKey}:{secretKey}");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(requestHeaders);
                client.DefaultRequestHeaders.Authorization = authorization;
                

                var domainRecordCall = $"domains/{domain.Domain}/records";
                var domainRecrodTypeCall = $"domains/{domain.Domain}/records/{type}/{name}";

                //var call = type.HasValue ? domainRecrodTypeCall : domainRecordCall;

                var response = await client.GetAsync(domainRecrodTypeCall);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Domain: {domain.Domain.ToUpper()}");

                    var records = await response.Content.ReadAsAsync<List<GoDaddyDNSRecord>>();
                    foreach (var record in records)
                    {
                        Console.WriteLine($"Record Type: {record.type} Record Data: {record.data} Record Name: {record.name}");
                    }

                }
            }
        }

        public async Task UpdatePrimaryARecord(GoDaddyDomain domain)
        {
            using (var client = new HttpClient())
            {
                var authorization = new AuthenticationHeaderValue("sso-key", $"{accessKey}:{secretKey}");

                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(requestHeaders);
                client.DefaultRequestHeaders.Authorization = authorization;

                var record = new List<GoDaddyDNSRecord>()
                {
                    { new GoDaddyDNSRecord {
                        name = "@",
                        type = "A",
                        data = "98.114.169.221",
                        //data = await GetPublicIP(),
                        ttl = 3600
                        }
                    }
                };

                var call = $"domains/{domain.Domain}/records/A/@";

                var response = await client.PutAsJsonAsync(call, record);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task<string> GetPublicIP()
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri("https://api.ipify.org");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(requestHeaders);

                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var ip = await response.Content.ReadAsStringAsync();
                    return ip;
                }
                else
                {
                    return null;
                }

            }
        }
    }
}
