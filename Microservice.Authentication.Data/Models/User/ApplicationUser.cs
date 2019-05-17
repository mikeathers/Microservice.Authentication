using System;
using Microsoft.AspNetCore.Identity;

namespace Microservice.Authentication.Data.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string FacebookId { get; set; }
        public virtual string GoogleId { get; set; }
        public virtual string PictureUrl { get; set; }
        public virtual string RefreshToken { get; set; }
        public virtual DateTime RefreshTokenExpiry { get; set; }

        public void UpdateRefreshToken(string refreshToken)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiry = DateTime.Now.AddDays(10);
        }
    }
}
