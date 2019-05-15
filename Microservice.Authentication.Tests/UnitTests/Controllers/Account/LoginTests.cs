using System.Threading.Tasks;
using Microservice.Authentication.Api.Controllers;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Interfaces.Account;
using Microservice.Authentication.Interfaces.Generic;
using Moq;
using Xunit;

namespace Microservice.Authentication.Tests.UnitTests.Controllers.Account
{
    public class LoginTests
    {
        [Fact]
        public async Task Should_Call_LoginService()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test.user@gmail.com",
                Password = "Password123!"
            };

            var sut = new AccountController();
            var mockLoginService = new Mock<ILoginService>();
            mockLoginService.Setup(m => m.Status).Returns(new StatusGenericHandler());

            // Act
            await sut.Login(loginDto, mockLoginService.Object);

            // Assert
            mockLoginService.Verify(m => m.Login(loginDto), Times.Once);
        }
    }
}