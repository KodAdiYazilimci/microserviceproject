using Services.Communication.Http.Broker.Department.HR.Models;

using Test.Services.Api.Gateway.Public;

namespace Test.Services.Api.Gateway
{
    public class DataProvider
    {
        private HumanResourcesControllerTest humanResourcesControllerTest;

        public DataProvider(HumanResourcesControllerTest humanResourcesControllerTest)
        {
            this.humanResourcesControllerTest = humanResourcesControllerTest;
        }

        public async Task<List<DepartmentModel>> GetDepartmentsAsync()
        {
            return await humanResourcesControllerTest.GetDepartmentsAsync();
        }
    }
}
