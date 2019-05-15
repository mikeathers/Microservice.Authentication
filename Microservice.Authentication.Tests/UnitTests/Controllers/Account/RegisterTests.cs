using System.Threading.Tasks;
using Microservice.Authentication.Api.Controllers;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Interfaces.Account;
using Microservice.Authentication.Interfaces.Generic;
using Moq;
using Xunit;

namespace Microservice.Authentication.Tests.UnitTests.Controllers.Account
{
    public class RegisterTests
    {
        [Fact]
        public async Task Should_Call_RegisterAccountService()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test.user@gmail.com",
                Password = "Password123!"
            };

            var sut = new AccountController();
            var mockRegisterAccountService = new Mock<IRegisterAccountService>();
            mockRegisterAccountService.Setup(m => m.Status).Returns(new StatusGenericHandler());

            // Act
            await sut.Register(registerDto, mockRegisterAccountService.Object);

            // Assert
            mockRegisterAccountService.Verify(m => m.RegisterAccount(registerDto), Times.Once);

        }
    }
}
