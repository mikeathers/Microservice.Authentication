using System.Reflection;
using Autofac;

namespace Microservice.Authentication.Services.Util
{
    public class AutoFacServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(GetType().GetTypeInfo().Assembly)
                .Where(type => type.Name.EndsWith("Service"))
                .AsImplementedInterfaces();
        }
    }
}