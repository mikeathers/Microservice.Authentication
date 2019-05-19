using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microservice.Authentication.Data.Models.Config;
using Microservice.Authentication.Data.Models.User;
using Microservice.Authentication.Interfaces.Factories;
using Microservice.Authentication.Interfaces.Generic;
using Microsoft.Extensions.Options;


namespace Microservice.Authentication.Factories.JwtFactory
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptions _jwtIssuerOptions;

        public StatusGenericHandler Status { get; }

        public JwtFactory(IOptions<JwtIssuerOptions> options)
        {
            Status = new StatusGenericHandler();
            _jwtIssuerOptions = options.Value;
        }

        public async Task<string> GenerateToken(ApplicationUser user)
        {
            // ConfirmationEmail a new array of claims.
            var claims = new[]
            {
                // subject of claim, username from the client.
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                // unique id for the claim generated from _jwtOptions class (GUID).
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtIssuerOptions.JtiGenerator()),
                // issuedAt set for the claim in seconds (2 hours)
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtIssuerOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                // specify the id for the user set in the claim
                new Claim("userId", user.Id),
                // specify the name for the user set in the claim
                new Claim("firstName", user.FirstName), 
                // specify the email for the user set in the claim
                new Claim("email", user.Email) 
            };

            // ConfirmationEmail the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtIssuerOptions.Issuer,
                audience: _jwtIssuerOptions.Audience,
                claims: claims,
                notBefore: _jwtIssuerOptions.NotBefore,
                expires: _jwtIssuerOptions.Expiration,
                signingCredentials: _jwtIssuerOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() -
                                 new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
