namespace DNSUpdater.Base
{
    public interface IDnsRecord
    {
#pragma warning disable IDE1006 // Using proper case for these causes an error with the GoDaddy API
        string data { get; set; }
#pragma warning disable IDE1006 // Using proper case for these causes an error with the GoDaddy API
        string name { get; set; }
#pragma warning disable IDE1006 // Using proper case for these causes an error with the GoDaddy API
        int ttl { get; set; }
#pragma warning disable IDE1006 // Using proper case for these causes an error with the GoDaddy API
        string type { get; set; }
    }
}