using System.Threading.Tasks;
using Microservice.Authentication.Interfaces.Generic;

namespace Microservice.Authentication.Interfaces.Account
{
    public interface IConfirmEmailService
    {
        StatusGenericHandler Status { get; }
        Task<bool> ConfirmEmail(string userId, string emailConfirmationToken);
    }
}