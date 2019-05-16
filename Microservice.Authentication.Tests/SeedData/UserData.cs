using System.Collections.Generic;
using Microservice.Authentication.Data.Configurations;
using Microservice.Authentication.Data.Models.User;

namespace Microservice.Authentication.Tests.SeedData
{
    public static class UserData
    {
        public static void SeedUserData(this ApplicationDbContext context)
        {
            context.Users.AddRange(Users());
        }

        public static List<ApplicationUser> Users()
        {
            return new List<ApplicationUser>
            {
                new ApplicationUser()
                {
                    Email = "test@test.com"
                }
            };

        }
    }
}