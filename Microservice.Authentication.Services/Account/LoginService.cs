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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly IErrorFactory _errorFactory;
        private readonly ApplicationDbContext _context;

        public StatusGenericHandler Status { get; }

        public LoginService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IJwtFactory jwtFactory, 
            IErrorFactory errorFactory, ApplicationDbContext context)
        {
            Status = new StatusGenericHandler();
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _errorFactory = errorFactory;
            _context = context;
        }

        public async Task<AccountDto> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginDto.Email);

                if (user == null) Status.AddError("Unable to login as this account does not exist.");
                if (Status.HasErrors) return null;

                var userSignedIn = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);

                if (userSignedIn.Succeeded)
                {
                    var accountDto = await RetrieveUserAccount.Get(user, _jwtFactory);
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