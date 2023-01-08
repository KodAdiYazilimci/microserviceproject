using Infrastructure.Mock.Data;
using Infrastructure.Mock.Providers.Configuration.Sections.AuthorizationNode;
using Infrastructure.Mock.Providers.Configuration.Sections.PersistenceNode;

using Microsoft.Extensions.Configuration;

using System.Collections.Generic;

namespace Test.Services.Api.Business.Departments.HR.Factories.Infrastructure
{
    public class ConfigurationFactory
    {
        public static IConfiguration GetConfiguration()
        {
            CredentialSection authorizationCredentialSection = new CredentialSection();
            authorizationCredentialSection["email"] = "Services.Api.Business.Departments.HR@service.service";
            authorizationCredentialSection["password"] = "1234";

            return Configuration.GetConfiguration(
                authorizationCredential: authorizationCredentialSection,
                loggingAbsoluteFilePath: "C:\\Logs\\Services.Api.Business.Departments.HR\\",
                loggingRelativeFilePath: "RequestResponseLogs/",
                databaseSections: new List<AnyDatabaseSection>()
                {
                    new AnyDatabaseSection("Microservice_HR_DB","server=localhost;DataBase=Microservice_AA_DB;user=sa;password=Srkn_CMR*1987;MultipleActiveResultSets=true;TrustServerCertificate=Yes")
                });
        }
    }
}
