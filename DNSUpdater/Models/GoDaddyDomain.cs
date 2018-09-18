using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DNSUpdater
{
    public class GoDaddyDomain
    {
        public int DomainId { get; set; }
        public string Domain { get; set; }
        public string Status { get; set; }
        public DateTime Expires { get; set; }
        public bool ExperationProtected { get; set; }
        public bool HoldRegistrar { get; set; }
        public bool Locked { get; set; }
        public bool Privacy { get; set; }
        public bool RenewAuto { get; set; }
        public bool Renewable { get; set; }
        public DateTime RenewDeadline { get; set; }
        public bool TransferProtected { get; set; }
        public DateTime CreatedAt { get; set; }

        //"domainId": 220185845,
        //"domain": "boxedupbuild.com",
        //"status": "ACTIVE",
        //"expires": "2020-07-25T11:41:12Z",
        //"expirationProtected": false,
        //"holdRegistrar": false,
        //"locked": true,
        //"privacy": false,
        //"renewAuto": true,
        //"renewable": true,
        //"renewDeadline": "2020-09-08T11:41:12Z",
        //"transferProtected": false,
        //"createdAt": "2016-07-25T11:41:12Z"
    }
}