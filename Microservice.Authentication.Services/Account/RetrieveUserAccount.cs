using System.Threading.Tasks;
using Microservice.Authentication.Data.Models.User;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Interfaces.Factories;

namespace Microservice.Authentication.Services.Account
{
    public static class RetrieveUserAccount
    {
        
        public async static Task<AccountDto> Get(ApplicationUser user, IJwtFactory jwtFactory)
        {
            var token = await jwtFactory.GenerateToken(user);
            var refreshToken = jwtFactory.GenerateRefreshToken();
            user.UpdateRefreshToken(refreshToken);

            var accountInfo = new AccountDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PictureUrl = user.PictureUrl,
                IsAuthenticated = true,
                RefreshToken = refreshToken,
                Token = token
            };

            return accountInfo;
        }
    }
}