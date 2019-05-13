using System.Collections.Generic;
using System.Threading.Tasks;
using Microservice.Authentication.Api.Controllers;
using Microservice.Authentication.Dtos.Account;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Microservice.Authentication.Tests.UnitTests.Controllers.Account
{
    public class RegisterTests
    {

        public static IEnumerable<object[]> RegisterData => new List<object[]>
        {
            new []{"", "User", "test.user@test.com", "Password123!"},
            new []{"Test", "", "test.user@test.com", "Password123!"},
            new []{"Test", "User", "", "Password123!"},
            new []{"Test", "User", "test.user@test.com", ""},
        };


        [Fact]
        public async Task Should_RegisterUser_WhenRequiredInfo_IsProvided()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test.user@gmail.com",
                Password = "Password123!"
            };

            var accountController = new AccountController();

            // Act
            var result = await accountController.Register(registerDto);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldEqual(result as OkObjectResult);
        }

        [Theory, MemberData(nameof(RegisterData))]
        public async Task Should_ReturnError_WhenRequired_IsNotProvided(string firstName, string lastName, string email, string password)
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };

            var accountController = new AccountController();

            // Act
            var result = await accountController.Register(registerDto);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldEqual(result as BadRequestObjectResult);
        }

        
    }
}
