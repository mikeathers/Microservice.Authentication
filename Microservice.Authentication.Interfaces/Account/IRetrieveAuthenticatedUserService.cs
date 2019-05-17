using System.Threading.Tasks;
using Microservice.Authentication.Data.Models.User;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Interfaces.Factories;

namespace Microservice.Authentication.Interfaces.Account
{
    public interface IRetrieveAuthenticatedUserService
    {
        Task<AccountDto> Get(ApplicationUser user, IJwtFactory jwtFactory);
    }
}