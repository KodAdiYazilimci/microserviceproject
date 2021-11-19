using Infrastructure.Security.Model;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Infrastructure.Security.Authentication.JWT.Providers
{
    /// <summary>
    /// JWT token için token bilgisini sağlayan sınıf
    /// </summary>
    public class TokenProvider
    {
        /// <summary>
        /// Token bilgisi detayları için configuration nesnesi
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// JWT token için yayımcı bilgisini sağlayan sınıf nesnesi
        /// </summary>
        private readonly IssuerProvider issuerProvider;

        /// <summary>
        /// JWT token için private key bilgisini sağlayan sınıf nesnesi
        /// </summary>
        private readonly SecretProvider secretProvider;

        /// <summary>
        /// JWT token için token bilgisini sağlayan sınıf
        /// </summary>
        /// <param name="configuration">Token bilgisi detayları için configuration nesnesi</param>
        /// <param name="issuerProvider">JWT token için yayımcı bilgisini sağlayan sınıf nesnesi</param>
        /// <param name="secretProvider">JWT token için private key bilgisini sağlayan sınıf nesnesi</param>
        public TokenProvider(
            IConfiguration configuration,
            IssuerProvider issuerProvider,
            SecretProvider secretProvider)
        {
            this.configuration = configuration;
            this.issuerProvider = issuerProvider;
            this.secretProvider = secretProvider;
        }

        /// <summary>
        /// JWT token verir
        /// </summary>
        /// <param name="user">JWT token alacak kullanıcının nesnesi</param>
        /// <returns></returns>
        public AuthenticationToken GetSecurityToken(AuthenticatedUser user)
        {
            DateTime expiration =
                DateTime.Now.AddMinutes(
                    Convert.ToInt32(
                        configuration
                        .GetSection("Configuration")
                        .GetSection("Authorization")
                        .GetSection("Jwt")["Expiration"]));

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(user.Claims.Select(x => new Claim(x.Name, x.Value)).ToList()),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(
                    key: new SymmetricSecurityKey(secretProvider.Bytes),
                    algorithm: SecurityAlgorithms.HmacSha256Signature),
                Issuer = issuerProvider.Issuer,
                Audience = issuerProvider.Audience,
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            string tokenKey = tokenHandler.WriteToken(token);

            return new AuthenticationToken
            {
                TokenKey = tokenKey,
                ValidTo = expiration
            };
        }
    }
}
