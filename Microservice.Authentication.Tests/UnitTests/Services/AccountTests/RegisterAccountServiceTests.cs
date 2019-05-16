﻿using System.Threading.Tasks;
using Microservice.Authentication.Data.Configurations;
using Microservice.Authentication.Data.Models.User;
using Microservice.Authentication.Dtos.Account;
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
                var store = _fixture.UserStoreMock;

                var userManagerMock = _fixture.UserManagerMock;
                userManagerMock.Setup(m => m.CreateAsync(user.Object, "Password123!")).Returns(Task.FromResult(IdentityResult.Success));

                var errorFactoryMock = _fixture.ErrorFactoryMock;

                var mapperMock = _fixture.MapperMock;
                mapperMock.Setup(m => m.Map<RegisterDto, ApplicationUser>(accountToCreate)).Returns(user.Object);

                var jwtFactoryMock = _fixture.JwtFactoryMock;
                jwtFactoryMock.Setup(m => m.GenerateRefreshToken()).Returns("9090909090");
                jwtFactoryMock.Setup(m => m.GenerateToken(user.Object)).Returns(value: Task.FromResult("930dkdkdkd"));

                var sut = new RegisterAccountService(userManagerMock.Object, context, errorFactoryMock.Object, mapperMock.Object, jwtFactoryMock.Object); 

                // Act
                await sut.RegisterAccount(accountToCreate);

                //Assert
                userManagerMock.Verify(m => m.CreateAsync(user.Object, "Password123!"), Times.Once);

            }
        }

        [Fact]
        public async Task Should_ReturnAccountDto_OnCompletion()
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
                var store = _fixture.UserStoreMock;

                var userManagerMock = _fixture.UserManagerMock;
                userManagerMock.Setup(m => m.CreateAsync(user.Object, "Password123!")).Returns(Task.FromResult(IdentityResult.Success));

                var errorFactoryMock = _fixture.ErrorFactoryMock;

                var mapperMock = _fixture.MapperMock;
                mapperMock.Setup(m => m.Map<RegisterDto, ApplicationUser>(accountToCreate)).Returns(user.Object);

                var jwtFactoryMock = _fixture.JwtFactoryMock;
                jwtFactoryMock.Setup(m => m.GenerateRefreshToken()).Returns("9090909090");
                jwtFactoryMock.Setup(m => m.GenerateToken(user.Object)).Returns(value: Task.FromResult("930dkdkdkd"));

                var sut = new RegisterAccountService(userManagerMock.Object, context, errorFactoryMock.Object, mapperMock.Object, jwtFactoryMock.Object);

                // Act
                var accountDto = await sut.RegisterAccount(accountToCreate);

                //Assert
                accountDto.ShouldNotBeNull();

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
                var store = _fixture.UserStoreMock;

                var userManagerMock = _fixture.UserManagerMock;
                userManagerMock.Setup(m => m.CreateAsync(null, "Password123!")).Returns(Task.FromResult(IdentityResult.Success));

                var errorFactoryMock = _fixture.ErrorFactoryMock;

                var mapperMock = _fixture.MapperMock;
                mapperMock.Setup(m => m.Map<RegisterDto, ApplicationUser>(accountToCreate)).Returns(user.Object);

                var jwtFactoryMock = _fixture.JwtFactoryMock;
                jwtFactoryMock.Setup(m => m.GenerateRefreshToken()).Returns("9090909090");
                jwtFactoryMock.Setup(m => m.GenerateToken(user.Object)).Returns(value: Task.FromResult("930dkdkdkd"));

                var sut = new RegisterAccountService(userManagerMock.Object, context, errorFactoryMock.Object, mapperMock.Object, jwtFactoryMock.Object);

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
                var store = _fixture.UserStoreMock;

                var userManagerMock = _fixture.UserManagerMock;
                userManagerMock.Setup(m => m.CreateAsync(user.Object, null)).Returns(Task.FromResult(IdentityResult.Success));

                var errorFactoryMock = _fixture.ErrorFactoryMock;

                var mapperMock = _fixture.MapperMock;
                mapperMock.Setup(m => m.Map<RegisterDto, ApplicationUser>(accountToCreate)).Returns(user.Object);

                var jwtFactoryMock = _fixture.JwtFactoryMock;
                jwtFactoryMock.Setup(m => m.GenerateRefreshToken()).Returns("9090909090");
                jwtFactoryMock.Setup(m => m.GenerateToken(user.Object)).Returns(value: Task.FromResult("930dkdkdkd"));

                var sut = new RegisterAccountService(userManagerMock.Object, context, errorFactoryMock.Object, mapperMock.Object, jwtFactoryMock.Object);

                // Act
                await sut.RegisterAccount(accountToCreate);

                //Assert
                sut.Status.HasErrors.ShouldBeTrue();

            }
        }
    }
}
