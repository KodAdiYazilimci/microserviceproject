﻿using Services.Communication.Http.Broker.Department.IT.Models;

namespace Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Responses
{
    public class CreateDefaultInventoryForNewWorkerCommandResponse
    {
        public InventoryModel Inventory { get; set; }
    }
}
