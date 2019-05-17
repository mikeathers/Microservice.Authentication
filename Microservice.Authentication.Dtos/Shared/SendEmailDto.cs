namespace Microservice.Authentication.Dtos.Shared
{
    public class SendEmailDto
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ToName { get; set; }
        public string FromName { get; set; }
    }
}