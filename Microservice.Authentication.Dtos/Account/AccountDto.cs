namespace Microservice.Authentication.Dtos.Account
{
    public class AccountDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PictureUrl { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}