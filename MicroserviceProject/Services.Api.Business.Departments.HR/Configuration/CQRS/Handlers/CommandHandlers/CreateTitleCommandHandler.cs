﻿using MediatR;

using Services.Api.Business.Departments.HR.Services;
using Services.Api.Business.Departments.HR.Util.Validation.Person.CreateTitle;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateTitleCommandHandler : IRequestHandler<CreateTitleCommandRequest, CreateTitleCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly PersonService _personService;

        public CreateTitleCommandHandler(
            RuntimeHandler runtimeHandler,
            PersonService personService)
        {
            _runtimeHandler = runtimeHandler;
            _personService = personService;
        }

        public async Task<CreateTitleCommandResponse> Handle(CreateTitleCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateTitleValidator.ValidateAsync(request.Title, cancellationTokenSource);

            return new CreateTitleCommandResponse()
            {
                CreatedTitleId =
                await
                _runtimeHandler.ExecuteResultMethod<Task<int>>(
                    _personService,
                    nameof(_personService.CreateTitleAsync),
                    new object[] { request.Title, cancellationTokenSource })
            };
        }
    }
}