using Microsoft.VisualStudio.TestTools.UnitTesting;
using DNSUpdater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http.Headers;

namespace DNSUpdater.Tests
{
    [TestClass()]
    public class GoDaddyAPICallsTests
    {
        [TestMethod()]
        public void GetDomainTest()
        {
            var expected = new GoDaddyDomain()
            {
                Domain = "hernanfam.com"
            };

            var client = new GoDaddyHttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("sso-key", "AEdBCuyrDvW_6XVDcvdAc7MbSoQbUkqkwq:6XVGwL6a88WW2V1G1fDiHd");

            var goDaddyApi = new GoDaddyAPICalls();
            var actual = goDaddyApi.GetDomain(client, "hernanfam.com").GetAwaiter().GetResult();

            Assert.AreEqual(expected.Domain, actual.Domain);
        }

        [TestMethod()]
        public void GetDomainRecordsTest()
        {
            var expected = new GoDaddyDNSRecord()
            {
                name = "@"
            };

            var client = new GoDaddyHttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("sso-key", "AEdBCuyrDvW_6XVDcvdAc7MbSoQbUkqkwq:6XVGwL6a88WW2V1G1fDiHd");

            var goDaddyApi = new GoDaddyAPICalls();
            var domain = goDaddyApi.GetDomain(client, "hernanfam.com").GetAwaiter().GetResult();
            var actual = goDaddyApi.GetDomainRecords(client, domain).GetAwaiter().GetResult();

            Assert.AreEqual(expected.name, actual[0].name);
        }
    }
}