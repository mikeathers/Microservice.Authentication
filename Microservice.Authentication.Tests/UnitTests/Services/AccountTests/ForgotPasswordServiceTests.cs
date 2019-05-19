using System.Threading.Tasks;
using Microservice.Authentication.Data.Models.User;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Interfaces.Account;
using Microservice.Authentication.Interfaces.Generic;
using Microservice.Authentication.Interfaces.Shared;
using Microservice.Authentication.Services.Account;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Microservice.Authentication.Tests.UnitTests.Services.Account
{
    public class ForgotPasswordServiceTests
    {
        [Fact]
        public async Task Should_UseUserManager_ToCheck_UserExists()
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto
            {
                Email = "test@test.com"
            };

            var userMock = new Mock<ApplicationUser>();
            userMock.Setup(m => m.Email).Returns(forgotPasswordDto.Email);

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(m => m.FindByEmailAsync(forgotPasswordDto.Email)).Returns(Task.FromResult(userMock.Object));

            var mockAccountEmailsService = new Mock<IGenerateAccountEmailsService>();

            var mockSendEmailService = new Mock<ISendEmailService>();

            var sut = new ForgotPasswordService(userManagerMock.Object, mockAccountEmailsService.Object, mockSendEmailService.Object);

            // Act
            await sut.RequestPasswordReset(forgotPasswordDto);

            // Assert
            userManagerMock.Verify(m => m.FindByEmailAsync(forgotPasswordDto.Email), Times.Once);
        }


        [Fact]
        public async Task Should_UseUserManager_ToGenerate_ValidPasswordResetToken()
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto
            {
                Email = "test@test.com"
            };

            var userMock = new Mock<ApplicationUser>();
            userMock.Setup(m => m.Email).Returns(forgotPasswordDto.Email);

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(m => m.FindByEmailAsync(forgotPasswordDto.Email)).Returns(Task.FromResult(userMock.Object));
            userManagerMock.Setup(m => m.GeneratePasswordResetTokenAsync(userMock.Object));

            var mockAccountEmailsService = new Mock<IGenerateAccountEmailsService>();

            var mockSendEmailService = new Mock<ISendEmailService>();

            var sut = new ForgotPasswordService(userManagerMock.Object, mockAccountEmailsService.Object, mockSendEmailService.Object);

            // Act
            await sut.RequestPasswordReset(forgotPasswordDto);

            // Assert
            userManagerMock.Verify(m => m.GeneratePasswordResetTokenAsync(userMock.Object), Times.Once);
        }

        [Fact]
        public async Task Should_UseGenerateAccountEmailsService_ToGeneratePasswordResetEmail()
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto
            {
                Email = "test@test.com"
            };

            var userId = "0001";
            var firstName = "Tester User";
            var passwordResetCode = "123xyz";
            var forgotPasswordEmail = "Test Email";

            var userMock = new Mock<ApplicationUser>();

            userMock.SetupAllProperties();
            userMock.Object.FirstName = firstName;

            userMock.Setup(m => m.Email).Returns(forgotPasswordDto.Email);
            userMock.Setup(m => m.Id).Returns(userId);

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(m => m.FindByEmailAsync(forgotPasswordDto.Email)).Returns(Task.FromResult(userMock.Object));
            userManagerMock.Setup(m => m.GeneratePasswordResetTokenAsync(userMock.Object)).Returns(Task.FromResult(passwordResetCode));

            var mockAccountEmailsService = new Mock<IGenerateAccountEmailsService>();
            mockAccountEmailsService.Setup(m => m.ForgotPasswordEmail(firstName, userId, passwordResetCode))
                .Returns(forgotPasswordEmail);

            var mockSendEmailService = new Mock<ISendEmailService>();

            var sut = new ForgotPasswordService(userManagerMock.Object, mockAccountEmailsService.Object, mockSendEmailService.Object);

            // Act
            await sut.RequestPasswordReset(forgotPasswordDto);

            // Assert
            mockAccountEmailsService.Verify(m => m.ForgotPasswordEmail(firstName, userId, passwordResetCode), Times.Once);
        }

        [Fact]
        public async Task Should_UseSendEmailService_ToSendPasswordResetEmail()
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto
            {
                Email = "test@test.com"
            };

            var userId = "0001";
            var firstName = "Tester User";
            var passwordResetCode = "123xyz";
            var forgotPasswordEmail = "Test Email";
            var subject = "Password Reset";
            

            var userMock = new Mock<ApplicationUser>();

            userMock.SetupAllProperties();
            userMock.Object.FirstName = firstName;

            userMock.Setup(m => m.Email).Returns(forgotPasswordDto.Email);
            userMock.Setup(m => m.Id).Returns(userId);

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(m => m.FindByEmailAsync(forgotPasswordDto.Email)).Returns(Task.FromResult(userMock.Object));
            userManagerMock.Setup(m => m.GeneratePasswordResetTokenAsync(userMock.Object)).Returns(Task.FromResult(passwordResetCode));

            var mockAccountEmailsService = new Mock<IGenerateAccountEmailsService>();
            mockAccountEmailsService.Setup(m => m.ForgotPasswordEmail(firstName, userId, passwordResetCode))
                .Returns(forgotPasswordEmail);


            var mockSendEmailService = new Mock<ISendEmailService>();
            mockSendEmailService.Setup(m => m.SendAsync(forgotPasswordDto.Email, subject, forgotPasswordEmail, firstName));
            mockSendEmailService.Setup(m => m.Status).Returns(new StatusGenericHandler());

            var sut = new ForgotPasswordService(userManagerMock.Object, mockAccountEmailsService.Object, mockSendEmailService.Object);

            // Act
            await sut.RequestPasswordReset(forgotPasswordDto);

            // Assert
            mockSendEmailService.Verify(m => m.SendAsync(forgotPasswordDto.Email, subject, forgotPasswordEmail, firstName), Times.Once);
        }

        [Fact]
        public async Task Should_HaveErrors_IfUser_DoesntExist()
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto
            {
                Email = "test@test.com"
            };

            var userMock = new Mock<ApplicationUser>();
            userMock.Setup(m => m.Email).Returns(forgotPasswordDto.Email);

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(m => m.FindByEmailAsync(forgotPasswordDto.Email)).Returns(Task.FromResult((ApplicationUser)null));

            var mockAccountEmailsService = new Mock<IGenerateAccountEmailsService>();

            var mockSendEmailService = new Mock<ISendEmailService>();

            var sut = new ForgotPasswordService(userManagerMock.Object, mockAccountEmailsService.Object, mockSendEmailService.Object);

            // Act
            await sut.RequestPasswordReset(forgotPasswordDto);

            // Assert
            sut.Status.HasErrors.ShouldBeTrue();
        }

        

    }
}