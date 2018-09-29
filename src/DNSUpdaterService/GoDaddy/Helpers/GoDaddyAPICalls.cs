using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using DNSUpdaterService.Base;

namespace DNSUpdater
{
    public class GoDaddyAPICalls : IApiCaller
    {
        public async Task<HttpResponseMessage> GetDomain(GoDaddyHttpClient client, string domainName)
        {
            var domainCall = $"domains/{domainName}";

            var response = await client.GetAsync(domainCall);
            return response;
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

        public async Task<List<GoDaddyDNSRecord>> UpdateDNSRecord(
            GoDaddyHttpClient client, 
            GoDaddyDomain domain, 
            List<GoDaddyDNSRecord> record,
            string ip)
        {
            record[0].data = ip;

            var call = $"domains/{domain.Domain}/records/A/@";

            var response = await client.PutAsJsonAsync(call, record);
            if (response.IsSuccessStatusCode)
            {
                var updatedRecord = await response.Content.ReadAsAsync<List<GoDaddyDNSRecord>>();
                return updatedRecord;
            }
            else
            {
                response.EnsureSuccessStatusCode();
                return null;
            }
        }
    }
}
