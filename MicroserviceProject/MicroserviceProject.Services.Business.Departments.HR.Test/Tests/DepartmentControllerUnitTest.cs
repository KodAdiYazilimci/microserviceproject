using MicroserviceProject.Services.Business.Departments.HR.Controllers;
using MicroserviceProject.Services.Business.Departments.HR.Test.Factories.Services;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.HR.Test.Tests
{
    [TestClass]
    public class DepartmentControllerUnitTest
    {
        [TestMethod]
        public async Task GetDepartmmentsTest()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            DepartmentController departmentController = new DepartmentController(DepartmentServiceFactory.Instance);

            var departments = await departmentController.GetDepartments(cancellationTokenSource);

            Assert.IsNotNull(departments);
        }
    }
}
