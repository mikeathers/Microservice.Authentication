using FluentValidation;
using Microservice.Authentication.Dtos.Account;

namespace Microservice.Authentication.Api.Validators.Account
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(m => m.Email).NotEmpty().WithMessage("You must provide an email address to register.");
            RuleFor(m => m.FirstName).NotEmpty().WithMessage("Your first name is needed for registration.");
            RuleFor(m => m.LastName).NotEmpty().WithMessage("Your last name is needed for registration.");
            RuleFor(m => m.Password).NotEmpty().WithMessage("You must provide a password to register.");
        }
    }
}
