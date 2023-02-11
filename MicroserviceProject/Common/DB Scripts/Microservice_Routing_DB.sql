USE [Microservice_Routing_DB]
GO
/****** Object:  Table [dbo].[HOSTS]    Script Date: 16.08.2022 11:17:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HOSTS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NAME] [nvarchar](250) NULL,
	[HOST] [nvarchar](1000) NULL,
	[DELETE_DATE] [datetime] NULL,
 CONSTRAINT [PK_HOSTS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SERVICE_ROUTES]    Script Date: 16.08.2022 11:17:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SERVICE_ROUTES](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NAME] [nvarchar](250) NULL,
	[CALLTYPE] [nvarchar](50) NULL,
	[ENDPOINT] [nvarchar](500) NULL,
	[DELETE_DATE] [datetime] NULL,
	[ENABLED] [bit] NULL,
	[ROUTE_TYPE] [int] NULL,
 CONSTRAINT [PK_SERVICE_ROUTES] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SERVICE_ROUTES_ALTERNATIVES]    Script Date: 16.08.2022 11:17:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SERVICE_ROUTES_ALTERNATIVES](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SERVICE_ROUTES_ID] [int] NULL,
	[ALTERNATIVE_SERVICE_ROUTE_ID] [int] NULL,
	[DELETE_DATE] [datetime] NULL,
 CONSTRAINT [PK_SERVICE_ROUTES_ALTERNATIVES] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SERVICE_ROUTES_QUERYKEYS]    Script Date: 16.08.2022 11:17:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SERVICE_ROUTES_QUERYKEYS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SERVICE_ROUTE_ID] [int] NULL,
	[KEY] [nvarchar](50) NULL,
	[DELETE_DATE] [datetime] NULL,
 CONSTRAINT [PK_SERVICE_ROUTE_QUERYKEYS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT INTO Microservice_Routing_DB.dbo.SERVICE_ROUTES (NAME,CALLTYPE,ENDPOINT,DELETE_DATE,ENABLED,ROUTE_TYPE) VALUES
	 (N'authorization.auth.getuser',N'GET',N'http://authorization-service.default.svc.cluster.local:5100/Auth/GetUser',NULL,1,1),
	 (N'authorization.auth.gettoken',N'POST',N'http://authorization-service.default.svc.cluster.local:5100/Auth/GetToken',NULL,1,1),
	 (N'hr.department.getdepartments',N'GET',N'http://hr-service.default.svc.cluster.local:5102/Department/GetDepartments',NULL,0,1),
	 (N'hr.department.createdepartment',N'POST',N'http://hr-service.default.svc.cluster.local:5102/Department/CreateDepartment',NULL,0,1),
	 (N'hr.person.getpeople',N'GET',N'http://hr-service.default.svc.cluster.local:5102/Person/GetPeople',NULL,0,1),
	 (N'hr.person.createperson',N'POST',N'http://hr-service.default.svc.cluster.local:5102/Person/CreatePerson',NULL,0,1),
	 (N'hr.person.gettitles',N'GET',N'http://hr-service.default.svc.cluster.local:5102/Person/GetTitles',NULL,0,1),
	 (N'hr.person.createtitle',N'POST',N'http://hr-service.default.svc.cluster.local:5102/Person/CreateTitle',NULL,0,1),
	 (N'hr.person.getworkers',N'GET',N'http://hr-service.default.svc.cluster.local:5102/Person/GetWorkers',NULL,0,1),
	 (N'hr.person.createworker',N'POST',N'http://hr-service.default.svc.cluster.local:5102/Person/CreateWorker',NULL,0,1);
INSERT INTO Microservice_Routing_DB.dbo.SERVICE_ROUTES (NAME,CALLTYPE,ENDPOINT,DELETE_DATE,ENABLED,ROUTE_TYPE) VALUES
	 (N'accounting.bankaccounts.createbankaccount',N'POST',N'http://accounting-service.default.svc.cluster.local:5103/BankAccounts/CreateBankAccount',NULL,0,1),
	 (N'accounting.bankaccounts.getbankaccountsofworker',N'GET',N'http://accounting-service.default.svc.cluster.local:5103/BankAccounts/GetBankAccountsOfWorker',NULL,0,1),
	 (N'accounting.bankaccounts.getcurrencies',N'GET',N'http://accounting-service.default.svc.cluster.local:5103/BankAccounts/GetCurrencies',NULL,0,1),
	 (N'accounting.bankaccounts.createcurrency',N'POST',N'http://accounting-service.default.svc.cluster.local:5103/BankAccounts/CreateCurrency',NULL,0,1),
	 (N'it.inventory.getinventories',N'GET',N'http://it-service.default.svc.cluster.local:5108/Inventory/GetInventories',NULL,0,1),
	 (N'it.inventory.createinventory',N'POST',N'http://it-service.default.svc.cluster.local:5108/Inventory/CreateInventory',NULL,0,1),
	 (N'it.inventory.assigninventorytoworker',N'POST',N'http://it-service.default.svc.cluster.local:5108/Inventory/AssignInventoryToWorker',NULL,0,1),
	 (N'aa.inventory.getinventories',N'GET',N'http://aa-service.default.svc.cluster.local:5104/Inventory/GetInventories',NULL,0,1),
	 (N'aa.inventory.createinventory',N'POST',N'http://aa-service.default.svc.cluster.local:5104/Inventory/CreateInventory',NULL,0,1),
	 (N'aa.inventory.assigninventorytoworker',N'POST',N'http://aa-service.default.svc.cluster.local:5104/Inventory/AssignInventoryToWorker',NULL,0,1);
INSERT INTO Microservice_Routing_DB.dbo.SERVICE_ROUTES (NAME,CALLTYPE,ENDPOINT,DELETE_DATE,ENABLED,ROUTE_TYPE) VALUES
	 (N'aa.inventory.getinventoriesfornewworker',N'GET',N'http://aa-service.default.svc.cluster.local:5104/Inventory/GetInventoriesForNewWorker',NULL,0,1),
	 (N'it.inventory.getinventoriesfornewworker',N'GET',N'http://it-service.default.svc.cluster.local:5108/Inventory/GetInventoriesForNewWorker',NULL,0,1),
	 (N'it.inventory.createdefaultinventoryfornewworker',N'POST',N'http://it-service.default.svc.cluster.local:5108/Inventory/CreateDefaultInventoryForNewWorker',NULL,0,1),
	 (N'aa.inventory.createdefaultinventoryfornewworker',N'POST',N'http://aa-service.default.svc.cluster.local:5104/Inventory/CreateDefaultInventoryForNewWorker',NULL,0,1),
	 (N'hr.transaction.rollbacktransaction',N'POST',N'http://hr-service.default.svc.cluster.local:5102/Transaction/RollbackTransaction',NULL,0,1),
	 (N'accounting.transaction.rollbacktransaction',N'POST',N'http://accounting-service.default.svc.cluster.local:5103/Transaction/RollbackTransaction',NULL,0,1),
	 (N'it.transaction.rollbacktransaction',N'POST',N'http://it-service.default.svc.cluster.local:5108/Transaction/RollbackTransaction',NULL,0,1),
	 (N'aa.transaction.rollbacktransaction',N'POST',N'http://aa-service.default.svc.cluster.local:5104/Transaction/RollbackTransaction',NULL,0,1),
	 (N'buying.request.createinventoryrequest',N'POST',N'http://buying-service.default.svc.cluster.local:5105/Request/CreateInventoryRequest',NULL,0,1),
	 (N'buying.request.getinventoryrequests',N'GET',N'http://buying-service.default.svc.cluster.local:5105/Request/GetInventoryRequests',NULL,0,1);
INSERT INTO Microservice_Routing_DB.dbo.SERVICE_ROUTES (NAME,CALLTYPE,ENDPOINT,DELETE_DATE,ENABLED,ROUTE_TYPE) VALUES
	 (N'buying.transaction.rollbacktransaction',N'POST',N'http://buying-service.default.svc.cluster.local:5105/Transaction/RollbackTransaction',NULL,0,1),
	 (N'finance.cost.createcost',N'POST',N'http://finance-service.default.svc.cluster.local:5107/Cost/CreateCost',NULL,0,1),
	 (N'finance.cost.getdecidedcosts',N'GET',N'http://finance-service.default.svc.cluster.local:5107/Cost/GetDecidedCosts',NULL,0,1),
	 (N'finance.cost.decidecost',N'POST',N'http://finance-service.default.svc.cluster.local:5107/Cost/DecideCost',NULL,0,1),
	 (N'buying.request.validatecostinventory',N'POST',N'http://buying-service.default.svc.cluster.local:5105/Request/ValidateCostInventory',NULL,0,1),
	 (N'aa.inventory.informinventoryrequest',N'POST',N'http://aa-service.default.svc.cluster.local:5104/Inventory/InformInventoryRequest',NULL,0,1),
	 (N'it.inventory.informinventoryrequest',N'POST',N'http://it-service.default.svc.cluster.local:5108/Inventory/InformInventoryRequest',NULL,0,1),
	 (N'logging.logging.writerequestresponselog',N'POST',N'http://logging-service.default.svc.cluster.local:5101/Logging/WriteRequestResponseLog',NULL,0,1),
	 (N'accounting.bankaccounts.getsalarypaymentsofworker',N'GET',N'http://accounting-service.default.svc.cluster.local:5103/BankAccounts/GetSalaryPaymentsOfWorker',NULL,0,1),
	 (N'accounting.bankaccounts.createsalarypayment',N'POST',N'http://accounting-service.default.svc.cluster.local:5103/BankAccounts/CreateSalaryPayment',NULL,0,1);
INSERT INTO Microservice_Routing_DB.dbo.SERVICE_ROUTES (NAME,CALLTYPE,ENDPOINT,DELETE_DATE,ENABLED,ROUTE_TYPE) VALUES
	 (N'cr.customers.getcustomers',N'GET',N'http://cr-service.default.svc.cluster.local:5106/Customers/GetCustomers',NULL,0,1),
	 (N'cr.customers.createcustomer',N'POST',N'http://cr-service.default.svc.cluster.local:5106/Customers/CreateCustomer',NULL,0,1),
	 (N'selling.selling.getsolds',N'GET',N'http://selling-service.default.svc.cluster.local:5110/Selling/GetSolds',NULL,0,1),
	 (N'selling.selling.createselling',N'POST',N'http://selling-service.default.svc.cluster.local:5110/Selling/CreateSelling',NULL,0,1),
	 (N'production.product.getproducts',N'GET',N'http://production-service.default.svc.cluster.local:5109/Product/GetProducts',NULL,0,1),
	 (N'production.product.createproduct',N'POST',N'http://production-service.default.svc.cluster.local:5109/Product/CreateProduct',NULL,0,1),
	 (N'storage.stock.getstock',N'GET',N'http://storage-service.default.svc.cluster.local:5111/Stock/GetStock',NULL,0,1),
	 (N'storage.stock.createstock',N'POST',N'http://storage-service.default.svc.cluster.local:5111/Stock/CreateStock',NULL,0,1),
	 (N'production.production.produceproduct',N'POST',N'http://production-service.default.svc.cluster.local:5109/Production/ProduceProduct',NULL,0,1),
	 (N'storage.stock.descendproductstock',N'POST',N'http://storage-service.default.svc.cluster.local:5111/Stock/DescendProductStock',NULL,0,1);
INSERT INTO Microservice_Routing_DB.dbo.SERVICE_ROUTES (NAME,CALLTYPE,ENDPOINT,DELETE_DATE,ENABLED,ROUTE_TYPE) VALUES
	 (N'production.production.reevaluateproduceproduct',N'GET',N'http://production-service.default.svc.cluster.local:5109/Production/ReEvaluateProduceProduct',NULL,0,1),
	 (N'finance.productionrequest.getproductionrequests',N'GET',N'http://finance-service.default.svc.cluster.local:5107/ProductionRequest/GetProductionRequests',NULL,0,1),
	 (N'finance.productionrequest.createproductionrequest',N'POST',N'http://finance-service.default.svc.cluster.local:5107/ProductionRequest/CreateProductionRequest',NULL,0,1),
	 (N'finance.productionrequest.decideproductionrequest',N'POST',N'http://finance-service.default.svc.cluster.local:5107/ProductionRequest/DecideProductionRequest',NULL,0,1),
	 (N'selling.selling.notifyproductionrequest',N'POST',N'http://selling-service.default.svc.cluster.local:5110/Selling/NotifyProductionRequest',NULL,0,1),
	 (N'accounting.identity.removesessionifexistsincache',N'GET',N'http://accounting-service.default.svc.cluster.local:5103/Identity/RemoveSessionIfExistsInCache',NULL,0,1),
	 (N'hr.identity.removesessionifexistsincache',N'GET',N'http://hr-service.default.svc.cluster.local:5102/Identity/RemoveSessionIfExistsInCache',NULL,0,1),
	 (N'it.identity.removesessionifexistsincache',N'GET',N'http://it-service.default.svc.cluster.local:5108/Identity/RemoveSessionIfExistsInCache',NULL,0,1),
	 (N'aa.identity.removesessionifexistsincache',N'GET',N'http://aa-service.default.svc.cluster.local:5104/Identity/RemoveSessionIfExistsInCache',NULL,0,1),
	 (N'buying.identity.removesessionifexistsincache',N'GET',N'http://buying-service.default.svc.cluster.local:5105/Identity/RemoveSessionIfExistsInCache',NULL,0,1);
INSERT INTO Microservice_Routing_DB.dbo.SERVICE_ROUTES (NAME,CALLTYPE,ENDPOINT,DELETE_DATE,ENABLED,ROUTE_TYPE) VALUES
	 (N'finance.identity.removesessionifexistsincache',N'GET',N'http://finance-service.default.svc.cluster.local:5107/Identity/RemoveSessionIfExistsInCache',NULL,0,1),
	 (N'cr.identity.removesessionifexistsincache',N'GET',N'http://cr-service.default.svc.cluster.local:5106/Identity/RemoveSessionIfExistsInCache',NULL,0,1),
	 (N'selling.identity.removesessionifexistsincache',N'GET',N'http://selling-service.default.svc.cluster.local:5110/Identity/RemoveSessionIfExistsInCache',NULL,0,1),
	 (N'production.identity.removesessionifexistsincache',N'GET',N'http://production-service.default.svc.cluster.local:5109/Identity/RemoveSessionIfExistsInCache',NULL,0,1),
	 (N'storage.identity.removesessionifexistsincache',N'GET',N'http://storage-service.default.svc.cluster.local:5111/Identity/RemoveSessionIfExistsInCache',NULL,0,1),
	 (N'presentation.ui.web.identity.user.login',N'GET',N'http://192.168.1.102:31098/Login',NULL,1,1),
	 (N'gateway.public.hr.getdepartments',N'GET',N'http://agwpublic-service.default.svc.cluster.local:5099/HR/GetDepartments',NULL,1,1);
GO
ALTER TABLE [dbo].[SERVICE_ROUTES_QUERYKEYS]  WITH CHECK ADD  CONSTRAINT [FK_SERVICE_ROUTES_QUERYKEYS_SERVICE_ROUTES] FOREIGN KEY([SERVICE_ROUTE_ID])
REFERENCES [dbo].[SERVICE_ROUTES] ([ID])
GO
ALTER TABLE [dbo].[SERVICE_ROUTES_QUERYKEYS] CHECK CONSTRAINT [FK_SERVICE_ROUTES_QUERYKEYS_SERVICE_ROUTES]
GO
