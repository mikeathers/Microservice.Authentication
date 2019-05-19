using System;
using System.Threading.Tasks;
using Microservice.Authentication.Data.Models.User;
using Microservice.Authentication.Dtos.Account;
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

        public async Task ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {
            try
            {
                var userToConfirm = await _userManager.FindByIdAsync(confirmEmailDto.UserId);
                if (userToConfirm == null) Status.AddError("A user could not be found with the 'user id' provided");
                if (Status.HasErrors) return;

                var emailConfirm = await _userManager.ConfirmEmailAsync(userToConfirm, confirmEmailDto.EmailConfirmationToken);

                if (emailConfirm.Succeeded) return;

                Status.AddError("Email confirmation failed.");

                foreach (var error in emailConfirm.Errors)
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