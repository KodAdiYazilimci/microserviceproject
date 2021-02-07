using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Model.Errors;
using MicroserviceProject.Services.Business.Departments.HR.Services;
using MicroserviceProject.Services.Business.Departments.HR.Util.Validation.Person.CreatePerson;
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
    }
}
