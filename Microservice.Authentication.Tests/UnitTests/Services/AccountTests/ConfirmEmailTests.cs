using System.Collections.Generic;
using System.Threading.Tasks;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Services.Account;
using Microservice.Authentication.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Microservice.Authentication.Tests.UnitTests.Services.Account
{
    public class ConfirmEmailTests : IClassFixture<AccountServicesTestsFixture>
    {
        private readonly AccountServicesTestsFixture _fixture;

        public ConfirmEmailTests(AccountServicesTestsFixture fixture)
        {
            _fixture = fixture;
        }

        public static IEnumerable<object[]> ConfirmEmailData => new List<object[]>
        {
            new []{"", "1233"},
            new []{"001", ""},
        };

        [Fact]
        public async Task Should_UseUserManager_ToFindUser()
        {
            // Arrange 
            var confirmEmailDto = new ConfirmEmailDto
            {
                EmailConfirmationToken = "0000",
                UserId = "1234"
            };

            var userMock = _fixture.UserMock;
            userMock.Setup(m => m.Id).Returns(confirmEmailDto.UserId);

            var userManagerMock = _fixture.UserManagerMock;
            userManagerMock.Setup(m => m.FindByIdAsync(confirmEmailDto.UserId)).Returns(Task.FromResult(userMock.Object));
            userManagerMock.Setup(m => m.ConfirmEmailAsync(userMock.Object, confirmEmailDto.EmailConfirmationToken))
                .Returns(Task.FromResult(IdentityResult.Success));

            var sut = new ConfirmEmailService(userManagerMock.Object);
            
            // Act
            var emailConfirmed = await sut.ConfirmEmail(confirmEmailDto.UserId, confirmEmailDto.EmailConfirmationToken);

            // Assert
            userManagerMock.Verify(m => m.FindByIdAsync(confirmEmailDto.UserId), Times.Once);
        }

        [Fact]
        public async Task Should_UseUserManager_ToConfirmEmail()
        {
            // Arrange 
            var confirmEmailDto = new ConfirmEmailDto
            {
                EmailConfirmationToken = "0000",
                UserId = "1234"
            };

            var userMock = _fixture.UserMock;
            userMock.Setup(m => m.Id).Returns(confirmEmailDto.UserId);

            var userManagerMock = _fixture.UserManagerMock;
            userManagerMock.Setup(m => m.FindByIdAsync(confirmEmailDto.UserId)).Returns(Task.FromResult(userMock.Object));
            userManagerMock.Setup(m => m.ConfirmEmailAsync(userMock.Object, confirmEmailDto.EmailConfirmationToken))
                .Returns(Task.FromResult(IdentityResult.Success));

            var sut = new ConfirmEmailService(userManagerMock.Object);

            // Act
            var emailConfirmed = await sut.ConfirmEmail(confirmEmailDto.UserId, confirmEmailDto.EmailConfirmationToken);

            // Assert
            userManagerMock.Verify(m => m.ConfirmEmailAsync(userMock.Object, confirmEmailDto.EmailConfirmationToken), Times.Once);
        }

        [Theory, MemberData(nameof(ConfirmEmailData))]
        public async Task Should_HaveErrors_WhenRequiredInfo_NotProvided(string userId, string emailConfirmationToken)
        {
            // Arrange 
           

            var userManagerMock = _fixture.UserManagerMock;
            var sut = new ConfirmEmailService(userManagerMock.Object);

            // Act
            var emailConfirmed = await sut.ConfirmEmail(userId, emailConfirmationToken);

            // Assert
            emailConfirmed.ShouldBeFalse();
            sut.Status.HasErrors.ShouldBeTrue();
                

        }
    }
}