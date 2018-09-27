using DNSUpdaterService.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.ServiceProcess;
using System.Timers;
using System.Configuration;

namespace DNSUpdaterService
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
                Interval = 300000
            };
            timer.Elapsed += new ElapsedEventHandler(OnTimer);
            timer.Start();

        }

        private async void OnTimer(object sender, ElapsedEventArgs e)
        {
            GoDaddyDomain domain = null;
            List<GoDaddyDNSRecord> record = null;

            var goDaddyAPI = new GoDaddyAPICalls();

            using (var client = new GoDaddyHttpClient())
            {
                try
                {
                    domain = await goDaddyAPI.GetDomain(client, GoDaddyAPI.Default.DomainName);
                    EventLog.WriteEntry($"Domain Retrieved: {domain.Domain}");
                }
                catch (HttpRequestException ex)
                {
                    EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
                }

                try
                {
                    record = await goDaddyAPI.GetDomainRecords(client, domain);
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

                //try
                //{
                //    await goDaddyAPI.UpdateDNSRecord(client, domain, record);
                //    EventLog.WriteEntry("DNS Updated Succsessfully", EventLogEntryType.Information);
                //}
                //catch (HttpRequestException ex)
                //{
                //    EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
                //}
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

    }
}
