using DNSUpdater;
using DNSUpdater.Base;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DNSUpdaterService.Base.Interfaces
{
    public interface IRegistrar
    {
        Task UpdateRegistrar(EventLog log);
    }
}
