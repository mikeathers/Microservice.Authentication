using System;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microservice.Authentication.Data.Configurations;
using Microservice.Authentication.Data.Models.User;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Interfaces.Account;
using Microservice.Authentication.Interfaces.Factories;
using Microservice.Authentication.Interfaces.Generic;
using Microservice.Authentication.Interfaces.Shared;
using Microsoft.AspNetCore.Identity;

namespace Microservice.Authentication.Services.Account
{
    public class RegisterAccountService : IRegisterAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IErrorFactory _errorFactory;
        private readonly IMapper _mapper;
        private readonly ISendEmailService _sendEmailService;
        private readonly IGenerateAccountEmailsService _confimationEmailService;

        public StatusGenericHandler Status { get; }

        public RegisterAccountService(UserManager<ApplicationUser> userManager, 
            ApplicationDbContext context, IErrorFactory errorFactory, IMapper mapper, 
            ISendEmailService sendEmailService, IGenerateAccountEmailsService confirmationEmailService)
        {
            Status = new StatusGenericHandler();
            _userManager = userManager;
            _context = context;
            _errorFactory = errorFactory;
            _mapper = mapper;
            _sendEmailService = sendEmailService;
            _confimationEmailService = confirmationEmailService;
        }

        public async Task RegisterAccount(RegisterDto registerDto)
        {
            using (var scope = _context.Database.BeginTransaction())
            {
                try
                {
                    var user = _mapper.Map<RegisterDto, ApplicationUser>(registerDto);
                    var userCreated = await _userManager.CreateAsync(user, registerDto.Password);

                    if (userCreated.Succeeded)
                    {
                        var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        emailConfirmationToken = HttpUtility.UrlEncode(emailConfirmationToken);

                        var confirmationEmail =
                            _confimationEmailService.ConfirmationEmail(registerDto.FirstName, user.Id, emailConfirmationToken);

                        await _sendEmailService.SendAsync(user.Email, "Confirm your account", confirmationEmail,
                            registerDto.FirstName);

                        if (_sendEmailService.Status.HasErrors) Status.CombineErrors(_sendEmailService.Status);
                        if (Status.HasErrors) return;

                        await _context.SaveChangesAsync();
                        scope.Commit();
                    }

                    foreach (var error in userCreated.Errors)
                    {
                        Status.AddError(error.Description);
                    }
                    
                }
                catch (Exception e)
                {
                    Status.AddError(e.Message);
                    scope.Rollback();
                    await _errorFactory.LogError(e.Message, "RegisterAccount", _context);
                }
            }
        }
    }
}
