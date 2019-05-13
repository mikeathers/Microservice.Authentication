using System.Threading.Tasks;
using Microservice.Authentication.Data.Configurations;
using Microservice.Authentication.Data.Models.Shared;
using Microservice.Authentication.Interfaces.Factories;

namespace Microservice.Authentication.Factories.ErrorFactory
{
    public class ErrorFactory : IErrorFactory
    {
        public async Task LogError(string errorMessage, string functionName, ApplicationDbContext context)
        {
            using (var scope = context.Database.BeginTransaction())
            {
                try
                {
                    var errorToLog = new Error(errorMessage, functionName);
                    await context.Errors.AddAsync(errorToLog);
                    await context.SaveChangesAsync();
                    scope.Commit();
                }
                catch
                {
                    scope.Rollback();
                }
            }
        }
    }
}
