using System;
using System.Diagnostics;
using System.Net.Http;
using System.ServiceProcess;
using System.Timers;
using System.Configuration;
using System.Threading.Tasks;
using DNSUpdaterService.Properties;
using DNSUpdaterService.Base.Classes;

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

        protected async override void OnStart(string[] args)
        {
            if (string.IsNullOrEmpty(DusApi.Default.DomainName) ||
                string.IsNullOrEmpty(DusApi.Default.AccessKey)  ||
                string.IsNullOrEmpty(DusApi.Default.SecretKey))
            {
                throw new NullReferenceException(
                    "A domain name, access key, or secret key has " +
                    "not been specified in the application config file. " +
                    "Please specify these settings and try again.");
            }

            EncryptConfigSection("userSettings/DNSUpdaterService.Properties.DusApi");

            if (Environment.UserInteractive)
            {
                var registrar = RegistrarFactory.CreateRegistrar();
                await registrar.UpdateRegistrar(EventLog);
            }
            else
            {
                Timer timer = new Timer()
                {
                    Interval = 300000
                };
                timer.Elapsed += new ElapsedEventHandler(OnTimer);
                timer.Start();
            }
        }

        private async void OnTimer(object sender, ElapsedEventArgs e)
        {
            var registrar = RegistrarFactory.CreateRegistrar();
            await registrar.UpdateRegistrar(EventLog);
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
