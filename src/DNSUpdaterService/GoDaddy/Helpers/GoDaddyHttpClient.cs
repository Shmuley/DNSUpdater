using DNSUpdaterService.Properties;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DNSUpdater
{
    public class GoDaddyHttpClient : HttpClient
    {
        public AuthenticationHeaderValue SsoKey { get; set; }
        public Uri Uri {get; set;}
        public MediaTypeWithQualityHeaderValue HeaderValue { get; set; }

        public GoDaddyHttpClient()
        {
            Uri = new Uri(ConfigurationManager.AppSettings["URL"]);
            HeaderValue = new MediaTypeWithQualityHeaderValue(ConfigurationManager.AppSettings["RequestHeaders"]);
            SsoKey = new AuthenticationHeaderValue(
                "sso-key", $"{GoDaddyAPI.Default.AccessKey}:{GoDaddyAPI.Default.SecretKey}");

            BaseAddress = Uri;
            DefaultRequestHeaders.Clear();
            DefaultRequestHeaders.Accept.Add(HeaderValue);
            DefaultRequestHeaders.Authorization = SsoKey;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            GoDaddyHttpClient compare = obj as GoDaddyHttpClient;
            if (compare != null &&
                Uri.OriginalString == compare.Uri.OriginalString &&
                HeaderValue.MediaType == compare.HeaderValue.MediaType &&
                SsoKey.Parameter == compare.SsoKey.Parameter)
            {
                return true;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}