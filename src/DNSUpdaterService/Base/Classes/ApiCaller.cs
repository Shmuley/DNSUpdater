using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using DNSUpdater.Base;

namespace DNSUpdater
{
    public class ApiCaller <TDomain, TRecord> : IApiCaller <TDomain, TRecord> 
        where TDomain : IDnsDomain where TRecord : IDnsRecord
    {
        public async void UpdateProvider(DusHttpClient client, EventLog log)
        {
            TDomain domain = default;
            IList<TRecord> record = null;

            try
            {
                var domainResponse = await GetDomain(client, null);
                if (domainResponse.IsSuccessStatusCode)
                {
                    domain = await domainResponse.Content.ReadAsAsync<TDomain>();
                    log.WriteEntry($"Domain Retrieved: {domain.Domain}");

                    var recordResponse = await GetDomainRecords(client, null);
                    if (recordResponse.IsSuccessStatusCode)
                    {
                        record = await recordResponse.Content.ReadAsAsync<IList<TRecord>>();
                        log.WriteEntry($"Record(s) Retrieved: {record.ToString()}");

                        var updateResponse = await UpdateDnsRecord(client, domain, record, await DNSUpdaterService.GetPublicIP(), null);
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
            catch (Exception ex)
            {
                log.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
        }

        public async Task<HttpResponseMessage> GetDomain(DusHttpClient client, string domainCall)
        {
            var response = await client.GetAsync(domainCall ?? client.DomainApiCall);
            return response;

        }

        public async Task<HttpResponseMessage> GetDomainRecords(DusHttpClient client, string recordCall)
        {
            var response = await client.GetAsync(recordCall ?? client.DomainRecordApiCall);
            return response;
        }

        public async Task<HttpResponseMessage> UpdateDnsRecord(
            DusHttpClient client, 
            TDomain domain, 
            IList<TRecord> record, 
            string ip, 
            string updateCall)
        {
            record[0].data = ip;

            var response = await client.PutAsJsonAsync(updateCall ?? client.DomainUpdateDnsApiCall, record);
            return response;
        }
    }
}