using System.Threading.Tasks;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Interfaces.Generic;

namespace Microservice.Authentication.Interfaces.Account
{
    public interface IResetPasswordService
    {
        StatusGenericHandler Status { get; }
        Task ResetPassword(ResetPasswordDto resetPasswordDto);
    }
}