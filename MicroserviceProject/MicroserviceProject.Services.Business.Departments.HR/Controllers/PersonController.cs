using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Model.Errors;
using MicroserviceProject.Services.Business.Departments.HR.Services;
using MicroserviceProject.Services.Business.Departments.HR.Util.Validation.Person.CreatePerson;
using MicroserviceProject.Services.Business.Departments.HR.Util.Validation.Person.CreateTitle;
using MicroserviceProject.Services.Business.Departments.HR.Util.Validation.Person.CreateWorker;
using MicroserviceProject.Services.Business.Model.Department.HR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.HR.Controllers
{
    [Authorize]
    [Route("Person")]
    public class PersonController : Controller
    {
        private readonly PersonService _personService;

        public PersonController(PersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        [Route(nameof(GetPeople))]
        public async Task<IActionResult> GetPeople(CancellationToken cancellationToken)
        {
            try
            {
                List<PersonModel> people =
                    await _personService.GetPeopleAsync(cancellationToken);

                ServiceResult<List<PersonModel>> serviceResult = new ServiceResult<List<PersonModel>>()
                {
                    IsSuccess = true,
                    Data = people
                };

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() }
                });
            }
        }

        [HttpPost]
        [Route(nameof(CreatePerson))]
        public async Task<IActionResult> CreatePerson(
            [FromBody] PersonModel person,
            CancellationToken cancellationToken)
        {
            try
            {
                ServiceResult validationResult =
                    await CreatePersonValidator.ValidateAsync(person, cancellationToken);

                if (!validationResult.IsSuccess)
                {
                    return BadRequest(validationResult);
                }

                int generatedId = await _personService.CreatePersonAsync(person, cancellationToken);

                ServiceResult<int> serviceResult = new ServiceResult<int>()
                {
                    IsSuccess = true,
                    Data = generatedId
                };

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() }
                });
            }
        }

        [HttpGet]
        [Route(nameof(GetTitles))]
        public async Task<IActionResult> GetTitles(CancellationToken cancellationToken)
        {
            try
            {
                List<TitleModel> titles =
                    await _personService.GetTitlesAsync(cancellationToken);

                ServiceResult<List<TitleModel>> serviceResult = new ServiceResult<List<TitleModel>>()
                {
                    IsSuccess = true,
                    Data = titles
                };

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() }
                });
            }
        }

        [HttpPost]
        [Route(nameof(CreateTitle))]
        public async Task<IActionResult> CreateTitle(
            [FromBody] TitleModel title,
            CancellationToken cancellationToken)
        {
            try
            {
                ServiceResult validationResult =
                    await CreateTitleValidator.ValidateAsync(title, cancellationToken);

                if (!validationResult.IsSuccess)
                {
                    return BadRequest(validationResult);
                }

                int generatedId = await _personService.CreateTitleAsync(title, cancellationToken);

                ServiceResult<int> serviceResult = new ServiceResult<int>()
                {
                    IsSuccess = true,
                    Data = generatedId
                };

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() }
                });
            }
        }

        [HttpGet]
        [Route(nameof(GetWorkers))]
        public async Task<IActionResult> GetWorkers(CancellationToken cancellationToken)
        {
            try
            {
                List<WorkerModel> workers =
                    await _personService.GetWorkersAsync(cancellationToken);

                ServiceResult<List<WorkerModel>> serviceResult = new ServiceResult<List<WorkerModel>>()
                {
                    IsSuccess = true,
                    Data = workers
                };

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() }
                });
            }
        }

        [HttpPost]
        [Route(nameof(CreateWorker))]
        public async Task<IActionResult> CreateWorker(
            [FromBody] WorkerModel worker,
            CancellationToken cancellationToken)
        {
            try
            {
                ServiceResult validationResult =
                    await CreateWorkerValidator.ValidateAsync(worker, cancellationToken);

                if (!validationResult.IsSuccess)
                {
                    return BadRequest(validationResult);
                }

                int generatedId = await _personService.CreateWorkerAsync(worker, cancellationToken);

                ServiceResult<int> serviceResult = new ServiceResult<int>()
                {
                    IsSuccess = true,
                    Data = generatedId
                };

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() }
                });
            }
        }
    }
}
