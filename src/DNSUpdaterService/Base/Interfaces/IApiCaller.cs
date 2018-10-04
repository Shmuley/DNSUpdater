using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace DNSUpdater.Base
{
    interface IApiCaller <TDomain, TRecord>
    {
        Task UpdateProvider(DusHttpClient client, EventLog log);

        Task<HttpResponseMessage> GetDomain(DusHttpClient client);

        Task<HttpResponseMessage> GetDomainRecords(DusHttpClient client);

        Task<HttpResponseMessage> UpdateDnsRecord(DusHttpClient client, TDomain domain, IList<TRecord> record, string ip);
    }
}
