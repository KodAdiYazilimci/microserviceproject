﻿using MicroserviceProject.Model.Communication.Basics;

using Microsoft.AspNetCore.Mvc;

using SampleDataProviderService.Model;

namespace SampleDataProviderService.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("GetData")]
        public IActionResult GetData()
        {
            ServiceResult<SampleModel> serviceResult = new ServiceResult<SampleModel>();

            serviceResult.Data = new SampleModel() { Id = 1, Name = "test" };

            return Json(serviceResult);
        }

        [HttpPost]
        [Route("PostData")]
        public IActionResult PostData([FromBody] SampleModel sampleModel)
        {
            ServiceResult<SampleModel> serviceResult = new ServiceResult<SampleModel>();

            serviceResult.Data = sampleModel;

            return Json(serviceResult);
        }
    }
}