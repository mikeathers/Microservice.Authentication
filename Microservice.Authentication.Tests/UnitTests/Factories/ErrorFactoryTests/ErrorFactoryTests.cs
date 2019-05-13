using System.Linq;
using System.Threading.Tasks;
using Microservice.Authentication.Data.Configurations;
using Microservice.Authentication.Factories.ErrorFactory;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Microservice.Authentication.Tests.UnitTests.Factories.ErrorFactoryTests
{
    public class ErrorFactoryTests
    {
        [Fact]
        public async Task Should_LogAnError_WhenRequiredInfo_IsProvided()
        {
            var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                context.Database.EnsureCreated();
                var errorFactory = new ErrorFactory();
                var currentErrors = context.Errors.Count();

                // Act
                await errorFactory.LogError("Error Occured", "Test Function", context);

                //Assert
                context.Errors.Count().ShouldEqual(currentErrors + 1);
            }
        }
    }
}
