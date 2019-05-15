using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace Microservice.Authentication.Services.Util
{
    public class AutoFacServiceModule :Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(GetType().GetTypeInfo().Assembly)
                .Where(c => c.Name.EndsWith("Service"))
                .AsImplementedInterfaces();
        }
    }
}