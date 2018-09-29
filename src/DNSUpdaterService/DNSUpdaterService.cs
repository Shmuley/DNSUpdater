using DNSUpdater.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.ServiceProcess;
using System.Timers;
using System.Configuration;
using System.Threading.Tasks;
using DNSUpdaterService.Base;

namespace DNSUpdater
{
    public partial class DNSUpdaterService : ServiceBase
    {
        public DNSUpdaterService()
        {
            InitializeComponent();

            ServiceName = "DNSUpdaterService";
            CanStop = true;
            CanPauseAndContinue = true;
            AutoLog = true;

            EventLog.WriteEntry("Service Starting", EventLogEntryType.Information);
        }

        internal void TestStartAndStop(string[] args)
        {
            OnStart(args);
            Console.ReadLine();
            OnStop();
        }

        protected override void OnStart(string[] args)
        {
            EncryptConfigSection("userSettings/DNSUpdaterService.Properties.GoDaddyAPI");

            Timer timer = new Timer()
            {
                Interval = 10000
            };
            timer.Elapsed += new ElapsedEventHandler(OnTimer);
            timer.Start();

        }

        private async void OnTimer(object sender, ElapsedEventArgs e)
        {
            GoDaddyDomain domain = null;
            List<GoDaddyDNSRecord> record = null;

            var apiCaller = new GoDaddyAPICalls();

            using (var client = new GoDaddyHttpClient())
            {
                try
                {
                    var response = await apiCaller.GetDomain(client, GoDaddyAPI.Default.DomainName);

                    if (response.IsSuccessStatusCode)
                    {
                        domain = await response.Content.ReadAsAsync<GoDaddyDomain>();
                        EventLog.WriteEntry($"Domain Retrieved: {domain.Domain}");
                    }
                    else
                    {
                        EventLog.WriteEntry(await response.Content.ReadAsStringAsync(), EventLogEntryType.Error);
                    }

                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
                }

                if (domain != null)
                {
                    try
                    {
                        record = await apiCaller.GetDomainRecords(client, domain);
                        foreach (var rec in record)
                        {
                            EventLog.WriteEntry($"Record Retrieved: " +
                                $"Type: {rec.type} " +
                                $"Name: {rec.name} " +
                                $"Data: {rec.data} " +
                                $"TTL: {rec.ttl}");
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
                    }
                }

                if (record != null)
                {
                    try
                    {
                        await apiCaller.UpdateDNSRecord(client, domain, record, await GetPublicIP());
                        EventLog.WriteEntry("DNS Updated Succsessfully", EventLogEntryType.Information);
                    }
                    catch (Exception ex)
                    {
                        EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
                    }

                }
            }

        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("Stopping Service");
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

        public static async Task<string> GetPublicIP()
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri("https://api.ipify.org");
                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var ip = await response.Content.ReadAsStringAsync();
                    return ip;
                }
                else
                {
                    return null;
                }
            }
        }

    }
}
