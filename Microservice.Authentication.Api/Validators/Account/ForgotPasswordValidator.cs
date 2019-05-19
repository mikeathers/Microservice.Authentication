using FluentValidation;
using Microservice.Authentication.Dtos.Account;

namespace Microservice.Authentication.Api.Validators.Account
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(m => m.Email).NotEmpty().WithMessage("An 'email address' has not been provided.");
        }
    }
}