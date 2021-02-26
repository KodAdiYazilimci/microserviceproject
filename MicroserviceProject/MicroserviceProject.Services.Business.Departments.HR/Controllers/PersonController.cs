using MicroserviceProject.Services.Business.Departments.HR.Services;
using MicroserviceProject.Services.Business.Departments.HR.Util.Validation.Person.CreatePerson;
using MicroserviceProject.Services.Business.Departments.HR.Util.Validation.Person.CreateTitle;
using MicroserviceProject.Services.Business.Departments.HR.Util.Validation.Person.CreateWorker;
using MicroserviceProject.Services.Model.Department.HR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            if (Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _personService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

            return await ServiceExecuter.ExecuteServiceAsync<List<PersonModel>>(async () =>
            {
                return await _personService.GetPeopleAsync(cancellationToken);
            },
            services: _personService);
        }

        [HttpPost]
        [Route(nameof(CreatePerson))]
        public async Task<IActionResult> CreatePerson([FromBody] PersonModel person, CancellationToken cancellationToken)
        {
            if (Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _personService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

            return await ServiceExecuter.ExecuteServiceAsync<int>(async () =>
            {
                await CreatePersonValidator.ValidateAsync(person, cancellationToken);

                return await _personService.CreatePersonAsync(person, cancellationToken);
            },
            services: _personService);
        }

        [HttpGet]
        [Route(nameof(GetTitles))]
        public async Task<IActionResult> GetTitles(CancellationToken cancellationToken)
        {
            if (Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _personService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

            return await ServiceExecuter.ExecuteServiceAsync<List<TitleModel>>(async () =>
            {
                return await _personService.GetTitlesAsync(cancellationToken);
            },
            services: _personService);
        }

        [HttpPost]
        [Route(nameof(CreateTitle))]
        public async Task<IActionResult> CreateTitle([FromBody] TitleModel title, CancellationToken cancellationToken)
        {
            if (Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _personService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

            return await ServiceExecuter.ExecuteServiceAsync<int>(async () =>
            {
                await CreateTitleValidator.ValidateAsync(title, cancellationToken);

                return await _personService.CreateTitleAsync(title, cancellationToken);
            },
            services: _personService);
        }

        [HttpGet]
        [Route(nameof(GetWorkers))]
        public async Task<IActionResult> GetWorkers(CancellationToken cancellationToken)
        {
            if (Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _personService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

            return await ServiceExecuter.ExecuteServiceAsync<List<WorkerModel>>(async () =>
            {
                return await _personService.GetWorkersAsync(cancellationToken);
            },
            services: _personService);
        }

        [HttpPost]
        [Route(nameof(CreateWorker))]
        public async Task<IActionResult> CreateWorker([FromBody] WorkerModel worker, CancellationToken cancellationToken)
        {
            if (Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _personService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

            return await ServiceExecuter.ExecuteServiceAsync<int>(async () =>
            {
                await CreateWorkerValidator.ValidateAsync(worker, cancellationToken);

                return await _personService.CreateWorkerAsync(worker, cancellationToken);
            },
            services: _personService);
        }
    }
}
