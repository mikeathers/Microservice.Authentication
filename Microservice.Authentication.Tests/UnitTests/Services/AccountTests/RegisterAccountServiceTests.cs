using System;
using System.Threading.Tasks;
using AutoMapper;
using Microservice.Authentication.Data.Configurations;
using Microservice.Authentication.Data.Models.User;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Interfaces.Factories;
using Microservice.Authentication.Services.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TestSupport.EfHelpers;
using Xunit;

namespace Microservice.Authentication.Tests.UnitTests.Services.Account
{
    public class RegisterAccountServiceTests 
    {
        [Fact]
        public async Task Should_RegisterAccount_WhenRequiredInfo_IsProvided()
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


                

                var userManagerMock = new FakeUserManager();
                var signInManagerMock = new FakeSignInManager();

                var errorFactoryMock = new Mock<IErrorFactory>();
                

                var user = new Mock<ApplicationUser>();
                user.Object.UserName = "test";
                user.Object.Id = "000";
                user.Object.Email = "test@test.com";

                var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<RegisterDto, ApplicationUser>(accountToCreate)).Returns(user.Object);




                var jwtFactoryMock = new Mock<IJwtFactory>();
                jwtFactoryMock.Setup(m => m.GenerateRefreshToken()).Returns("9090909090");
                jwtFactoryMock.Setup(m => m.GenerateToken(user.Object)).Returns(Task.FromResult("930dkdkdkd"));

                

                var sut = new RegisterAccountService(signInManagerMock, userManagerMock, context, errorFactoryMock.Object, mapperMock.Object, jwtFactoryMock.Object); 

                // Act
                await sut.RegisterAccount(accountToCreate);

                //Assert
                

            }
        }
    }
}

public class FakeSignInManager : SignInManager<ApplicationUser>
{
    public FakeSignInManager()
        : base(new Mock<FakeUserManager>().Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object)
    { }
}

public class FakeUserManager : UserManager<ApplicationUser>
{
    public FakeUserManager()
        : base(new Mock<IUserStore<ApplicationUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<ApplicationUser>>().Object,
            new IUserValidator<ApplicationUser>[0],
            new IPasswordValidator<ApplicationUser>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<ApplicationUser>>>().Object)
    { }

    public override Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
    {
        return Task.FromResult(IdentityResult.Success);
    }

    public override Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
    {
        return Task.FromResult(IdentityResult.Success);
    }

    public override Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
    {
        return Task.FromResult(Guid.NewGuid().ToString());
    }

}