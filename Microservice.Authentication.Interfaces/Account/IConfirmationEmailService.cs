using System.Threading.Tasks;
using Microservice.Authentication.Interfaces.Generic;

namespace Microservice.Authentication.Interfaces.Account
{
    public interface IConfirmationEmailService
    {
        string Create(string firstName, string userId, string emailConfirmationCode);

    }
}