using System.Text;
using Microservice.Authentication.Interfaces.Account;

namespace Microservice.Authentication.Services.Account
{
    public class ConfirmationEmailService : IConfirmationEmailService
    {
        public string Create(string firstName, string userId, string emailConfirmationCode)
        {
            var emailUrl = $"http://localhost:58875/api/account/confirmaccount/{userId}/{emailConfirmationCode}";
            var sb = new StringBuilder();
            sb.Append($"<p>Hi {firstName}</p>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<p>Please confirm your account by clicking on the link below.<p>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append($"<a href={emailUrl}>Confirm Account :)</a>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<p>Thanks<p>");

            return sb.ToString();
        }
    }
}