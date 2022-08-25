using Infrastructure.Mock.Data;
using Infrastructure.Mock.Providers.Configuration.Sections.AuthorizationNode;
using Infrastructure.Mock.Providers.Configuration.Sections.PersistenceNode;

using Microsoft.Extensions.Configuration;

using System.Collections.Generic;

namespace Test.Services.Api.Business.Departments.IT.Factories.Infrastructure
{
    public class ConfigurationFactory
    {
        public static IConfiguration GetConfiguration()
        {
            CredentialSection authorizationCredentialSection = new CredentialSection();
            authorizationCredentialSection["email"] = "Services.Api.Business.Departments.IT@service.service";
            authorizationCredentialSection["password"] = "1234";

            return Configuration.GetConfiguration(
                authorizationCredential: authorizationCredentialSection,
                loggingFilePath: "C:\\Logs\\Services.Api.Business.Departments.IT\\",
                databaseSections: new List<AnyDatabaseSection>()
                {
                    new AnyDatabaseSection("Microservice_IT_DB","server=localhost;DataBase=Microservice_IT_DB;user=sa;password=Srkn_CMR*1987;MultipleActiveResultSets=true")
                });
        }
    }
}
