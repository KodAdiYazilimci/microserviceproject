USE [master]
GO
/****** Object:  Database [Microservice_Routing_DB]    Script Date: 6.10.2021 18:36:41 ******/
CREATE DATABASE [Microservice_Routing_DB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Microservice_Routing_DB', FILENAME = N'D:\Data Sources\MSSQL\Microservice_Routing_DB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Microservice_Routing_DB_log', FILENAME = N'D:\Data Sources\MSSQL\Microservice_Routing_DB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Microservice_Routing_DB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Microservice_Routing_DB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Microservice_Routing_DB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Microservice_Routing_DB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Microservice_Routing_DB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Microservice_Routing_DB] SET ARITHABORT OFF 
GO
ALTER DATABASE [Microservice_Routing_DB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Microservice_Routing_DB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Microservice_Routing_DB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Microservice_Routing_DB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Microservice_Routing_DB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Microservice_Routing_DB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Microservice_Routing_DB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Microservice_Routing_DB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Microservice_Routing_DB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Microservice_Routing_DB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Microservice_Routing_DB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Microservice_Routing_DB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Microservice_Routing_DB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Microservice_Routing_DB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Microservice_Routing_DB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Microservice_Routing_DB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Microservice_Routing_DB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Microservice_Routing_DB] SET RECOVERY FULL 
GO
ALTER DATABASE [Microservice_Routing_DB] SET  MULTI_USER 
GO
ALTER DATABASE [Microservice_Routing_DB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Microservice_Routing_DB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Microservice_Routing_DB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Microservice_Routing_DB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Microservice_Routing_DB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Microservice_Routing_DB', N'ON'
GO
ALTER DATABASE [Microservice_Routing_DB] SET QUERY_STORE = OFF
GO
USE [Microservice_Routing_DB]
GO
/****** Object:  Table [dbo].[SERVICE_ROUTES]    Script Date: 6.10.2021 18:36:41 ******/
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
 CONSTRAINT [PK_SERVICE_ROUTES] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SERVICE_ROUTES_ALTERNATIVES]    Script Date: 6.10.2021 18:36:41 ******/
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
/****** Object:  Table [dbo].[SERVICE_ROUTES_QUERYKEYS]    Script Date: 6.10.2021 18:36:41 ******/
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
SET IDENTITY_INSERT [dbo].[SERVICE_ROUTES] ON 

INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (3, N'authorization.auth.getuser', N'GET', N'http://localhost:16859/Auth/GetUser', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (4, N'authorization.auth.gettoken', N'POST', N'http://localhost:16859/Auth/GetToken', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1002, N'authorization.alternative.auth.getuser', N'GET', N'http://localhost/MicroserviceProject.Services.Infrastructure.Authorization/Auth/GetUser', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1003, N'authorization.alternative.auth.gettoken', N'POST', N'http://localhost/MicroserviceProject.Services.Infrastructure.Authorization/Auth/GetToken', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1004, N'hr.department.getdepartments', N'GET', N'http://localhost:26920/Department/GetDepartments', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1005, N'hr.department.createdepartment', N'POST', N'http://localhost:26920/Department/CreateDepartment', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1006, N'hr.person.getpeople', N'GET', N'http://localhost:26920/Person/GetPeople', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1007, N'hr.person.createperson', N'POST', N'http://localhost:26920/Person/CreatePerson', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1008, N'hr.person.gettitles', N'GET', N'http://localhost:26920/Person/GetTitles', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1009, N'hr.person.createtitle', N'POST', N'http://localhost:26920/Person/CreateTitle', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1010, N'hr.person.getworkers', N'GET', N'http://localhost:26920/Person/GetWorkers', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1011, N'hr.person.createworker', N'POST', N'http://localhost:26920/Person/CreateWorker', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1012, N'accounting.bankaccounts.createbankaccount', N'POST', N'http://localhost:30775/BankAccounts/CreateBankAccount', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1014, N'accounting.bankaccounts.getbankaccountsofworker', N'GET', N'http://localhost:30775/BankAccounts/GetBankAccountsOfWorker', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1015, N'accounting.bankaccounts.getcurrencies', N'GET', N'http://localhost:30775/BankAccounts/GetCurrencies', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1016, N'accounting.bankaccounts.createcurrency', N'POST', N'http://localhost:30775/BankAccounts/CreateCurrency', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1017, N'it.inventory.getinventories', N'GET', N'http://localhost:65390/Inventory/GetInventories', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1018, N'it.inventory.createinventory', N'POST', N'http://localhost:65390/Inventory/CreateInventory', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1019, N'it.inventory.assigninventorytoworker', N'POST', N'http://localhost:65390/Inventory/AssignInventoryToWorker', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1020, N'aa.inventory.getinventories', N'GET', N'http://localhost:34308/Inventory/GetInventories', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1021, N'aa.inventory.createinventory', N'POST', N'http://localhost:34308/Inventory/CreateInventory', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1022, N'aa.inventory.assigninventorytoworker', N'POST', N'http://localhost:34308/Inventory/AssignInventoryToWorker', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1023, N'aa.inventory.getinventoriesfornewworker', N'GET', N'http://localhost:34308/Inventory/GetInventoriesForNewWorker', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1024, N'it.inventory.getinventoriesfornewworker', N'GET', N'http://localhost:65390/Inventory/GetInventoriesForNewWorker', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1025, N'accounting.alternative.bankaccounts.createbankaccount', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.Accounting/BankAccounts/CreateBankAccount', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1026, N'accounting.alternative.bankaccounts.getbankaccountsofworker', N'GET', N'http://localhost/MicroserviceProject.Services.Business.Departments.Accounting/BankAccounts/GetBankAccountsOfWorker', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1027, N'accounting.alternative.bankaccounts.getcurrencies', N'GET', N'http://localhost/MicroserviceProject.Services.Business.Departments.Accounting/BankAccounts/GetCurrencies', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1028, N'accounting.alternative.bankaccounts.createcurrency', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.Accounting/BankAccounts/CreateCurrency', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1029, N'aa.alternative.inventory.getinventories', N'GET', N'http://localhost/MicroserviceProject.Services.Business.Departments.AA/Inventory/GetInventories', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1030, N'aa.alternative.inventory.createinventory', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.AA/Inventory/CreateInventory', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1031, N'aa.alternative.inventory.assigninventorytoworker', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.AA/Inventory/AssignInventoryToWorker', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1032, N'aa.alternative.inventory.getinventoriesfornewworker', N'GET', N'http://localhost/MicroserviceProject.Services.Business.Departments.AA/Inventory/GetInventoriesForNewWorker', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1033, N'hr.alternative.department.getdepartments', N'GET', N'http://localhost/MicroserviceProject.Services.Business.Departments.HR/Department/GetDepartments', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1034, N'hr.alternative.department.createdepartment', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.HR/Department/CreateDepartment', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1035, N'hr.alternative.person.getpeople', N'GET', N'http://localhost/MicroserviceProject.Services.Business.Departments.HR/Person/GetPeople', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1036, N'hr.alternative.person.createperson', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.HR/Person/CreatePerson', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1037, N'hr.alternative.person.gettitles', N'GET', N'http://localhost/MicroserviceProject.Services.Business.Departments.HR/Person/GetTitles', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1038, N'hr.alternative.person.createtitle', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.HR/Person/CreateTitle', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1039, N'hr.alternative.person.getworkers', N'GET', N'http://localhost/MicroserviceProject.Services.Business.Departments.HR/Person/GetWorkers', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1040, N'hr.alternative.person.createworker', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.HR/Person/CreateWorker', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1041, N'it.alternative.inventory.getinventories', N'GET', N'http://localhost/MicroserviceProject.Services.Business.Departments.IT/Inventory/GetInventories', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1042, N'it.alternative.inventory.createinventory', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.IT/Inventory/CreateInventory', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1043, N'it.alternative.inventory.assigninventorytoworker', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.IT/Inventory/GetInventoriesForNewWorker', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1044, N'it.alternative.inventory.getinventoriesfornewworker', N'GET', N'http://localhost/MicroserviceProject.Services.Business.Departments.IT/Inventory/GetInventoriesForNewWorker', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1045, N'it.inventory.createdefaultinventoryfornewworker', N'POST', N'http://localhost:65390/Inventory/CreateDefaultInventoryForNewWorker', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1046, N'it.alternative.inventory.createdefaultinventoryfornewworker', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.IT/Inventory/CreateDefaultInventoryForNewWorker', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1047, N'aa.inventory.createdefaultinventoryfornewworker', N'POST', N'http://localhost:34308/Inventory/CreateDefaultInventoryForNewWorker', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1048, N'aa.alternative.inventory.createdefaultinventoryfornewworker', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.AA/Inventory/CreateDefaultInventoryForNewWorker', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1049, N'hr.transaction.rollbacktransaction', N'POST', N'http://localhost:26920/Transaction/RollbackTransaction', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1050, N'hr.alternative.transaction.rollbacktransaction', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.HR/Transaction/RollbackTransaction', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1051, N'accounting.transaction.rollbacktransaction', N'POST', N'http://localhost:30775/Transaction/RollbackTransaction', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1052, N'accounting.alternative.transaction.rollbacktransaction', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.Accounting/Transaction/RollbackTransaction', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1053, N'it.transaction.rollbacktransaction', N'POST', N'http://localhost:65390/Transaction/RollbackTransaction', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1054, N'it.alternative.transaction.rollbacktransaction', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.IT/Transaction/RollbackTransaction', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1055, N'aa.transaction.rollbacktransaction', N'POST', N'http://localhost:34308/Transaction/RollbackTransaction', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (1056, N'aa.alternative.transaction.rollbacktransaction', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.AA/Transaction/RollbackTransaction', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2045, N'buying.request.createinventoryrequest', N'POST', N'http://localhost:26558/Request/CreateInventoryRequest', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2046, N'buying.request.getinventoryrequests', N'GET', N'http://localhost:26558/Request/GetInventoryRequests', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2047, N'buying.transaction.rollbacktransaction', N'POST', N'http://localhost:26558/Transaction/RollbackTransaction', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2048, N'buying.alternative.request.createinventoryrequest', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.Buying/Request/CreateInventoryRequest', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2049, N'buying.alternative.request.getinventoryrequests', N'GET', N'http://localhost/MicroserviceProject.Services.Business.Departments.Buying/Request/GetInventoryRequests', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2050, N'buying.alternative.transaction.rollbacktransaction', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.Buying/Transaction/RollbackTransaction', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2051, N'finance.cost.createcost', N'POST', N'http://localhost:32355/Cost/CreateCost', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2052, N'finance.cost.getdecidedcosts', N'GET', N'http://localhost:32355/Cost/GetDecidedCosts', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2053, N'finance.cost.decidecost', N'POST', N'http://localhost:32355/Cost/DecideCost', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2054, N'buying.request.validatecostinventory', N'POST', N'http://localhost:26558/Request/ValidateCostInventory', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2055, N'aa.inventory.informinventoryrequest', N'POST', N'http://localhost:34308/Inventory/InformInventoryRequest', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2056, N'it.inventory.informinventoryrequest', N'POST', N'http://localhost:65390/Inventory/InformInventoryRequest', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2057, N'finance.alternative.cost.createcost', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.Finance/Cost/CreateCost', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2058, N'finance.alternative.cost.getdecidedcosts', N'GET', N'http://localhost/MicroserviceProject.Services.Business.Departments.Finance/Cost/GetDecidedCosts', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2059, N'finance.alternative.cost.decidecost', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.Finance/Cost/DecideCost', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2060, N'buying.alternative.request.validatecostinventory', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.Buying/Request/ValidateCostInventory', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2061, N'aa.alternative.inventory.informinventoryrequest', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.AA/Inventory/InformInventoryRequest', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2062, N'it.alternative.inventory.informinventoryrequest', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.IT/Inventory/InformInventoryRequest', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2063, N'logging.logging.writerequestresponselog', N'POST', N'http://localhost:15455/Logging/WriteRequestResponseLog', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2065, N'websockets.security.sendtokennotification', N'POST', N'http://localhost:43449/SendTokenNotification', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2066, N'websockets.reliability.senderrornotification', N'POST', N'http://localhost:23681/SendErrorNotification', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2067, N'accounting.bankaccounts.getsalarypaymentsofworker', N'GET', N'http://localhost:30775/BankAccounts/GetSalaryPaymentsOfWorker', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2068, N'accounting.alternative.bankaccounts.getsalarypaymentsofworker', N'GET', N'http://localhost/MicroserviceProject.Services.Business.Departments.Accounting/BankAccounts/GetSalaryPaymentsOfWorker', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2069, N'accounting.bankaccounts.createsalarypayment', N'POST', N'http://localhost:30775/BankAccounts/CreateSalaryPayment', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2070, N'accounting.alternative.bankaccounts.createsalarypayment', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.Accounting/BankAccounts/CreateSalaryPayment', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2071, N'cr.customers.getcustomers', N'GET', N'http://localhost:60403/Customers/GetCustomers', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2072, N'cr.customers.createcustomer', N'POST', N'http://localhost:60403/Customers/CreateCustomer', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2073, N'cr.alternative.customers.getcustomers', N'GET', N'http://localhost/MicroserviceProject.Services.Business.Departments.CR/Customers/GetCustomers', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2074, N'cr.alternative.customers.createcustomer', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.CR/Customers/CreateCustomer', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2075, N'selling.selling.getsolds', N'GET', N'http://localhost:5139/Selling/GetSolds', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2076, N'selling.selling.createselling', N'POST', N'http://localhost:5139/Selling/CreateSelling', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2077, N'selling.alternative.selling.getsolds', N'GET', N'http://localhost/MicroserviceProject.Services.Business.Departments.Selling/Selling/GetSolds', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2078, N'selling.alternative.selling.createselling', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.Selling/Selling/CreateSelling', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2079, N'production.product.getproducts', N'GET', N'http://localhost:9311/Product/GetProducts', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2080, N'production.product.createproduct', N'POST', N'http://localhost:9311/Product/CreateProduct', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2081, N'production.alternative.product.getproducts', N'GET', N'http://localhost/MicroserviceProject.Services.Business.Departments.Production/Product/GetProducts', NULL)
INSERT [dbo].[SERVICE_ROUTES] ([ID], [NAME], [CALLTYPE], [ENDPOINT], [DELETE_DATE]) VALUES (2082, N'production.alternative.product.createproduct', N'POST', N'http://localhost/MicroserviceProject.Services.Business.Departments.Production/Product/CreateProduct', NULL)
SET IDENTITY_INSERT [dbo].[SERVICE_ROUTES] OFF
GO
SET IDENTITY_INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ON 

INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (1, 1020, 1029, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (2, 1012, 1025, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (3, 1014, 1026, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (4, 1015, 1027, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (5, 1016, 1028, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (6, 1021, 1030, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (7, 1022, 1031, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (8, 1023, 1032, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (9, 3, 1002, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (10, 4, 1003, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (11, 1004, 1033, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (12, 1005, 1034, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (13, 1006, 1035, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (14, 1007, 1036, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (15, 1008, 1037, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (16, 1009, 1038, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (17, 1010, 1039, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (18, 1011, 1040, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (19, 1017, 1041, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (20, 1018, 1042, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (21, 1019, 1043, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (22, 1020, 1029, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (23, 1024, 1044, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (24, 1045, 1046, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (25, 1047, 1048, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (26, 1049, 1050, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (27, 1051, 1052, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (28, 1053, 1054, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (29, 1055, 1056, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (1024, 2045, 2048, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (1025, 2046, 2049, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (1026, 2047, 2050, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (1027, 2067, 2068, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (1028, 2069, 2070, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (1029, 2071, 2073, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (1030, 2072, 2074, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (1031, 2075, 2077, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (1032, 2076, 2078, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (1033, 2079, 2081, NULL)
INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] ([ID], [SERVICE_ROUTES_ID], [ALTERNATIVE_SERVICE_ROUTE_ID], [DELETE_DATE]) VALUES (1034, 2080, 2082, NULL)
SET IDENTITY_INSERT [dbo].[SERVICE_ROUTES_ALTERNATIVES] OFF
GO
SET IDENTITY_INSERT [dbo].[SERVICE_ROUTES_QUERYKEYS] ON 

INSERT [dbo].[SERVICE_ROUTES_QUERYKEYS] ([ID], [SERVICE_ROUTE_ID], [KEY], [DELETE_DATE]) VALUES (2, 3, N'token', NULL)
INSERT [dbo].[SERVICE_ROUTES_QUERYKEYS] ([ID], [SERVICE_ROUTE_ID], [KEY], [DELETE_DATE]) VALUES (1002, 1002, N'token', NULL)
INSERT [dbo].[SERVICE_ROUTES_QUERYKEYS] ([ID], [SERVICE_ROUTE_ID], [KEY], [DELETE_DATE]) VALUES (1003, 1014, N'workerId', NULL)
INSERT [dbo].[SERVICE_ROUTES_QUERYKEYS] ([ID], [SERVICE_ROUTE_ID], [KEY], [DELETE_DATE]) VALUES (1005, 1026, N'workerId', NULL)
INSERT [dbo].[SERVICE_ROUTES_QUERYKEYS] ([ID], [SERVICE_ROUTE_ID], [KEY], [DELETE_DATE]) VALUES (1006, 2067, N'workerId', NULL)
INSERT [dbo].[SERVICE_ROUTES_QUERYKEYS] ([ID], [SERVICE_ROUTE_ID], [KEY], [DELETE_DATE]) VALUES (1007, 2068, N'workerId', NULL)
SET IDENTITY_INSERT [dbo].[SERVICE_ROUTES_QUERYKEYS] OFF
GO
ALTER TABLE [dbo].[SERVICE_ROUTES_QUERYKEYS]  WITH CHECK ADD  CONSTRAINT [FK_SERVICE_ROUTES_QUERYKEYS_SERVICE_ROUTES] FOREIGN KEY([SERVICE_ROUTE_ID])
REFERENCES [dbo].[SERVICE_ROUTES] ([ID])
GO
ALTER TABLE [dbo].[SERVICE_ROUTES_QUERYKEYS] CHECK CONSTRAINT [FK_SERVICE_ROUTES_QUERYKEYS_SERVICE_ROUTES]
GO
USE [master]
GO
ALTER DATABASE [Microservice_Routing_DB] SET  READ_WRITE 
GO
