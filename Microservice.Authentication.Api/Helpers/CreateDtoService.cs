using Microservice.Authentication.Api.Helpers.Interfaces;
using Microservice.Authentication.Dtos.Account;

namespace Microservice.Authentication.Api.Helpers
{
    public class CreateDtoService : ICreateDtoService
    {
        public ConfirmEmailDto ConfirmEmailDto(string userId, string emailConfirmationToken)
        {
            return new ConfirmEmailDto
            {
                EmailConfirmationToken = emailConfirmationToken,
                UserId = userId
            };
        }
    }
}