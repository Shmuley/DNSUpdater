using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DNSUpdater
{
    public class GoDaddyHttpClient : HttpClient
    {
        private AuthenticationHeaderValue SsoKey { get; set; }
        public Uri Uri {get; set;}  = new Uri("https://api.godaddy.com/v1/");
        public MediaTypeWithQualityHeaderValue HeaderValue { get; set; }

        public GoDaddyHttpClient(string accessKey, string secretKey, string url, string headerValue)
        {
            Uri = new Uri(url);
            HeaderValue = new MediaTypeWithQualityHeaderValue(headerValue);
            SsoKey = new AuthenticationHeaderValue("sso-key", $"{accessKey}:{secretKey}");

            BaseAddress = Uri;
            DefaultRequestHeaders.Clear();
            DefaultRequestHeaders.Accept.Add(HeaderValue);
            DefaultRequestHeaders.Authorization = SsoKey;
        }
    }
}