[![Build Status](https://dev.azure.com/smhernandez90/DNSUpdater/_apis/build/status/DNS%20Updater%20Service)](https://dev.azure.com/smhernandez90/DNSUpdater/_build/latest?definitionId=1)

# DNS Updater Service
(This will be) A Small Windows service to updated your public DNS with you current public IP.

## Build
You should be able to build this using VS 2017 and [WiX v3.11.1](https://github.com/wixtoolset/wix3/releases/tag/wix3111rtm). Run nuget restore and build, there is a WIX installer project that will produce the msi to install and register the service.

## Install
Once the service is installed:
1. Run the installer
2. Navigate to C:\Program Files (x86)\DNS Updater
3. Edit DnsUpdaterService.exe.config and add your information to the "DNSUpdaterService.Properties.DusApi" section
  a. 
4. Start the service
