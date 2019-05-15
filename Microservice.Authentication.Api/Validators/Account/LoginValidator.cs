using FluentValidation;
using Microservice.Authentication.Dtos.Account;

namespace Microservice.Authentication.Api.Validators.Account
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(m => m.Email).NotEmpty().WithMessage("An email address has not been provided.");
            RuleFor(m => m.Password).NotEmpty().WithMessage("A password has not been provided.");
        }
    }
}