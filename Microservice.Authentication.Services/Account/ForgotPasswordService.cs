using System;
using System.Threading.Tasks;
using System.Web;
using Microservice.Authentication.Data.Models.User;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Interfaces.Account;
using Microservice.Authentication.Interfaces.Generic;
using Microservice.Authentication.Interfaces.Shared;
using Microsoft.AspNetCore.Identity;

namespace Microservice.Authentication.Services.Account
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenerateAccountEmailsService _accountEmailsService;
        private readonly ISendEmailService _sendEmailService;

        public StatusGenericHandler Status { get; }
        
        public ForgotPasswordService(UserManager<ApplicationUser> userManager, IGenerateAccountEmailsService accountEmailsService,
            ISendEmailService sendEmailService)
        {
            Status = new StatusGenericHandler();
            _userManager = userManager;
            _accountEmailsService = accountEmailsService;
            _sendEmailService = sendEmailService;
        }

        public async Task RequestPasswordReset(ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
                if (user == null) Status.AddError("A user was not found with the 'email address' provided");
                if (Status.HasErrors) return;

                var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                passwordResetToken = HttpUtility.UrlEncode(passwordResetToken);

                var forgotPasswordEmail =
                    _accountEmailsService.ForgotPasswordEmail(user.FirstName, user.Id,
                        passwordResetToken);

                await _sendEmailService.SendAsync(user.Email, "Password Reset", forgotPasswordEmail, user.FirstName);

                if (_sendEmailService.Status.HasErrors) Status.CombineErrors(_sendEmailService.Status);
            }
            catch (Exception e)
            {
                Status.AddError(e.Message);
            }
        }
    }
}