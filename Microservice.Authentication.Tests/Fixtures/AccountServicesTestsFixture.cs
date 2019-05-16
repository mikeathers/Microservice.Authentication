using AutoMapper;
using Microservice.Authentication.Data.Models.User;
using Microservice.Authentication.Interfaces.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Microservice.Authentication.Tests.Fixtures
{
    public class AccountServicesTestsFixture
    {
        public Mock<ApplicationUser> UserMock { get; set; }
        public Mock<IUserStore<ApplicationUser>> UserStoreMock { get; set; }
        public Mock<UserManager<ApplicationUser>> UserManagerMock { get; set; }
        public Mock<SignInManager<ApplicationUser>> SignInManagerMock { get; set; }
        public Mock<IErrorFactory> ErrorFactoryMock { get; set; }
        public Mock<IMapper> MapperMock { get; set; }
        public Mock<IJwtFactory> JwtFactoryMock { get; set; }

        public AccountServicesTestsFixture()
        {
            CreateMocks();   
        }

        private void CreateMocks()
        {

            var user = new Mock<ApplicationUser>();
            user.Object.UserName = "test";
            user.Object.Id = "000";
            user.Object.Email = "test@test.com";

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();

            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(userManagerMock.Object,
                contextAccessor.Object, userPrincipalFactory.Object, null, null, null);
            
            var errorFactoryMock = new Mock<IErrorFactory>();

            var mapperMock = new Mock<IMapper>();
            

            var jwtFactoryMock = new Mock<IJwtFactory>();
            

            UserMock = user;
            UserStoreMock = store;
            UserManagerMock = userManagerMock;
            SignInManagerMock = signInManagerMock;
            ErrorFactoryMock = errorFactoryMock;
            MapperMock = mapperMock;
            JwtFactoryMock = jwtFactoryMock;
        }
    }
}