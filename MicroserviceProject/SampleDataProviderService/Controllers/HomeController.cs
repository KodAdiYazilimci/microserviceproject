
using MicroserviceProject.Infrastructure.Communication.Model.Basics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SampleDataProviderService.Model;

namespace SampleDataProviderService.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("GetData")]
        [Authorize]
        public IActionResult GetData(int number)
        {
            ServiceResult<SampleModel> serviceResult = new ServiceResult<SampleModel>();

            serviceResult.Data = new SampleModel() { Id = 1, Name = "test" };

            return Json(serviceResult);
        }

        [HttpPost]
        [Route("PostData")]
        [Authorize]
        public IActionResult PostData([FromBody] SampleModel sampleModel)
        {
            ServiceResult<SampleModel> serviceResult = new ServiceResult<SampleModel>();

            serviceResult.Data = sampleModel;

            return Json(serviceResult);
        }
    }
}
