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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IErrorFactory _errorFactory;
        private readonly IMapper _mapper;
        private readonly IJwtFactory _jwtFactory;

        public StatusGenericHandler Status { get; }

        public RegisterAccountService(UserManager<ApplicationUser> userManager, 
            ApplicationDbContext context, IErrorFactory errorFactory, IMapper mapper, IJwtFactory jwtFactory)
        {
            Status = new StatusGenericHandler();
            _userManager = userManager;
            _context = context;
            _errorFactory = errorFactory;
            _mapper = mapper;
            _jwtFactory = jwtFactory;
        }

        public async Task<AccountDto> RegisterAccount(RegisterDto registerDto)
        {
            using (var scope = _context.Database.BeginTransaction())
            {
                try
                {
                    var user = _mapper.Map<RegisterDto, ApplicationUser>(registerDto);
                    var userCreated = await _userManager.CreateAsync(user, registerDto.Password);

                    if (userCreated.Succeeded)
                    {
                        var token = await _jwtFactory.GenerateToken(user);
                        var refreshToken = _jwtFactory.GenerateRefreshToken();
                        user.UpdateRefreshToken(refreshToken);

                        var accountInfo = new AccountDto()
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            PictureUrl = user.PictureUrl,
                            IsAuthenticated = true,
                            RefreshToken = refreshToken,
                            Token = token
                        };

                        await _context.SaveChangesAsync();
                        scope.Commit();

                        return accountInfo;
                    }

                    foreach (var error in userCreated.Errors)
                    {
                        Status.AddError(error.Description);
                    }
                    
                    return null;
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
