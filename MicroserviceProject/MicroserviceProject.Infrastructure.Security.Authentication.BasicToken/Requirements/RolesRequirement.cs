
using MicroserviceProject.Infrastructure.Security.Authentication.BasicToken.Abstracts;

using Microsoft.AspNetCore.Authorization;

using System;

namespace MicroserviceProject.Infrastructure.Security.Authentication.BasicToken.Requirements
{
    /// <summary>
    /// Kimliğe ait rol gereksinimini tanımlayan sınıf
    /// </summary>
    public class RolesRequirement : IAuthorizationRequirement, IIdentifiable
    {
        /// <summary>
        /// Kimliğe ait rol gereksinimini tanımlayan sınıf
        /// </summary>
        /// <param name="roles">Gerekli roller</param>
        /// <param name="identifier">Kimlik tanımayıcı</param>
        public RolesRequirement(string roles, Guid identifier)
        {
            Roles = roles;
            Identifier = identifier;
        }

        /// <summary>
        /// Gerekli roller
        /// </summary>
        public string Roles { get; }

        /// <summary>
        /// Kimlik tanımayıcı
        /// </summary>
        public Guid Identifier { get; set; }
    }
}
