using System;
using System.Threading.Tasks;
using AutoMapper;
using Microservice.Authentication.Data.Configurations;
using Microservice.Authentication.Data.Models.User;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Interfaces.Account;
using Microservice.Authentication.Interfaces.Factories;
using Microservice.Authentication.Interfaces.Generic;
using Microsoft.AspNetCore.Identity;

namespace Microservice.Authentication.Services.Account
{
    public class RegisterAccountService : IRegisterAccountService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IErrorFactory _errorFactory;
        private readonly IMapper _mapper;

        public StatusGenericHandler Status { get; }

        public RegisterAccountService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IErrorFactory errorFactory, IMapper mapper)
        {
            Status = new StatusGenericHandler();
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _errorFactory = errorFactory;
            _mapper = mapper;
        }

        public async Task<AccountDto> RegisterAccount(RegisterDto registerDto)
        {
            using (var scope = _context.Database.BeginTransaction())
            {
                try
                {
                    var userToCreate = _mapper.Map<RegisterDto, ApplicationUser>(registerDto);
                    var userCreated = await _userManager.CreateAsync(userToCreate, registerDto.Password);
                }
                catch (Exception e)
                {
                    Status.AddError(e.Message);
                    scope.Rollback();
                    await _errorFactory.LogError(e.Message, "RegisterAccount", _context);
                    return null;
                }
            }
        }
    }
}
