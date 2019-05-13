using AutoMapper;
using Microservice.Authentication.Services.AutoMapper;

namespace Microservice.Authentication.Services.Util
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(mappping =>
            {
                mappping.AddProfile<AccountMapping>();
            });
        }
    }
}