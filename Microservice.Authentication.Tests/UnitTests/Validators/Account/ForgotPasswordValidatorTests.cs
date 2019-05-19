using System.Threading.Tasks;
using Microservice.Authentication.Api.Validators.Account;
using Microservice.Authentication.Dtos.Account;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Microservice.Authentication.Tests.UnitTests.Validators.Account
{
    public class ForgotPasswordValidatorTests
    {
        [Fact]
        public async Task Should_ReturnTrue_WhenRequiredInfo_IsProvided()
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto
            {
                Email = "test@test.com"
            };

            var passwordValidator = new ForgotPasswordValidator();

            // Act
            var validatorResult = await passwordValidator.ValidateAsync(forgotPasswordDto);

            // Assert
            validatorResult.IsValid.ShouldBeTrue();
        }

        [Fact]
        public async Task Should_ReturnFalse_WhenRequiredInfo_IsNotProvided()
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto
            {
                Email = ""
            };

            var passwordValidator = new ForgotPasswordValidator();

            // Act
            var validatorResult = await passwordValidator.ValidateAsync(forgotPasswordDto);

            // Assert
            validatorResult.IsValid.ShouldBeFalse();
        }
    }
}
