using System.Collections.Generic;
using Microservice.Authentication.Api.Validators.Account;
using Microservice.Authentication.Dtos.Account;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Microservice.Authentication.Tests.UnitTests.Validators.Account
{
    public class LoginValidatorTests
    {
        public static IEnumerable<object[]> RegisterData => new List<object[]>
        {
            new []{"test.user@test.com", ""},
            new []{"", "Password123!"},
        };

        [Fact]
        public void Should_ReturnTrue_WhenRequiredInfo_IsProvided()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test.user@gmail.com",
                Password = "Password123!"
            };

            var loginValidator = new LoginValidator();

            // Act
            var validatorResult = loginValidator.Validate(loginDto);

            // Assert
            validatorResult.IsValid.ShouldBeTrue();
        }

        [Theory, MemberData(nameof(RegisterData))]
        public void Should_ReturnFalse_WhenRequiredInfo_IsNotProvided(string email, string password)
        {
            // Arrange
            var loginDto = new LoginDto()
            {
                Email = email,
                Password = password
            };

            var loginValidator = new LoginValidator();

            // Act
            var validatorResult = loginValidator.Validate(loginDto);

            // Assert
            validatorResult.IsValid.ShouldBeFalse();

        }
    }
}