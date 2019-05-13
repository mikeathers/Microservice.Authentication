using System.Collections.Generic;
using Microservice.Authentication.Data.Configurations;
using Microservice.Authentication.Data.Models.Shared;

namespace Microservice.Authentication.Tests.SeedData
{
    public static class ErrorData
    {
        public static void SeedErrorData(this ApplicationDbContext context)
        {
            context.Errors.AddRange(Errors());
        }

        public static List<Error> Errors()
        {
            return new List<Error>
            {
                new Error("Test Error 1", "Test Function 1"),
                new Error("Test Error 3", "Test Function 2"),
            };
            
        }
    }
}
