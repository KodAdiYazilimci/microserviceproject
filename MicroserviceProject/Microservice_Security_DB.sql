USE [master]
GO
/****** Object:  Database [Microservice_Security_DB]    Script Date: 19.11.2021 15:43:19 ******/
CREATE DATABASE [Microservice_Security_DB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Microservice_Security_DB', FILENAME = N'C:\Data Sources\MSSQL\Microservice_Security_DB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Microservice_Security_DB_log', FILENAME = N'C:\Data Sources\MSSQL\Microservice_Security_DB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Microservice_Security_DB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Microservice_Security_DB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Microservice_Security_DB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Microservice_Security_DB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Microservice_Security_DB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Microservice_Security_DB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Microservice_Security_DB] SET ARITHABORT OFF 
GO
ALTER DATABASE [Microservice_Security_DB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Microservice_Security_DB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Microservice_Security_DB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Microservice_Security_DB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Microservice_Security_DB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Microservice_Security_DB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Microservice_Security_DB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Microservice_Security_DB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Microservice_Security_DB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Microservice_Security_DB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Microservice_Security_DB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Microservice_Security_DB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Microservice_Security_DB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Microservice_Security_DB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Microservice_Security_DB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Microservice_Security_DB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Microservice_Security_DB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Microservice_Security_DB] SET RECOVERY FULL 
GO
ALTER DATABASE [Microservice_Security_DB] SET  MULTI_USER 
GO
ALTER DATABASE [Microservice_Security_DB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Microservice_Security_DB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Microservice_Security_DB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Microservice_Security_DB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Microservice_Security_DB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Microservice_Security_DB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Microservice_Security_DB', N'ON'
GO
ALTER DATABASE [Microservice_Security_DB] SET QUERY_STORE = OFF
GO
USE [Microservice_Security_DB]
GO
/****** Object:  Table [dbo].[SESSIONS]    Script Date: 19.11.2021 15:43:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SESSIONS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TOKEN] [nvarchar](50) NULL,
	[USERAGENT] [nvarchar](250) NULL,
	[USERID] [int] NULL,
	[VALIDTO] [datetime] NULL,
	[ISVALID] [bit] NULL,
	[IPADDRESS] [nvarchar](50) NULL,
	[CREATEDATE] [datetime] NULL,
	[UPDATEDATE] [datetime] NULL,
	[DELETE_DATE] [datetime] NULL,
 CONSTRAINT [PK_SESSIONS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USERS]    Script Date: 19.11.2021 15:43:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USERS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NAME] [nvarchar](50) NULL,
	[EMAIL] [nvarchar](250) NULL,
	[PASSWORD] [nvarchar](250) NULL,
	[ISADMIN] [bit] NULL,
	[CREATEDATE] [datetime] NULL,
	[UPDATEDATE] [datetime] NULL,
	[DELETE_DATE] [datetime] NULL,
 CONSTRAINT [PK_USERS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[USERS] ON 

INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (1001, N'LoggingService', N'Services.Infrastructure.Logging@service.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (1002, N'User', N'user@user.com', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (1003, N'HRService', N'Services.Business.Departments.HR@service.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (1004, N'AccountingService', N'Services.Business.Departments.Accounting@service.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (1005, N'PresentationUI', N'Presentation.UI.WindowsForm@ui.desktop', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (1006, N'ITService', N'Services.Business.Departments.IT@service.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (1007, N'AAService', N'Services.Business.Departments.AA@service.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (1008, N'MQ.AA.Service', N'Services.MQ.AA@queue.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (1009, N'MQ.IT.Service', N'Services.MQ.IT@queue.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (1010, N'BuyingService', N'Services.Business.Departments.Buying@service.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (2010, N'MQ.IT.Buying', N'Services.MQ.Buying@queue.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (2011, N'FinanceService', N'Services.Business.Departments.Finance@service.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (2012, N'MQ.IT.Finance', N'Services.MQ.Finance@queue.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (2013, N'Gateway.Public', N'Services.Gateway.Public@gateway.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (2014, N'Services.Monitoring.Security.Console', N'Services.Monitoring.Security.Console@application.app', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (2015, N'Services.WebSockets.Security', N'Services.WebSockets.Security@service.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (2016, N'Services.WebSockets.Reliability', N'Services.WebSockets.Reliability@service.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (2017, N'Services.Monitoring.Reliability.Console', N'Services.Monitoring.Reliability.Console@application.app', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (2018, N'CRService', N'Services.Business.Departments.CR@service.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (3018, N'SellingService', N'Services.Business.Departments.Selling@service.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (3019, N'ProductionService', N'Services.Business.Departments.Selling@service.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (3020, N'StorageService', N'Services.Business.Departments.Storage@service.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (3021, N'MQ.Storage', N'Services.MQ.Storage@queue.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (3022, N'MQ.Production', N'Services.MQ.Production@queue.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (3023, N'MQ.Selling', N'Services.MQ.Selling@queue.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[USERS] ([ID], [NAME], [EMAIL], [PASSWORD], [ISADMIN], [CREATEDATE], [UPDATEDATE], [DELETE_DATE]) VALUES (3024, N'UI.Web', N'Presentation.UI.Web@ui.service', N'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[USERS] OFF
GO
USE [master]
GO
ALTER DATABASE [Microservice_Security_DB] SET  READ_WRITE 
GO
