using System.Collections.Generic;
using System.Threading.Tasks;
using Microservice.Authentication.Api.Validators.Account;
using Microservice.Authentication.Dtos.Account;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Microservice.Authentication.Tests.UnitTests.Validators.Account
{
    public class ResetPasswordValidatorTests
    {
        public static IEnumerable<object[]> ResetPasswordData => new List<object[]>
        {
            new []{"", "Password123!!", "1234xyz"},
            new []{"test@test.com", "", "1234xyz"},
            new []{"test@test.com", "Password123!!", ""}
        };

        [Fact]
        public async Task Should_ReturnTrue_WhenRequiredInfo_IsProvided()
        {
            // Arrange
            var resetPasswordDto = new ResetPasswordDto
            {
                Email = "test@test.com",
                Password = "Password123!!",
                PasswordResetToken = "1234xyz"
                
            };

            var resetPasswordValidator = new ResetPasswordValidator();

            // Act
            var validatorResult = await resetPasswordValidator.ValidateAsync(resetPasswordDto);

            // Assert
            validatorResult.IsValid.ShouldBeTrue();
        }

        [Theory, MemberData(nameof(ResetPasswordData))]
        public void Should_ReturnFalse_WhenRequiredInfo_IsNotProvided(string email, string password, string passwordResetToken)
        {
            // Arrange
            var resetPasswordDto = new ResetPasswordDto
            {
                Email = email,
                Password = password,
                PasswordResetToken = passwordResetToken
            };

            var resetPasswordValidator = new ResetPasswordValidator();

            // Act
            var validatorResult = resetPasswordValidator.Validate(resetPasswordDto);

            // Assert
            validatorResult.IsValid.ShouldBeFalse();

        }
    }
}