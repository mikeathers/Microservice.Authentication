using System.Threading.Tasks;
using Microservice.Authentication.Interfaces.Generic;

namespace Microservice.Authentication.Interfaces.Shared
{
    public interface ISendEmailService
    {
        StatusGenericHandler Status { get; }
        Task SendAsync(string email, string subject, string body, string toName = null);
    }
}
