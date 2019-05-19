using System.Collections.Generic;
using System.Threading.Tasks;
using Microservice.Authentication.Data.Models.User;
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

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(m => m.FindByIdAsync(confirmEmailDto.UserId)).Returns(Task.FromResult(userMock.Object));
            userManagerMock.Setup(m => m.ConfirmEmailAsync(userMock.Object, confirmEmailDto.EmailConfirmationToken))
                .Returns(Task.FromResult(IdentityResult.Success));

            var sut = new ConfirmEmailService(userManagerMock.Object);
            
            // Act
            await sut.ConfirmEmail(confirmEmailDto);

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
            userMock.SetupGet(m => m.Id).Returns(confirmEmailDto.UserId);

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(m => m.FindByIdAsync(confirmEmailDto.UserId)).Returns(Task.FromResult(userMock.Object));
            userManagerMock.Setup(m => m.ConfirmEmailAsync(userMock.Object, confirmEmailDto.EmailConfirmationToken))
                .Returns(Task.FromResult(IdentityResult.Success));

            var sut = new ConfirmEmailService(userManagerMock.Object);

            // Act
            await sut.ConfirmEmail(confirmEmailDto);

            // Assert
            userManagerMock.Verify(m => m.ConfirmEmailAsync(userMock.Object, confirmEmailDto.EmailConfirmationToken), Times.Once);
        }

        [Fact]
        public async Task Should_HaveErrors_When_EmailConfirmationFailed()
        {
            // Arrange 
            var confirmEmailDto = new ConfirmEmailDto
            {
                EmailConfirmationToken = "0000",
                UserId = "1234"
            };
            
            var userMock = _fixture.UserMock;
            userMock.SetupGet(m => m.Id).Returns(confirmEmailDto.UserId);

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(m => m.FindByIdAsync(confirmEmailDto.UserId)).Returns(Task.FromResult(userMock.Object));
            userManagerMock.Setup(m => m.ConfirmEmailAsync(userMock.Object, confirmEmailDto.EmailConfirmationToken))
                .Returns(Task.FromResult(IdentityResult.Failed()));

            var sut = new ConfirmEmailService(userManagerMock.Object);

            // Act
            await sut.ConfirmEmail(confirmEmailDto);

            // Assert
            sut.Status.HasErrors.ShouldBeTrue();
        }

        [Theory, MemberData(nameof(ConfirmEmailData))]
        public async Task Should_HaveErrors_WhenRequiredInfo_NotProvided(string userId, string emailConfirmationToken)
        {
            // Arrange 
            var confirmEmailDto = new ConfirmEmailDto
            {
                EmailConfirmationToken = emailConfirmationToken,
                UserId = userId
            };
            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            var sut = new ConfirmEmailService(userManagerMock.Object);

            // Act
            await sut.ConfirmEmail(confirmEmailDto);

            // Assert            
            sut.Status.HasErrors.ShouldBeTrue();
        }
    }
}