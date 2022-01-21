﻿using MediatR;

using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.AA.Models;

namespace Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests
{
    public class InformInventoryRequestCommandRequest : IRequest<InformInventoryRequestCommandResponse>
    {
        public InventoryRequestModel InventoryRequest { get; set; }
    }
}
