﻿using MediatR;

using Services.Api.Business.Departments.Accounting.Services;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Responses;
using Services.Communication.Http.Broker.Department.Accounting.Models;
using Services.Logging.Aspect.Handlers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Accounting.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQueryRequest, GetCurrenciesQueryResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly BankService _bankService;

        public GetCurrenciesQueryHandler(
            RuntimeHandler runtimeHandler,
            BankService bankService)
        {
            _runtimeHandler = runtimeHandler;
            _bankService = bankService;
        }

        public async Task<GetCurrenciesQueryResponse> Handle(GetCurrenciesQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetCurrenciesQueryResponse()
            {
                Currencies =
                await
                _runtimeHandler.ExecuteResultMethod<Task<List<CurrencyModel>>>(
                    _bankService,
                    nameof(_bankService.GetCurrenciesAsync),
                    new object[] { new CancellationTokenSource() })
            };
        }
    }
}