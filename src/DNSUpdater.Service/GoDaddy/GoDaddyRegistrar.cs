using DNSUpdater;
using DNSUpdater.Base;
using DNSUpdaterService.Base.Interfaces;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DNSUpdaterService.GoDaddy
{
    public class GoDaddyRegistrar : IRegistrar
    {
        public async Task UpdateRegistrar(EventLog log)
        {
            using(var client = new GoDaddyHttpClient())
            {
                var apiCaller = new ApiCaller<GoDaddyDomain, GoDaddyDnsRecord>(client, log);
                await apiCaller.UpdateProvider();
            }
        }
    }
}