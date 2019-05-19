using System;
using System.Threading.Tasks;
using Microservice.Authentication.Data.Models.User;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Interfaces.Account;
using Microservice.Authentication.Interfaces.Generic;
using Microsoft.AspNetCore.Identity;

namespace Microservice.Authentication.Services.Account
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public StatusGenericHandler Status { get; }
        
        public ResetPasswordService(UserManager<ApplicationUser> userManager)
        {
            Status = new StatusGenericHandler();
            _userManager = userManager;

        }

        public async Task ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
                if (user == null) Status.AddError("A user was not found with the 'email address' provided.");

                var passwordReset = await _userManager.ResetPasswordAsync(user, resetPasswordDto.PasswordResetToken,
                    resetPasswordDto.Password);

                if (passwordReset.Succeeded) return;

                Status.AddError("Password reset failed.");

                foreach (var error in passwordReset.Errors)
                {
                    Status.AddError(error.Description);
                }

            }
            catch (Exception e)
            {
                Status.AddError(e.Message);
            }
        }
    }
}