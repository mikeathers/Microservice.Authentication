using System;
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
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }

        public void UpdateRefreshToken(string refreshToken)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiry = DateTime.Now.AddDays(10);
        }
    }
}
