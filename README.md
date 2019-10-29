| Branch  | Status |
|---|---|
| dev  | [![Build Status](https://dev.azure.com/SamboHernando/DNSUpdater/_apis/build/status/DNS%20Updater%20-%20dev?branchName=dev)](https://dev.azure.com/SamboHernando/DNSUpdater/_build/latest?definitionId=1&branchName=dev) |
| master  | [![Build Status](https://dev.azure.com/SamboHernando/DNSUpdater/_apis/build/status/DNS%20Updater%20-%20master?branchName=master)](https://dev.azure.com/SamboHernando/DNSUpdater/_build/latest?definitionId=3&branchName=master) |

# DNS Updater Service
(This will be) A Small Windows service to updated your public DNS with you current public IP.

## Build
You should be able to build this using VS 2017 and [WiX v3.11.1](https://github.com/wixtoolset/wix3/releases/tag/wix3111rtm). Run nuget restore and build, there is a WIX installer project that will produce the msi to install and register the service.

## Install
Once the service is installed:
1. Navigate to C:\Program Files (x86)\DNS Updater
2. Edit DnsUpdaterService.exe.config and add your information to the "DNSUpdaterService.Properties.DusApi" section
![Image of App.config](https://github.com/SmartyPantalones/DNSUpdater/blob/master/.github/images/Code_2018-10-03_10-38-38.png)
3. Start the service
