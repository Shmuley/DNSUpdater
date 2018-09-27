using Microsoft.VisualStudio.TestTools.UnitTesting;
using DNSUpdaterService;
using System;
using System.Net.Http.Headers;
using System.Net.Http;

namespace DNSUpdater.Tests
{
    [TestClass()]
    public class GoDaddyAPICallsTests
    {
        [TestMethod()]
        public void GetDomainTest()
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
                var actual = goDaddyApi.GetDomain(client, "abadds12ddfs345fgd.com").GetAwaiter().GetResult();

                Assert.AreEqual(expected.Domain, actual.Domain);
            }
        }

        [TestMethod()]
        public void GetDomainRecordsTest()
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
                var domain = goDaddyApi.GetDomain(client, "abadds12ddfs345fgd.com").GetAwaiter().GetResult();
                var actual = goDaddyApi.GetDomainRecords(client, domain).GetAwaiter().GetResult();

                Assert.AreEqual(expected.name, actual[0].name);
            }
        }

        [TestMethod()]
        public void UpdateDNSRecordTest()
        {
            using (var client = new GoDaddyHttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "sso-key", "UzQxLikm_46KxDFnbjN7cQjmw6wocia:46L26ydpkwMaKZV6uVdDWe");
                client.BaseAddress = new Uri("https://api.ote-godaddy.com/v1/");

                var goDaddyApi = new GoDaddyAPICalls();

                var expected = "Parked";

                var domain = goDaddyApi.GetDomain(client, "abadds12ddfs345fgd.com").GetAwaiter().GetResult();
                var records = goDaddyApi.GetDomainRecords(client, domain).GetAwaiter().GetResult();

                goDaddyApi.UpdateDNSRecord(client, domain, records).GetAwaiter();

                var actual = goDaddyApi.GetDomainRecords(client, domain).GetAwaiter().GetResult();

                Assert.AreEqual(expected, actual[0].data);
            }
        }

        [TestMethod()]
        public void GetPublicIPTest()
        {
            string expected = null;

            using(var client = new HttpClient())
            {
               var response = client.GetAsync("https://api.ipify.org").GetAwaiter().GetResult();
               expected = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            var goDaddyApi = new GoDaddyAPICalls();
            var actual = goDaddyApi.GetPublicIP().GetAwaiter().GetResult();

            Assert.AreEqual(expected, actual);

        }
    }
}