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
        private readonly DusHttpClient client;
        private readonly EventLog log;

        public ApiCaller(DusHttpClient client, EventLog log)
        {
            this.client = client;
            this.log = log;
        }

        public async Task UpdateProvider()
        {
            try
            {
                await UpdateDnsRecord(await GetDomain(), await GetDomainRecords());
            }
            catch (HttpRequestException ex)
            {
                log.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
        }

        public async Task<TDomain> GetDomain()
        {
            var response = await client.GetAsync(client.DomainApiCall);
            if (response.IsSuccessStatusCode)
            {
                var domain = await response.Content.ReadAsAsync<TDomain>();
                log.WriteEntry($"Domain Retrieved: {domain.Domain}", EventLogEntryType.Information);
                return domain;
            }
            else
            {
                var responseString = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(responseString);
            }
        }

        public async Task<IList<TRecord>> GetDomainRecords()
        {
            var response = await client.GetAsync(client.DomainRecordApiCall);
            if (response.IsSuccessStatusCode)
            {
                var records = await response.Content.ReadAsAsync<IList<TRecord>>();
                log.WriteEntry($"Record(s) Retrieved: {records.ToString()}");
                return records;
            }
            else
            {
                var responseString = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(responseString);
            }
        }

        public async Task UpdateDnsRecord(TDomain domain, IList<TRecord> records)
        {
            var ip = await GetPublicIP();

            var newRecords = records.Where(r => r.name == "@")
                .Select(r => { r.data = ip; return r; });

            var response = await client.PutAsJsonAsync(client.DomainRecordApiCall, newRecords);
            var responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                log.WriteEntry($"DNS Updated Successfully: {responseString}", EventLogEntryType.Information);
            }
            else
            {
                throw new HttpRequestException(responseString);
            }
        }

        public async Task<string> GetPublicIP()
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri("https://api.ipify.org");
                var response = await client.GetAsync(uri);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return responseString;
                }
                else
                {
                    throw new HttpRequestException(responseString);
                }
            }
        }

    }
}