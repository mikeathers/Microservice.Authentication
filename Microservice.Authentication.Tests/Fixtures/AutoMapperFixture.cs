using System;
using AutoMapper;
using Microservice.Authentication.Services.AutoMapper;

namespace Microservice.Authentication.Tests.Fixtures
{
    public class AutoMapperFixture : IDisposable
    {
        public IMapper MapperFixture;

        public AutoMapperFixture()
        {
            var config = new MapperConfiguration(mapper =>
            {
                mapper.AddProfile<AccountMapping>();
            });

            MapperFixture = config.CreateMapper();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}