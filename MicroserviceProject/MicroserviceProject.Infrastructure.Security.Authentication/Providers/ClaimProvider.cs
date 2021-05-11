using MicroserviceProject.Infrastructure.Security.Model;

using System.Collections.Generic;
using System.Security.Claims;

namespace MicroserviceProject.Infrastructure.Security.Authentication.Persistence
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
                new Claim(ClaimTypes.UserData,Newtonsoft.Json.JsonConvert.SerializeObject(user)),
                new Claim(ClaimTypes.Role,user.IsAdmin?"Admin":"User")
            };
        }
    }
}
