using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Configuration;
using DNSUpdaterService.Properties;

namespace DNSUpdater.Tests
{
    [TestClass()]
    public class GoDaddyAPICallsTests
    {
        private static readonly string _domain = "123mainst.cc";
        private readonly string _domainApiCall = $"domains/{_domain}";
        private readonly string _domainRecordApiCall = $"domains/{_domain}/records/A";
        private readonly EventLog _log = new EventLog("DUS Tests", Environment.MachineName, "DUS");

        [TestMethod()]
        public async Task GetDomainTest()
        {
            using (var client = new GoDaddyHttpClient())
            {
                var expected = new GoDaddyDomain()
                {
                    Domain = _domain
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "sso-key", "UzQxLikm_46KxDFnbjN7cQjmw6wocia:46L26ydpkwMaKZV6uVdDWe");
                client.BaseAddress = new Uri("https://api.ote-godaddy.com/v1/");
                client.DomainApiCall = _domainApiCall;

                var goDaddyApi = new ApiCaller<GoDaddyDomain, GoDaddyDnsRecord>(client, _log);
                var domain = await goDaddyApi.GetDomain();

                Assert.AreEqual(expected.Domain, domain.Domain);
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
                client.DomainApiCall = _domainApiCall;
                client.DomainRecordApiCall = _domainRecordApiCall;

                var goDaddyApi = new ApiCaller<GoDaddyDomain, GoDaddyDnsRecord>(client, _log);
                var domain = await goDaddyApi.GetDomain();
                var records = await goDaddyApi.GetDomainRecords();

                Assert.IsTrue(records.Any(r => r.name == expected.name));

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
                client.DomainApiCall = _domainApiCall;
                client.DomainRecordApiCall = _domainRecordApiCall;

                var goDaddyApi = new ApiCaller<GoDaddyDomain, GoDaddyDnsRecord>(client, _log);

                var expected = await goDaddyApi.GetPublicIP();
                var domain = await goDaddyApi.GetDomain();
                var records = await goDaddyApi.GetDomainRecords();
                await goDaddyApi.UpdateDnsRecord(domain, records);
                var newRecord = await goDaddyApi.GetDomainRecords();

                Assert.IsTrue(newRecord.Any(r => r.data == expected));
            }
        }
    }
}