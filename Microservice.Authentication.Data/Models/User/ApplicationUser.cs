using Microsoft.AspNetCore.Identity;

namespace Microservice.Authentication.Data.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FacebookId { get; set; }
        public string GoogleId { get; set; }
        public string PictureUrl { get; set; }

    }
}
