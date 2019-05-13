using System;
using System.Linq;
using Microservice.Authentication.Data.Configurations;
using Microservice.Authentication.Dtos.Account;
using TestSupport.EfHelpers;
using Xunit;

namespace Microservice.Authentication.Tests.UnitTests.Services.Account
{
    public class RegisterAccountServiceTests
    {
        [Fact]
        public void Should_RegisterAccount_WhenRequiredInfo_IsProvided()
        {
            var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                context.Database.EnsureCreated();

                var accountToCreate = new RegisterDto
                {
                    FirstName = "Test",
                    LastName = "User",
                    Email = "test.user@gmail.com",
                    Password = "Password123!"
                };



                // Act

                //Assert
            }
        }
    }
}
