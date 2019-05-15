using System.Threading.Tasks;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Interfaces.Generic;

namespace Microservice.Authentication.Interfaces.Account
{
    public interface ILoginService
    {
        StatusGenericHandler Status { get; }
        Task<AccountDto> Login(LoginDto loginDto);
    }
}