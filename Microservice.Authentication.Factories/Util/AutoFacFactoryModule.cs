using System.Reflection;
using Autofac;

namespace Microservice.Authentication.Factories.Util
{
    public class AutoFacFactoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(GetType().GetTypeInfo().Assembly)
                .Where(type => type.Name.EndsWith("Factory"))
                .AsImplementedInterfaces();
        }
    }
}
