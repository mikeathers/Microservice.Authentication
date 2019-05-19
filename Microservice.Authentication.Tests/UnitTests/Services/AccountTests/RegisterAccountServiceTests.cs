using System.Threading.Tasks;
using Microservice.Authentication.Data.Configurations;
using Microservice.Authentication.Data.Models.User;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Interfaces.Generic;
using Microservice.Authentication.Interfaces.Shared;
using Microservice.Authentication.Services.Account;
using Microservice.Authentication.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Moq;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Microservice.Authentication.Tests.UnitTests.Services.Account
{
    public class RegisterAccountServiceTests : IClassFixture<AccountServicesTestsFixture>
    {
        private readonly AccountServicesTestsFixture _fixture;

        public RegisterAccountServiceTests(AccountServicesTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_UseUserManager_ToRegister_Account()
        {
            var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                context.Database.EnsureCreated();

                var accountToCreate = new RegisterDto
                {
                    FirstName = "Test",
                    LastName = "User",
                    Email = "test.user@gmail.com",
                    Password = "Password123!"
                };

                var user = _fixture.UserMock;
                var store = new Mock<IUserStore<ApplicationUser>>();
                var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
                userManagerMock.Setup(m => m.CreateAsync(user.Object, "Password123!")).Returns(Task.FromResult(IdentityResult.Success));

                var errorFactoryMock = _fixture.ErrorFactoryMock;

                var mapperMock = _fixture.MapperMock;
                mapperMock.Setup(m => m.Map<RegisterDto, ApplicationUser>(accountToCreate)).Returns(user.Object);

                var sendEmailServiceMock = new Mock<ISendEmailService>();
                var confirmationEmailServiceMock = _fixture.ConfirmationEmailServiceMock;

                var sut = new RegisterAccountService(userManagerMock.Object, context, 
                    errorFactoryMock.Object, mapperMock.Object, sendEmailServiceMock.Object, confirmationEmailServiceMock.Object); 

                // Act
                await sut.RegisterAccount(accountToCreate);

                //Assert
                userManagerMock.Verify(m => m.CreateAsync(user.Object, "Password123!"), Times.Once);

            }
        }

        [Fact]
        public async Task Should_UseSendEmailService_ToSend_AccountConfimationEmail()
        {
            var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                context.Database.EnsureCreated();

                var accountToCreate = new RegisterDto
                {
                    FirstName = "Test",
                    LastName = "User",
                    Email = "test.user@gmail.com",
                    Password = "Password123!"
                };

                var confirmationEmail = "Test Confirmation Email";
                var userId = "0001";
                var emailConfirmationCode = "1234";


                var user = _fixture.UserMock;
                user.Setup(m => m.Id).Returns(userId);
                user.Setup(m => m.Email).Returns(accountToCreate.Email);

                var store = new Mock<IUserStore<ApplicationUser>>();
                var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
                userManagerMock.Setup(m => m.CreateAsync(user.Object, "Password123!")).Returns(Task.FromResult(IdentityResult.Success));
                userManagerMock.Setup(m => m.GenerateEmailConfirmationTokenAsync(user.Object))
                    .Returns(Task.FromResult(emailConfirmationCode));

                var errorFactoryMock = _fixture.ErrorFactoryMock;

                var mapperMock = _fixture.MapperMock;
                mapperMock.Setup(m => m.Map<RegisterDto, ApplicationUser>(accountToCreate)).Returns(user.Object);

                var sendEmailServiceMock = new Mock<ISendEmailService>();
                sendEmailServiceMock.Setup(m => m.SendAsync(accountToCreate.Email, "Confirm your account", confirmationEmail, accountToCreate.FirstName));
                sendEmailServiceMock.Setup(m => m.Status).Returns(new StatusGenericHandler());

                var confirmationEmailServiceMock = _fixture.ConfirmationEmailServiceMock;
                confirmationEmailServiceMock.Setup(m => m.ConfirmationEmail(accountToCreate.FirstName, userId, emailConfirmationCode)).Returns(confirmationEmail);

                var sut = new RegisterAccountService(userManagerMock.Object, context,
                    errorFactoryMock.Object, mapperMock.Object, sendEmailServiceMock.Object,
                    confirmationEmailServiceMock.Object);

                // Act
                await sut.RegisterAccount(accountToCreate);

                //Assert
                sendEmailServiceMock.Verify(m => m.SendAsync(accountToCreate.Email, "Confirm your account", confirmationEmail, accountToCreate.FirstName), Times.Once);

            }
        }

        [Fact]
        public async Task Should_UseEmailConfirmationService_ToCreate_AccountConfimationEmail()
        {
            var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                context.Database.EnsureCreated();

                var accountToCreate = new RegisterDto
                {
                    FirstName = "Test",
                    LastName = "User",
                    Email = "test.user@gmail.com",
                    Password = "Password123!"
                };

                var confirmationEmail = "Test Confirmation Email";
                var userId = "0001";
                var emailConfirmationCode = "1234";


                var user = _fixture.UserMock;
                user.Setup(m => m.Id).Returns(userId);
                user.Setup(m => m.Email).Returns(accountToCreate.Email);

                var store = new Mock<IUserStore<ApplicationUser>>();
                var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
                userManagerMock.Setup(m => m.CreateAsync(user.Object, "Password123!")).Returns(Task.FromResult(IdentityResult.Success));
                userManagerMock.Setup(m => m.GenerateEmailConfirmationTokenAsync(user.Object))
                    .Returns(Task.FromResult(emailConfirmationCode));

                var errorFactoryMock = _fixture.ErrorFactoryMock;

                var mapperMock = _fixture.MapperMock;
                mapperMock.Setup(m => m.Map<RegisterDto, ApplicationUser>(accountToCreate)).Returns(user.Object);

                var sendEmailServiceMock = new Mock<ISendEmailService>();
                sendEmailServiceMock.Setup(m => m.SendAsync("", "", "", ""));
                sendEmailServiceMock.Setup(m => m.Status).Returns(new StatusGenericHandler());

                var confirmationEmailServiceMock = _fixture.ConfirmationEmailServiceMock;
                confirmationEmailServiceMock.Setup(m => m.ConfirmationEmail(accountToCreate.FirstName, userId, emailConfirmationCode)).Returns(confirmationEmail);

                var sut = new RegisterAccountService(userManagerMock.Object, context,
                    errorFactoryMock.Object, mapperMock.Object, sendEmailServiceMock.Object,
                    confirmationEmailServiceMock.Object);

                // Act
                await sut.RegisterAccount(accountToCreate);

                //Assert
                confirmationEmailServiceMock.Verify(m => m.ConfirmationEmail(accountToCreate.FirstName, userId, emailConfirmationCode), Times.Once);

            }
        }

        [Fact]
        public async Task Should_HaveErrors_When_UserNotProvided()
        {
            var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                context.Database.EnsureCreated();

                var accountToCreate = new RegisterDto
                {
                    FirstName = "Test",
                    LastName = "User",
                    Email = "test.user@gmail.com",
                    Password = "Password123!"
                };

                var user = _fixture.UserMock;
                var store = new Mock<IUserStore<ApplicationUser>>();
                var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
                userManagerMock.Setup(m => m.CreateAsync(null, "Password123!")).Returns(Task.FromResult(IdentityResult.Success));

                var errorFactoryMock = _fixture.ErrorFactoryMock;

                var mapperMock = _fixture.MapperMock;
                mapperMock.Setup(m => m.Map<RegisterDto, ApplicationUser>(accountToCreate)).Returns(user.Object);

                var sendEmailServiceMock = new Mock<ISendEmailService>();
                var confirmationEmailServiceMock = _fixture.ConfirmationEmailServiceMock;

                var sut = new RegisterAccountService(userManagerMock.Object, context,
                    errorFactoryMock.Object, mapperMock.Object, sendEmailServiceMock.Object,
                    confirmationEmailServiceMock.Object);

                // Act
                await sut.RegisterAccount(accountToCreate);

                //Assert
                sut.Status.HasErrors.ShouldBeTrue();

            }
        }

        [Fact]
        public async Task Should_HaveErrors_When_PasswordNotProvided()
        {
            var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                context.Database.EnsureCreated();

                var accountToCreate = new RegisterDto
                {
                    FirstName = "Test",
                    LastName = "User",
                    Email = "test.user@gmail.com",
                    Password = "Password123!"
                };

                var user = _fixture.UserMock;
                var store = new Mock<IUserStore<ApplicationUser>>();
                var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
                userManagerMock.Setup(m => m.CreateAsync(user.Object, null)).Returns(Task.FromResult(IdentityResult.Success));

                var errorFactoryMock = _fixture.ErrorFactoryMock;

                var mapperMock = _fixture.MapperMock;
                mapperMock.Setup(m => m.Map<RegisterDto, ApplicationUser>(accountToCreate)).Returns(user.Object);

                var sendEmailServiceMock = new Mock<ISendEmailService>();
                var confirmationEmailServiceMock = _fixture.ConfirmationEmailServiceMock;

                var sut = new RegisterAccountService(userManagerMock.Object, context,
                    errorFactoryMock.Object, mapperMock.Object, sendEmailServiceMock.Object,
                    confirmationEmailServiceMock.Object);

                // Act
                await sut.RegisterAccount(accountToCreate);

                //Assert
                sut.Status.HasErrors.ShouldBeTrue();

            }
        }
    }
}
