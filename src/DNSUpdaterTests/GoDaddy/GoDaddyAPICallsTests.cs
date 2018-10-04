using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DNSUpdater.Tests
{
    [TestClass()]
    public class GoDaddyAPICallsTests
    {
        private readonly string domain = "abadds12ddfs345fgd.com";
        private readonly string domainApiCall = $"domains/abadds12ddfs345fgd.com";
        private readonly string domainRecordApiCall = $"domains/abadds12ddfs345fgd.com/records/A/@";

        [TestMethod()]
        public async Task GetDomainTest()
        {
            using (var client = new GoDaddyHttpClient())
            {
                var expected = new GoDaddyDomain()
                {
                    Domain = domain
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "sso-key", "UzQxLikm_46KxDFnbjN7cQjmw6wocia:46L26ydpkwMaKZV6uVdDWe");
                client.BaseAddress = new Uri("https://api.ote-godaddy.com/v1/");
                client.DomainApiCall = domainApiCall;

                var goDaddyApi = new ApiCaller<GoDaddyDomain, GoDaddyDnsRecord>();
                var response = await goDaddyApi.GetDomain(client);

                Assert.IsTrue(response.IsSuccessStatusCode);
            }
        }

        [TestMethod()]
        public async Task GetDomainRecordsTest()
        {
            using(var client = new GoDaddyHttpClient())
            {
                var expected = new GoDaddyDnsRecord()
                {
                    name = "@"
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "sso-key", "UzQxLikm_46KxDFnbjN7cQjmw6wocia:46L26ydpkwMaKZV6uVdDWe");
                client.BaseAddress = new Uri("https://api.ote-godaddy.com/v1/");
                client.DomainApiCall = domainApiCall;
                client.DomainRecordApiCall = domainRecordApiCall;

                var goDaddyApi = new ApiCaller<GoDaddyDomain, GoDaddyDnsRecord>();
                var response = await goDaddyApi.GetDomain(client);
                var domain = await response.Content.ReadAsAsync<GoDaddyDomain>();
                var recordsResponse = await goDaddyApi.GetDomainRecords(client);

                Assert.IsTrue(recordsResponse.IsSuccessStatusCode);

            }
        }

        [TestMethod()]
        public async Task UpdateDNSRecordTest()
        {
            using (var client = new GoDaddyHttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "sso-key", "UzQxLikm_46KxDFnbjN7cQjmw6wocia:46L26ydpkwMaKZV6uVdDWe");
                client.BaseAddress = new Uri("https://api.ote-godaddy.com/v1/");
                client.DomainApiCall = domainApiCall;
                client.DomainRecordApiCall = domainRecordApiCall;

                var goDaddyApi = new ApiCaller<GoDaddyDomain, GoDaddyDnsRecord>();

                var expected = await DNSUpdaterService.GetPublicIP();

                var response = await goDaddyApi.GetDomain(client);
                var domain = await response.Content.ReadAsAsync<GoDaddyDomain>();
                var recordsResponse = await goDaddyApi.GetDomainRecords(client);
                var records = await recordsResponse.Content.ReadAsAsync<IList<GoDaddyDnsRecord>>();
                var updateResponse = await goDaddyApi.UpdateDnsRecord(client, domain, records, expected);

                Assert.IsTrue(updateResponse.IsSuccessStatusCode);
            }
        }
    }
}