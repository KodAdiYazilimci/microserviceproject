﻿using MediatR;

using Services.Api.Business.Departments.Storage.Services;
using Services.Api.Business.Departments.Storage.Util.Validation.Stock;
using Services.Communication.Http.Broker.Department.Storage.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Storage.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Storage.Configuration.CQRS.Handlers.CommandHandlers
{
    public class DescendProductStockCommandHandler : IRequestHandler<DescendProductStockCommandRequest, DescendProductStockCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly StockService _stockService;
        private readonly DescendStockValidator _descendStockValidator;

        public DescendProductStockCommandHandler(
            RuntimeHandler runtimeHandler,
            StockService stockService,
            DescendStockValidator descendStockValidator)
        {
            _runtimeHandler = runtimeHandler;
            _stockService = stockService;
            _descendStockValidator = descendStockValidator;
        }

        public async Task<DescendProductStockCommandResponse> Handle(DescendProductStockCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await _descendStockValidator.ValidateAsync(request.Stock, cancellationTokenSource);

            return new DescendProductStockCommandResponse()
            {
                StockId =
                await _runtimeHandler.ExecuteResultMethod<Task<int>>(
                    _stockService,
                    nameof(_stockService.DescendProductStockAsync),
                    new object[] { request.Stock, cancellationTokenSource })
            };
        }
    }
}
