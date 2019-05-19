using System.Threading.Tasks;
using Microservice.Authentication.Api.Controllers;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Interfaces.Account;
using Microservice.Authentication.Interfaces.Generic;
using Moq;
using Xunit;

namespace Microservice.Authentication.Tests.UnitTests.Controllers.Account
{
    public class ResetPasswordTests
    {
        [Fact]
        public async Task Should_Call_ResetPasswordService()
        {
            // Arrange
            var resetPasswordDto = new ResetPasswordDto
            {
                Email = "test.user@gmail.com",
                Password = "Password123!",
                PasswordResetToken = "1234xyz"
            };

            var sut = new AccountController();
            var mockResetPasswordService = new Mock<IResetPasswordService>();
            mockResetPasswordService.Setup(m => m.Status).Returns(new StatusGenericHandler());

            // Act
            await sut.ResetPassword(resetPasswordDto, mockResetPasswordService.Object);

            // Assert
            mockResetPasswordService.Verify(m => m.ResetPassword(resetPasswordDto), Times.Once);
        }
    }
}