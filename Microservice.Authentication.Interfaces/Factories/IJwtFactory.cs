using System.Threading.Tasks;
using Microservice.Authentication.Data.Models.User;

namespace Microservice.Authentication.Interfaces.Factories
{
    public interface IJwtFactory
    {
        Task<string> GenerateToken(ApplicationUser user);
        string GenerateRefreshToken();
    }
}