using System.Threading.Tasks;
using Microservice.Authentication.Api.Controllers;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Interfaces.Account;
using Microservice.Authentication.Interfaces.Generic;
using Moq;
using Xunit;

namespace Microservice.Authentication.Tests.UnitTests.Controllers.Account
{
    public class ForgotPasswordTests
    {
        [Fact]
        public async Task Should_Call_ForgotPasswordService()
        {
            
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto
            {
                Email = "test.user@gmail.com"
            };

            var sut = new AccountController();
            var mockLoginService = new Mock<IForgotPasswordService>();
            mockLoginService.Setup(m => m.Status).Returns(new StatusGenericHandler());

            // Act
            await sut.ForgotPassword(forgotPasswordDto, mockLoginService.Object);

            // Assert
            mockLoginService.Verify(m => m.RequestPasswordReset(forgotPasswordDto), Times.Once);
        
        }
    }
}