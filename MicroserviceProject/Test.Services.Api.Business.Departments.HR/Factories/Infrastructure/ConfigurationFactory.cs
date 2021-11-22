using Infrastructure.Mock.Data;
using Infrastructure.Mock.Providers.Configuration.Sections.AuthorizationNode;

using Microsoft.Extensions.Configuration;

namespace Test.Services.Api.Business.Departments.HR.Factories.Infrastructure
{
    public class ConfigurationFactory
    {
        public static IConfiguration GetConfiguration()
        {
            CredentialSection authorizationCredentialSection = new CredentialSection();
            authorizationCredentialSection["email"] = "Services.Api.Business.Departments.HR@service.service";
            authorizationCredentialSection["password"] = "1234";

            return Configuration.GetConfiguration(authorizationCredentialSection, "C:\\Logs\\Services.Api.Business.Departments.HR\\");
        }
    }
}
