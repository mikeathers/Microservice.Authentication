namespace Microservice.Authentication.Dtos.Account
{
    public class ConfirmEmailDto
    {
        public string UserId { get; set; }
        public string EmailConfirmationToken { get; set; }
    }
}