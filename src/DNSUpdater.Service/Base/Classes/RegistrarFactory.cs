using DNSUpdaterService.Base.Interfaces;
using System;
using System.Configuration;

namespace DNSUpdaterService.Base.Classes
{
    public static class RegistrarFactory
    {
        public static IRegistrar CreateRegistrar()
        {
            var registrarType = ConfigurationManager.AppSettings["Registrar"];
            Type type = Type.GetType(registrarType);
            IRegistrar registrar = Activator.CreateInstance(type) as IRegistrar;

            return registrar;
        }
    }
}
