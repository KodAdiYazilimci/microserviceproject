using Infrastructure.Security.Authentication.BasicToken.Abstracts;

using Microsoft.AspNetCore.Authorization;

using System;
using System.Linq;

namespace Infrastructure.Security.Authentication.BasicToken.Util
{
    /// <summary>
    /// Kimlik gereksinimlerinin karşılandığını denetleyen sınıf
    /// </summary>
    public static class StateHandler
    {
        /// <summary>
        /// Kimlik gereksiniminin başarılı olduğunu doğrular
        /// </summary>
        /// <param name="context"></param>
        /// <param name="identifier"></param>
        public static void Succeed(AuthorizationHandlerContext context, Guid identifier)
        {
            var groupedRequirements = context.Requirements.Where(r => (r as IIdentifiable)?.Identifier == identifier);

            foreach (var requirement in groupedRequirements)
            {
                context.Succeed(requirement);
            }
        }
    }
}
