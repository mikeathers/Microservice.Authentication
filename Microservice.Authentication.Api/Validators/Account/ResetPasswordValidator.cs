using FluentValidation;
using Microservice.Authentication.Dtos.Account;

namespace Microservice.Authentication.Api.Validators.Account
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordValidator()
        {
            RuleFor(m => m.Email).NotEmpty().WithMessage("An 'email address' has not been provided.");
            RuleFor(m => m.Password).NotEmpty().WithMessage("A new 'password' has not been provided.");
            RuleFor(m => m.PasswordResetToken).NotEmpty()
                .WithMessage("A 'password reset token' has not been provided.");
        }
    }
}