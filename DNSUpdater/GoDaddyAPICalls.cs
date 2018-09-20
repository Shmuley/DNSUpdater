using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

namespace DNSUpdater
{
    public class GoDaddyAPICalls
    {
        public async Task<GoDaddyDomain> GetDomain(GoDaddyHttpClient client, string domainName)
        {
            var domainCall = $"domains/{domainName}";

            var response = await client.GetAsync(domainCall);
            if (response.IsSuccessStatusCode)
            {
                var domain = await response.Content.ReadAsAsync<GoDaddyDomain>();
                return domain;
            }
            else
            {
                response.EnsureSuccessStatusCode();
                return null;
            }
        }

        public async Task<List<GoDaddyDNSRecord>> GetDomainRecords(GoDaddyHttpClient client, GoDaddyDomain domain)
        {
            var domainRecrodTypeCall = $"domains/{domain.Domain}/records/A/@";

            var response = await client.GetAsync(domainRecrodTypeCall);
            if (response.IsSuccessStatusCode)
            {
                var record = await response.Content.ReadAsAsync<List<GoDaddyDNSRecord>>();
                return record;
            }
            else
            {
                response.EnsureSuccessStatusCode();
                return null;
            }
        }

        public async Task UpdateDNSRecord(GoDaddyHttpClient client, GoDaddyDomain domain, List<GoDaddyDNSRecord> record)
        {
            var ip = await GetPublicIP();
            record[0].data = ip;

            var call = $"domains/{domain.Domain}/records/A/@";

            var response = await client.PutAsJsonAsync(call, record);
            response.EnsureSuccessStatusCode();
        }

        public async Task<string> GetPublicIP()
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri("https://api.ipify.org");
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
