using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace DNSUpdater.Base
{
    interface IApiCaller <TDomain, TRecord>
    {
        Task UpdateProvider();

        Task<TDomain> GetDomain();

        Task<IList<TRecord>> GetDomainRecords();

        Task UpdateDnsRecord(TDomain domain, IList<TRecord> record);

        Task<string> GetPublicIP();
    }
}
