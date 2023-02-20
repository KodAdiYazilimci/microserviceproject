using Microsoft.VisualStudio.TestTools.UnitTesting;

using Services.Communication.Http.Broker.Department.HR.Models;

using Test.Services.Api.Gateway.Public.Factories.Infrastructure;

namespace Test.Services.Api.Gateway.Public
{
    [TestClass]
    public class HumanResourcesControllerUnitTest
    {
        private HumanResourcesControllerTest humanResourcesControllerTest;
        private DataProvider dataProvider;

        [TestInitialize]
        public void Init()
        {
            humanResourcesControllerTest = new HumanResourcesControllerTest(ConfigurationFactory.GetConfiguration());
            dataProvider = new DataProvider(humanResourcesControllerTest);
        }

        [TestMethod]
        public async Task GetDepartmentsTest()
        {
            List<DepartmentModel> departments = await dataProvider.GetDepartmentsAsync();

            Assert.IsTrue(departments != null && departments.Any());
        }

        [TestCleanup]
        public void CleanUp()
        {
            humanResourcesControllerTest = null;
        }
    }
}
