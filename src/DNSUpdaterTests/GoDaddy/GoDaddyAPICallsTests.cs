using Microsoft.VisualStudio.TestTools.UnitTesting;
using DNSUpdater;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace DNSUpdater.Tests
{
    [TestClass()]
    public class GoDaddyAPICallsTests
    {
        [TestMethod()]
        public async Task GetDomainTest()
        {
            using (var client = new GoDaddyHttpClient())
            {
                var expected = new GoDaddyDomain()
                {
                    Domain = "abadds12ddfs345fgd.com"
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "sso-key", "UzQxLikm_46KxDFnbjN7cQjmw6wocia:46L26ydpkwMaKZV6uVdDWe");
                client.BaseAddress = new Uri("https://api.ote-godaddy.com/v1/");

                var goDaddyApi = new GoDaddyAPICalls();
                var response = await goDaddyApi.GetDomain(client, "abadds12ddfs345fgd.com");
                var actual = await response.Content.ReadAsAsync<GoDaddyDomain>();

                Assert.AreEqual(expected.Domain, actual.Domain);
            }
        }

        [TestMethod()]
        public async Task GetDomainRecordsTest()
        {
            using(var client = new GoDaddyHttpClient())
            {
                var expected = new GoDaddyDNSRecord()
                {
                    name = "@"
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "sso-key", "UzQxLikm_46KxDFnbjN7cQjmw6wocia:46L26ydpkwMaKZV6uVdDWe");
                client.BaseAddress = new Uri("https://api.ote-godaddy.com/v1/");

                var goDaddyApi = new GoDaddyAPICalls();
                var response = await goDaddyApi.GetDomain(client, "abadds12ddfs345fgd.com");
                var domain = await response.Content.ReadAsAsync<GoDaddyDomain>();
                var actual = goDaddyApi.GetDomainRecords(client, domain).GetAwaiter().GetResult();

                Assert.AreEqual(expected.name, actual[0].name);
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

                var goDaddyApi = new GoDaddyAPICalls();

                var expected = "Parked";

                var response = await goDaddyApi.GetDomain(client, "abadds12ddfs345fgd.com");
                var domain = await response.Content.ReadAsAsync<GoDaddyDomain>();
                var records = goDaddyApi.GetDomainRecords(client, domain).GetAwaiter().GetResult();

                goDaddyApi.UpdateDNSRecord(client, domain, records, expected).GetAwaiter();

                var actual = goDaddyApi.GetDomainRecords(client, domain).GetAwaiter().GetResult();

                Assert.AreEqual(expected, actual[0].data);
            }
        }
    }
}