using Microservice.Authentication.Dtos.Account;

namespace Microservice.Authentication.Api.Helpers.Interfaces
{
    public interface ICreateDtoService
    {
        ConfirmEmailDto ConfirmEmailDto(string userId, string emailConfirmationToken);
    }
}
