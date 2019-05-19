using FluentValidation;
using Microservice.Authentication.Dtos.Account;

namespace Microservice.Authentication.Api.Validators.Account
{
    public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailDto>
    {
        public ConfirmEmailValidator()
        {
            RuleFor(m => m.UserId).NotEmpty().WithMessage("A 'user id' has not been provided.");
            RuleFor(m => m.EmailConfirmationToken).NotEmpty()
                .WithMessage("A 'email confirmation token' has not been provided.");
        }
    }
}