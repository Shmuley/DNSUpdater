using System.Diagnostics;

namespace DNSUpdater
{
    public class GoDaddyAPICaller  : ApiCaller <GoDaddyDomain, GoDaddyDnsRecord>
    {
        public void UpdateGoDaddyDns(EventLog log)
        {
            using (var client = new GoDaddyHttpClient())
            {
                UpdateProvider(client, log);
            }
        }

    }
}
