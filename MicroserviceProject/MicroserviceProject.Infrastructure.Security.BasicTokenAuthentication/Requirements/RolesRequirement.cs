using MicroserviceProject.Infrastructure.Security.BasicTokenAuthentication.Abstracts;

using Microsoft.AspNetCore.Authorization;

using System;

namespace MicroserviceProject.Infrastructure.Security.BasicTokenAuthentication.Requirements
{
    public class RolesRequirement : IAuthorizationRequirement, IIdentifiable
    {
        public RolesRequirement(string roles, Guid identifier)
        {
            Roles = roles;
            Identifier = identifier;
        }

        public string Roles { get; }

        public Guid Identifier { get; set; }
    }
}
