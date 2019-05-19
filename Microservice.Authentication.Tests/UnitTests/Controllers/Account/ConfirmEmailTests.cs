using System.Threading.Tasks;
using Microservice.Authentication.Api.Controllers;
using Microservice.Authentication.Api.Helpers.Interfaces;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Interfaces.Account;
using Microservice.Authentication.Interfaces.Generic;
using Moq;
using Xunit;

namespace Microservice.Authentication.Tests.UnitTests.Controllers.Account
{
    public class ConfirmEmailTests
    {
        [Fact]
        public async Task Should_Call_ConfirmEmailService()
        {
            // Arrange
            var confirmEmailDto = new ConfirmEmailDto
            {
                EmailConfirmationToken = "123yxz",
                UserId = "001",
            };

            var sut = new AccountController();
            var mockConfirmEmailService = new Mock<IConfirmEmailService>();
            mockConfirmEmailService.Setup(m => m.Status).Returns(new StatusGenericHandler());

            var mockCreateDtoService = new Mock<ICreateDtoService>();
            mockCreateDtoService.Setup(m =>
                m.ConfirmEmailDto(confirmEmailDto.UserId, confirmEmailDto.EmailConfirmationToken)).Returns(confirmEmailDto);

            // Act
            await sut.ConfirmEmail(confirmEmailDto.UserId, confirmEmailDto.EmailConfirmationToken, mockConfirmEmailService.Object, mockCreateDtoService.Object);

            // Assert
            mockConfirmEmailService.Verify(m => m.ConfirmEmail(confirmEmailDto), Times.Once);
        }
    
    }
}