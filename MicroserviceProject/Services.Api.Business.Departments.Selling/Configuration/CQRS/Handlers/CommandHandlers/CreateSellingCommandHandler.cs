using MediatR;

using Services.Api.Business.Departments.Selling.Services;
using Services.Api.Business.Departments.Selling.Util.Validation.Selling;
using Services.Communication.Http.Broker.Department.Selling.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Selling.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Selling.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateSellingCommandHandler : IRequestHandler<CreateSellingCommandRequest, CreateSellingCommandResponse>
    {
        private readonly SellingService _sellingService;

        public CreateSellingCommandHandler(SellingService sellingService)
        {
            _sellingService = sellingService;
        }

        public async Task<CreateSellingCommandResponse> Handle(CreateSellingCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateSellingValidator.ValidateAsync(request.Selling, cancellationTokenSource);

            return new CreateSellingCommandResponse()
            {
                CreatedSellingId = await _sellingService.CreateSellingAsync(request.Selling, cancellationTokenSource)
            };
        }
    }
}
