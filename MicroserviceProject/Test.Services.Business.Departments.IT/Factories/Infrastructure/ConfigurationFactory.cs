using Infrastructure.Mock.Data;
using Infrastructure.Mock.Providers.Configuration.Sections.AuthorizationNode;

using Microsoft.Extensions.Configuration;

namespace Test.Services.Business.Departments.IT.Factories.Infrastructure
{
    public class ConfigurationFactory
    {
        public static IConfiguration GetConfiguration()
        {
            CredentialSection authorizationCredentialSection = new CredentialSection();
            authorizationCredentialSection["email"] = "Services.Business.Departments.IT@service.service";
            authorizationCredentialSection["password"] = "1234";

            return Configuration.GetConfiguration(authorizationCredentialSection, "D:\\Logs\\Services.Business.Departments.IT\\");
        }
    }
}
