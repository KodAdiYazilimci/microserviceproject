using MicroserviceProject.Model.Security;

using System.Collections.Generic;
using System.Security.Claims;

namespace MicroserviceProject.Infrastructure.Security.BasicTokenAuthentication.Persistence
{
    public class AuthenticationPersistence
    {
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
