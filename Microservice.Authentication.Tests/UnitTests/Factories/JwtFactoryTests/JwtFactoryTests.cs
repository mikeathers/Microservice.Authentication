using System.Threading.Tasks;
using Microservice.Authentication.Data.Models.Config;
using Microservice.Authentication.Data.Models.User;
using Microservice.Authentication.Factories.JwtFactory;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Microservice.Authentication.Tests.UnitTests.Factories.JwtFactoryTests
{
    public class JwtFactoryTests
    {
        [Fact]
        public async Task Should_ReturnJwtToken_WhenUser_Provided()
        {
            // Arrange
            var options = Options.Create(new JwtIssuerOptions());
            
            var userMock = new Mock<ApplicationUser>();
            userMock.Setup(m => m.Id).Returns("001");
            userMock.Setup(m => m.Email).Returns("test@test.com");
            userMock.Setup(m => m.FirstName).Returns("Test");
            userMock.Setup(m => m.UserName).Returns("test@test.com");

            var sut = new JwtFactory(options);

            // Act
            var token = await sut.GenerateToken(userMock.Object);

            // Assert
            token.ShouldNotBeNull();
        }

        [Fact]
        public void Should_Return_RefreshToken()
        {
            // Arrange
            var options = Options.Create(new JwtIssuerOptions());

            var sut = new JwtFactory(options);

            // Act
            var token = sut.GenerateRefreshToken();

            // Assert
            token.ShouldNotBeNull();
        }
    }
}
