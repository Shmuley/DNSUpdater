using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Configuration;
using System.Threading.Tasks;
using DNSUpdater.Properties;

namespace DNSUpdater
{
    class Program
    {
        static async Task Main(string[] args)
        {
            EncryptConfigSection("userSettings/DNSUpdater.Properties.GoDaddyAPI");

            GoDaddyAPI.Default.DomainName = args[0];
            GoDaddyAPI.Default.AccessKey = args[1];
            GoDaddyAPI.Default.SecretKey = args[2];

            GoDaddyDomain domain = null;
            List<GoDaddyDNSRecord> record = null;

            var goDaddyAPI = new GoDaddyAPICalls();

            using (var client = new GoDaddyHttpClient())
            {
                try
                {
                    domain = await goDaddyAPI.GetDomain(client, GoDaddyAPI.Default.DomainName);
                    Console.WriteLine($"Domain Retrieved: {domain.Domain}");
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                try
                {
                    record = await goDaddyAPI.GetDomainRecords(client, domain);
                    Console.WriteLine($"Record Retrieved:");
                    foreach (var rec in record)
                    {
                        Console.WriteLine($" {rec.type}");
                        Console.WriteLine($" {rec.name}");
                        Console.WriteLine($" {rec.data}");
                        Console.WriteLine($" {rec.ttl}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                //    try
                //    {
                //        await goDaddyAPI.UpdateDNSRecord(client, domain, record);
                //    }
                //    catch (HttpRequestException ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
            }
        }

        private static void EncryptConfigSection(string sectionKey)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationSection section = config.GetSection(sectionKey);
            if (section != null)
            {
                if (!section.SectionInformation.IsProtected)
                {
                    if (!section.ElementInformation.IsLocked)
                    {
                        section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                        section.SectionInformation.ForceSave = true;
                        config.Save(ConfigurationSaveMode.Full);
                    }
                }
            }

        }
    }
}
