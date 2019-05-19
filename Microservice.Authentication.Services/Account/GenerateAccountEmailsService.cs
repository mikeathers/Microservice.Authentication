using System.Text;
using Microservice.Authentication.Interfaces.Account;

namespace Microservice.Authentication.Services.Account
{
    public class GenerateAccountEmailsService : IGenerateAccountEmailsService
    {
        public string ConfirmationEmail(string firstName, string userId, string emailConfirmationCode)
        {
            var emailUrl = $"http://localhost:58875/api/account/confirmemail?id={userId}&code={emailConfirmationCode}";
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

        public string ForgotPasswordEmail(string firstName, string userId, string passwordResetCode)
        {
            var emailUrl = $"http://localhost:58875/api/account/forgotpassword?id={userId}&code={passwordResetCode}";

            var sb = new StringBuilder();
            sb.Append($"<p>Hi {firstName}</p>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<p>Use the link below to reset your password.<p>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append($"<a href={emailUrl}>Reset Password</a>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<p>Thanks<p>");

            return sb.ToString();
        }
    }
}