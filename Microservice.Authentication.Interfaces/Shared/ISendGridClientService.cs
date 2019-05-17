using System.Threading.Tasks;
using Microservice.Authentication.Interfaces.Generic;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Microservice.Authentication.Interfaces.Shared
{
    public interface ISendGridClientService
    {
        StatusGenericHandler Status { get; }
        SendGridClient Create();
        SendGridMessage CreateSingleEmail(EmailAddress fromAddress, EmailAddress toAddress, string subject, string plainTextContent, string htmlContent);
        Task<Response> SendEmailAsync(SendGridMessage message, SendGridClient client);
        EmailAddress CreateFromAddress();
        EmailAddress CreateToAddress(string toAddress, string toName);
    }
}