using System.Threading.Tasks;
using Microservice.Authentication.Data.Configurations;
using Microservice.Authentication.Data.Models.User;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Services.Account;
using Microservice.Authentication.Tests.Fixtures;
using Microservice.Authentication.Tests.SeedData;
using Microsoft.AspNetCore.Identity;
using Moq;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Microservice.Authentication.Tests.UnitTests.Services.Account
{
    public class LoginServiceTests : IClassFixture<AccountServicesTestsFixture>
    {
        private readonly AccountServicesTestsFixture _fixture;

        public LoginServiceTests(AccountServicesTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_UseUserManager_ToFindUser()
        {
            var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                context.Database.EnsureCreated();

                var loginInfo = new LoginDto
                {
                    Email = "test.user@gmail.com",
                    Password = "Password123!"
                };

                var user = _fixture.UserMock;

                var store = new Mock<IUserStore<ApplicationUser>>();
                var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
                userManagerMock.Setup(m => m.FindByNameAsync(loginInfo.Email)).Returns(Task.FromResult(user.Object));

                var errorFactoryMock = _fixture.ErrorFactoryMock;

                var jwtFactoryMock = _fixture.JwtFactoryMock;
                jwtFactoryMock.Setup(m => m.GenerateRefreshToken()).Returns("9090909090");
                jwtFactoryMock.Setup(m => m.GenerateToken(user.Object)).Returns(value: Task.FromResult("930dkdkdkd"));

                var retrieveAuthenticatedUserService = _fixture.RetrieveAuthenticatedUserServiceMock;

                var sut = new LoginService(userManagerMock.Object, jwtFactoryMock.Object,
                    errorFactoryMock.Object, context, retrieveAuthenticatedUserService.Object);

                // Act
                await sut.Login(loginInfo);

                //Assert
                userManagerMock.Verify(m => m.FindByNameAsync(loginInfo.Email), Times.Once);

            }
        }

        [Fact]
        public async Task Should_UseUserManager_ToSignInUser()
        {
            var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                context.Database.EnsureCreated();
                context.SeedUserData();

                var loginInfo = new LoginDto
                {
                    Email = "test@test.com",
                    Password = "Password123!"
                };

                var user = _fixture.UserMock;
                var store = new Mock<IUserStore<ApplicationUser>>();
                var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
                userManagerMock.Setup(m => m.FindByNameAsync(loginInfo.Email)).Returns(Task.FromResult(user.Object));
                userManagerMock.Setup(m => m.CheckPasswordAsync(user.Object, loginInfo.Password))
                    .Returns(Task.FromResult(true));

                var errorFactoryMock = _fixture.ErrorFactoryMock;

                var jwtFactoryMock = _fixture.JwtFactoryMock;
                jwtFactoryMock.Setup(m => m.GenerateRefreshToken()).Returns("9090909090");
                jwtFactoryMock.Setup(m => m.GenerateToken(user.Object)).Returns(value: Task.FromResult("930dkdkdkd"));

                var retrieveAuthenticatedUserService = _fixture.RetrieveAuthenticatedUserServiceMock;

                var sut = new LoginService(userManagerMock.Object, jwtFactoryMock.Object,
                    errorFactoryMock.Object, context, retrieveAuthenticatedUserService.Object);

                // Act
                await sut.Login(loginInfo);

                //Assert
                userManagerMock.Verify(m => m.CheckPasswordAsync(user.Object, loginInfo.Password), Times.Once);

            }
        }

        [Fact]
        public async Task Should_HaveErrors_WhenUser_NotFound()
        {
            var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                context.Database.EnsureCreated();

                var loginInfo = new LoginDto
                {
                    Email = "test.user@gmail.com",
                    Password = "Password123!"
                };

                var user = _fixture.UserMock;
                var store = new Mock<IUserStore<ApplicationUser>>();
                var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
                userManagerMock.Setup(m => m.FindByNameAsync(loginInfo.Email)).Returns(Task.FromResult((ApplicationUser)null));

                var errorFactoryMock = _fixture.ErrorFactoryMock;

                var jwtFactoryMock = _fixture.JwtFactoryMock;
                jwtFactoryMock.Setup(m => m.GenerateRefreshToken()).Returns("9090909090");
                jwtFactoryMock.Setup(m => m.GenerateToken(user.Object)).Returns(value: Task.FromResult("930dkdkdkd"));

                var retrieveAuthenticatedUserService = _fixture.RetrieveAuthenticatedUserServiceMock;

                var sut = new LoginService(userManagerMock.Object, jwtFactoryMock.Object,
                    errorFactoryMock.Object, context, retrieveAuthenticatedUserService.Object);

                // Act
                await sut.Login(loginInfo);

                //Assert
                sut.Status.HasErrors.ShouldBeTrue();

            }
        }

        [Fact]
        public async Task Should_HaveErrors_WhenPassword_Incorrect()
        {
            var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                context.Database.EnsureCreated();
                context.SeedUserData();

                var loginInfo = new LoginDto
                {
                    Email = "test@test.com",
                    Password = "Password123!22222"
                };

                var user = _fixture.UserMock;
                var store = new Mock<IUserStore<ApplicationUser>>();
                var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
                userManagerMock.Setup(m => m.FindByNameAsync(loginInfo.Email)).Returns(Task.FromResult(user.Object));

                var errorFactoryMock = _fixture.ErrorFactoryMock;

                var jwtFactoryMock = _fixture.JwtFactoryMock;
                jwtFactoryMock.Setup(m => m.GenerateRefreshToken()).Returns("9090909090");
                jwtFactoryMock.Setup(m => m.GenerateToken(user.Object)).Returns(value: Task.FromResult("930dkdkdkd"));

                var retrieveAuthenticatedUserService = _fixture.RetrieveAuthenticatedUserServiceMock;

                var sut = new LoginService(userManagerMock.Object, jwtFactoryMock.Object,
                    errorFactoryMock.Object, context, retrieveAuthenticatedUserService.Object);

                // Act
                await sut.Login(loginInfo);

                //Assert
                sut.Status.HasErrors.ShouldBeTrue();

            }
        }
    }
}