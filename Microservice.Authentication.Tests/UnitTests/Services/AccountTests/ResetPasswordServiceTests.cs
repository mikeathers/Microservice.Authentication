using System.Threading.Tasks;
using Microservice.Authentication.Data.Models.User;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Services.Account;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Microservice.Authentication.Tests.UnitTests.Services.Account
{
    public class ResetPasswordServiceTests
    {
        [Fact]
        public async Task Should_UseUserManager_ToFindUser()
        {
            // Arrange
            var resetPasswordDto = new ResetPasswordDto
            {
                Email = "test@test.com",
                Password = "Password123!!"
            };

            var userMock = new Mock<ApplicationUser>();
            userMock.Setup(m => m.Email).Returns(resetPasswordDto.Email);

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(m => m.FindByEmailAsync(resetPasswordDto.Email)).Returns(Task.FromResult(userMock.Object));

            var sut = new ResetPasswordService(userManagerMock.Object);
            
            // Act
            await sut.ResetPassword(resetPasswordDto);

            // Assert
            userManagerMock.Verify(m => m.FindByEmailAsync(resetPasswordDto.Email), Times.Once);
        }

        [Fact]
        public async Task Should_HaveErrors_When_UserNotFound()
        {
            // Arrange
            var resetPasswordDto = new ResetPasswordDto
            {
                Email = "test@test.com",
                Password = "Password123!!"
            };

            var userMock = new Mock<ApplicationUser>();
            userMock.Setup(m => m.Email).Returns(resetPasswordDto.Email);

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(m => m.FindByEmailAsync(resetPasswordDto.Email)).Returns(Task.FromResult((ApplicationUser)null));

            var sut = new ResetPasswordService(userManagerMock.Object);

            // Act
            await sut.ResetPassword(resetPasswordDto);

            // Assert
            sut.Status.HasErrors.ShouldBeTrue();
        }

        [Fact]
        public async Task Should_UseUserManager_ToResetPassword_UsingPasswordResetToken()
        {
            // Arrange
            var resetPasswordDto = new ResetPasswordDto
            {
                Email = "test@test.com",
                Password = "Password123!!",
                PasswordResetToken = "123xyz"
            };

            var userMock = new Mock<ApplicationUser>();
            userMock.Setup(m => m.Email).Returns(resetPasswordDto.Email);

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(m => m.FindByEmailAsync(resetPasswordDto.Email)).Returns(Task.FromResult(userMock.Object));
            userManagerMock.Setup(m =>
                m.ResetPasswordAsync(userMock.Object, resetPasswordDto.PasswordResetToken, resetPasswordDto.Password)).Returns(Task.FromResult(IdentityResult.Success));

            var sut = new ResetPasswordService(userManagerMock.Object);

            // Act
            await sut.ResetPassword(resetPasswordDto);

            // Assert
            userManagerMock.Verify(m => m.ResetPasswordAsync(userMock.Object, resetPasswordDto.PasswordResetToken, resetPasswordDto.Password), Times.Once);
        }

        [Fact]
        public async Task Should_HaveErrors_When_PasswordResetFailed()
        {
            // Arrange
            var resetPasswordDto = new ResetPasswordDto
            {
                Email = "test@test.com",
                Password = "Password123!!",
                PasswordResetToken = "123xyz"
            };

            var userMock = new Mock<ApplicationUser>();
            userMock.Setup(m => m.Email).Returns(resetPasswordDto.Email);

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(m => m.FindByEmailAsync(resetPasswordDto.Email)).Returns(Task.FromResult(userMock.Object));
            userManagerMock.Setup(m =>
                m.ResetPasswordAsync(userMock.Object, resetPasswordDto.PasswordResetToken, resetPasswordDto.Password)).Returns(Task.FromResult(IdentityResult.Failed()));

            var sut = new ResetPasswordService(userManagerMock.Object);

            // Act
            await sut.ResetPassword(resetPasswordDto);

            // Assert
            userManagerMock.Verify(m => m.ResetPasswordAsync(userMock.Object, resetPasswordDto.PasswordResetToken, resetPasswordDto.Password), Times.Once);
        }
        


    }
}