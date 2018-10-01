namespace DNSUpdater.Base
{
    public interface IDnsDomain
    {
        int DomainId { get; set; }
        string Domain { get; set; }
    }
}