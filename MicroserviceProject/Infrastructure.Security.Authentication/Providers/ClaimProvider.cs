
using Infrastructure.Security.Model;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Security.Claims;

namespace Infrastructure.Security.Authentication.Persistence
{
    /// <summary>
    /// Kimlik bilgisi sağlayan sınıf
    /// </summary>
    public class ClaimProvider
    {
        /// <summary>
        /// Kimlik bilgilerini verir
        /// </summary>
        /// <param name="user">Kimlik bilgisi getirilecek kullanıcı</param>
        /// <returns></returns>
        public static IEnumerable<Claim> GetClaims(User user)
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.UserData,JsonConvert.SerializeObject(user)),
                new Claim(ClaimTypes.Role,user.IsAdmin?"Admin":"User")
            };
        }
    }
}
