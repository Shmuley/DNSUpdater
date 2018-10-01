using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DNSUpdater.Base
{
    public class DusHttpClient : HttpClient
    {
        public AuthenticationHeaderValue SsoKey { get; set; }
        public Uri Uri { get; set; }
        public MediaTypeWithQualityHeaderValue HeaderValue { get; set; }
        public string DomainApiCall { get; set; }
        public string DomainRecordApiCall { get; set; }
        public string DomainUpdateDnsApiCall { get; set; }

    }
}