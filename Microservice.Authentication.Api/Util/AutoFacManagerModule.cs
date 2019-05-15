using System.Linq;
using System.Reflection;
using Autofac;

namespace Microservice.Authentication.Api.Util
{
    public class AutoFacManagerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(GetType().GetTypeInfo().Assembly)
                .Where(type => type.Name.EndsWith("Manager"))
                .AsImplementedInterfaces();
        }
    }
}
