using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DNSUpdater.Base;

namespace DNSUpdater
{
    public class ApiCaller <TDomain, TRecord> : IApiCaller <TDomain, TRecord> 
        where TDomain : IDnsDomain where TRecord : IDnsRecord
    {
        public async Task UpdateProvider(DusHttpClient client, EventLog log)
        {
            TDomain domain = default;
            IList<TRecord> records = null;

            try
            {
                var domainResponse = await GetDomain(client);
                if (domainResponse.IsSuccessStatusCode)
                {
                    domain = await domainResponse.Content.ReadAsAsync<TDomain>();
                    log.WriteEntry($"Domain Retrieved: {domain.Domain}");

                    var recordResponse = await GetDomainRecords(client);
                    if (recordResponse.IsSuccessStatusCode)
                    {
                        records = await recordResponse.Content.ReadAsAsync<IList<TRecord>>();
                        log.WriteEntry($"Record(s) Retrieved: {records.ToString()}");

                        var ip = await DNSUpdaterService.GetPublicIP();

                        if (records.Select(r => r.data).Contains(ip))
                        {
                            log.WriteEntry("DNS up to date, no changes made", EventLogEntryType.Information);
                        }
                        else
                        {
                            var updateResponse = await UpdateDnsRecord(client, domain, records, ip);
                            if (updateResponse.IsSuccessStatusCode)
                            {
                                log.WriteEntry("DNS Updated Succsessfully", EventLogEntryType.Information);
                            }
                            else
                            {
                                var responseString = await updateResponse.Content.ReadAsStringAsync();
                                EventLog.WriteEntry(updateResponse.ReasonPhrase, responseString, EventLogEntryType.Error);
                            }
                        }
                    }
                    else
                    {
                        var responseString = await recordResponse.Content.ReadAsStringAsync();
                        EventLog.WriteEntry(recordResponse.ReasonPhrase, responseString, EventLogEntryType.Error);
                    }
                }
                else
                {
                    var responseString = await domainResponse.Content.ReadAsStringAsync();
                    EventLog.WriteEntry(domainResponse.ReasonPhrase, responseString, EventLogEntryType.Error);
                }
            }
            catch (HttpRequestException ex)
            {
                log.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
        }

        public async Task<HttpResponseMessage> GetDomain(DusHttpClient client)
        {
            var response = await client.GetAsync(client.DomainApiCall);
            return response;

        }

        public async Task<HttpResponseMessage> GetDomainRecords(DusHttpClient client)
        {
            var response = await client.GetAsync(client.DomainRecordApiCall);
            return response;
        }

        public async Task<HttpResponseMessage> UpdateDnsRecord(
            DusHttpClient client, 
            TDomain domain, 
            IList<TRecord> records, 
            string ip)
        {
            var newRecords = records.Where(r => r.name == "@")
                .Select(r => { r.data = ip; return r; });

            var response = await client.PutAsJsonAsync(client.DomainRecordApiCall, newRecords);
            return response;
        }
    }
}