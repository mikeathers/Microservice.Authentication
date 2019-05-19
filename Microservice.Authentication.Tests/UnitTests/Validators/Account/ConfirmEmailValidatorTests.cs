using System.Collections.Generic;
using Microservice.Authentication.Api.Validators.Account;
using Microservice.Authentication.Dtos.Account;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Microservice.Authentication.Tests.UnitTests.Validators.Account
{
    public class ConfirmEmailValidatorTests
    {
        public static IEnumerable<object[]> ConfirmEmailData => new List<object[]>
        {
            new []{"", "00001"},
            new []{"1234", ""}
        };

        [Fact]
        public void Should_ReturnTrue_WhenRequiredInfo_IsProvided()
        {
            // Arrange
            var confirmEmailDto = new ConfirmEmailDto
            {
                UserId = "1234",
                EmailConfirmationToken = "00001"
            };

            var emailValidator = new ConfirmEmailValidator();

            // Act
            var validatorResult = emailValidator.Validate(confirmEmailDto);

            // Assert
            validatorResult.IsValid.ShouldBeTrue();
        }

        [Theory, MemberData(nameof(ConfirmEmailData))]
        public void Should_ReturnFalse_WhenRequiredInfo_IsNotProvided(string userId, string emailConfirmationToken)
        {
            // Arrange
            var confirmEmailDto = new ConfirmEmailDto()
            {
                UserId = userId,
                EmailConfirmationToken = emailConfirmationToken
            };

            var emailValidator = new ConfirmEmailValidator();

            // Act
            var validatorResult = emailValidator.Validate(confirmEmailDto);

            // Assert
            validatorResult.IsValid.ShouldBeFalse();

        }
    }
}