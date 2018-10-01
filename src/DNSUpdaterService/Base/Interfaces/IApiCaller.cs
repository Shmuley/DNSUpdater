using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace DNSUpdater.Base
{
    interface IApiCaller <TDomain, TRecord>
    {
        void UpdateProvider(DusHttpClient client, EventLog log);

        Task<HttpResponseMessage> GetDomain(DusHttpClient client, string domainCall);

        Task<HttpResponseMessage> GetDomainRecords(DusHttpClient client, string recordCall);

        Task<HttpResponseMessage> UpdateDnsRecord(DusHttpClient client, TDomain domain, IList<TRecord> record, string ip, string updateCall);
    }
}
