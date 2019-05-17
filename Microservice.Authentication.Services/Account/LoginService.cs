using System;
using System.Threading.Tasks;
using Microservice.Authentication.Data.Configurations;
using Microservice.Authentication.Data.Models.User;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Interfaces.Account;
using Microservice.Authentication.Interfaces.Factories;
using Microservice.Authentication.Interfaces.Generic;
using Microsoft.AspNetCore.Identity;

namespace Microservice.Authentication.Services.Account
{
    public class LoginService : ILoginService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly IErrorFactory _errorFactory;
        private readonly ApplicationDbContext _context;
        private readonly IRetrieveAuthenticatedUserService _retrieveAuthenticatedUserService;

        public StatusGenericHandler Status { get; }

        public LoginService(UserManager<ApplicationUser> userManager, IJwtFactory jwtFactory, 
            IErrorFactory errorFactory, ApplicationDbContext context, IRetrieveAuthenticatedUserService retrieveAuthenticatedUserService)
        {
            Status = new StatusGenericHandler();
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _errorFactory = errorFactory;
            _context = context;
            _retrieveAuthenticatedUserService = retrieveAuthenticatedUserService;
        }

        public async Task<AccountDto> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginDto.Email);

                if (user == null) Status.AddError("Unable to login as this account does not exist.");
                if (Status.HasErrors) return null;

                var userSignedIn = await _userManager.CheckPasswordAsync(user, loginDto.Password);

                if (userSignedIn)
                {
                    var accountDto = await _retrieveAuthenticatedUserService.Get(user, _jwtFactory);
                    return accountDto;
                }

                Status.AddError("Username or password was incorrect.");
                return null;
            }
            catch (Exception e)
            {
                Status.AddError(e.Message);
                await _errorFactory.LogError(e.Message, "Login", _context);
                return null;
            }
        }
    }
}