using DNSUpdater.Base;
using DNSUpdaterService.Properties;
using System;
using System.Configuration;
using System.Net.Http.Headers;

namespace DNSUpdater
{
    public class GoDaddyHttpClient : DusHttpClient
    {
        public GoDaddyHttpClient()
        {
            Uri = new Uri(ConfigurationManager.AppSettings["GDURL"]);
            HeaderValue = new MediaTypeWithQualityHeaderValue(ConfigurationManager.AppSettings["GDRequestHeaders"]);
            SsoKey = new AuthenticationHeaderValue(
                "sso-key", $"{DusApi.Default.AccessKey}:{DusApi.Default.SecretKey}");

            BaseAddress = Uri;
            DefaultRequestHeaders.Clear();
            DefaultRequestHeaders.Accept.Add(HeaderValue);
            DefaultRequestHeaders.Authorization = SsoKey;

            //Need to test and make sure this is actually setting the property
            if (string.IsNullOrEmpty(DusApi.Default.RecordName))
            {
                DusApi.Default.RecordName = "@";
                DusApi.Default.Save();
            }

            DomainRecordApiCall = $"domains/{DusApi.Default.DomainName}/records/A/{DusApi.Default.RecordName}";
            DomainApiCall = $"domains/{DusApi.Default.DomainName}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            if (obj is GoDaddyHttpClient compare &&
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