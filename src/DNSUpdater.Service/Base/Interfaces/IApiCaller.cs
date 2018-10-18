using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNSUpdater.Base
{
    public interface IApiCaller <TDomain, TRecord> 
        where TDomain : IDnsDomain 
        where TRecord : IDnsRecord
    {
        Task UpdateProvider();

        Task<TDomain> GetDomain();

        Task<IList<TRecord>> GetDomainRecords();

        Task UpdateDnsRecord(TDomain domain, IList<TRecord> record);

        Task<string> GetPublicIP();
    }
}
