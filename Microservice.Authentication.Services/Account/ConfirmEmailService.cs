using System;
using System.Threading.Tasks;
using Microservice.Authentication.Data.Models.User;
using Microservice.Authentication.Interfaces.Account;
using Microservice.Authentication.Interfaces.Generic;
using Microsoft.AspNetCore.Identity;

namespace Microservice.Authentication.Services.Account
{
    public class ConfirmEmailService : IConfirmEmailService
    {
        public StatusGenericHandler Status { get; }
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmEmailService(UserManager<ApplicationUser> userManager)
        {
            Status = new StatusGenericHandler();
            _userManager = userManager;
        }

        public async Task<bool> ConfirmEmail(string userId, string emailConfirmationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(userId)) Status.AddError("A 'UserId' has not been provided.");
                if (string.IsNullOrEmpty(emailConfirmationToken))
                    Status.AddError("A 'EmailConfirmationToken' has not been provided.");
                if (Status.HasErrors) return false;

                var userToConfirm = await _userManager.FindByIdAsync(userId);
                var emailConfirm = await _userManager.ConfirmEmailAsync(userToConfirm, emailConfirmationToken);

                if (emailConfirm.Succeeded)
                {
                    return true;
                }

                foreach (var error in emailConfirm.Errors)
                {
                    Status.AddError(error.Description);
                }

                return false;
            }
            catch (Exception e)
            {
                Status.AddError(e.Message);
                return false;
            }
        }
    }
}