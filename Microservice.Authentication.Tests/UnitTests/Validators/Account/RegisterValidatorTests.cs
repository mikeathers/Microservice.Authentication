using System.Collections.Generic;
using Microservice.Authentication.Api.Validators.Account;
using Microservice.Authentication.Dtos.Account;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Microservice.Authentication.Tests.UnitTests.Validators.Account
{
    public class RegisterValidatorTests
    {

        public static IEnumerable<object[]> RegisterData => new List<object[]>
        {
            new []{"", "User", "test.user@test.com", "Password123!"},
            new []{"Test", "", "test.user@test.com", "Password123!"},
            new []{"Test", "User", "", "Password123!"},
            new []{"Test", "User", "test.user@test.com", ""},
        };

        [Fact]
        public void Should_ReturnTrue_WhenRequiredInfo_IsProvided()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test.user@gmail.com",
                Password = "Password123!"
            };

            var registerValidator = new EmailValidator();

            // Act
            var validatorResult = registerValidator.Validate(registerDto);

            // Assert
            validatorResult.IsValid.ShouldBeTrue();
        }

        [Theory, MemberData(nameof(RegisterData))]
        public void Should_ReturnFalse_WhenRequiredInfo_IsNotProvided(string firstName, string lastName, string email, string password)
        {
            // Arrange
            var registerDto = new RegisterDto()
            {
                FirstName =  firstName,
                LastName =  lastName,
                Email = email,
                Password = password
            };

            var registerValidator = new EmailValidator();

            // Act
            var validatorResult = registerValidator.Validate(registerDto);

            // Assert
            validatorResult.IsValid.ShouldBeFalse();

        }
    }
}
