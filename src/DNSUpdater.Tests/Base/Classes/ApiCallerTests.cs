using Microsoft.VisualStudio.TestTools.UnitTesting;
using DNSUpdater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http;

namespace DNSUpdater.Tests
{
    [TestClass()]
    public class ApiCallerTests
    {

        private readonly string domainApiCall = $"domains/abadds12ddfs345fgd.com";
        private readonly string domainRecordApiCall = $"domains/abadds12ddfs345fgd.com/records/A/@";
        private readonly EventLog log = new EventLog("DUS Tests",Environment.MachineName, "DUS");

        [TestMethod()]
        public async Task UpdateProviderTest()
        {
            using(var client = new GoDaddyHttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "sso-key", "UzQxLikm_46KxDFnbjN7cQjmw6wocia:46L26ydpkwMaKZV6uVdDWe");
                client.BaseAddress = new Uri("https://api.ote-godaddy.com/v1/");

                client.DomainApiCall = domainApiCall;
                client.DomainRecordApiCall = domainRecordApiCall;

                var apiCaller = new ApiCaller<GoDaddyDomain, GoDaddyDnsRecord>(client, log);

                await apiCaller.UpdateProvider();
            }
        }

        [TestMethod()]
        public async Task GetPublicIPTest()
        {
            string expected = null;
            string actual = null;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://api.ipify.org");
                expected = await response.Content.ReadAsStringAsync();
            }
            using (var client = new GoDaddyHttpClient())
            {
                var apiCaller = new ApiCaller<GoDaddyDomain, GoDaddyDnsRecord>(client, log);
                actual = await apiCaller.GetPublicIP();
            }


            Assert.AreEqual(expected, actual);
        }

    }
}