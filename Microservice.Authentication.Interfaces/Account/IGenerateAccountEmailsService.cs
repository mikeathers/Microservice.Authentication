namespace Microservice.Authentication.Interfaces.Account
{
    public interface IGenerateAccountEmailsService
    {
        string ConfirmationEmail(string firstName, string userId, string emailConfirmationCode);
        string ForgotPasswordEmail(string firstName, string userId, string passwordResetCode);

    }
}