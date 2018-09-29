using Microsoft.VisualStudio.TestTools.UnitTesting;
using DNSUpdater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace DNSUpdater.Tests
{
    [TestClass()]
    public class DNSUpdaterServiceTests
    {
        [TestMethod()]
        public async Task GetPublicIPTest()
        {
            string expected = null;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://api.ipify.org");
                expected = await response.Content.ReadAsStringAsync();
            }
            var actual = await DNSUpdaterService.GetPublicIP();

            Assert.AreEqual(expected, actual);
        }
    }
}