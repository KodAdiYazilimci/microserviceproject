USE [master]
GO
/****** Object:  Database [Microservice_Socket_DB]    Script Date: 6.10.2021 18:38:02 ******/
CREATE DATABASE [Microservice_Socket_DB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Microservice_Socket_DB', FILENAME = N'D:\Data Sources\MSSQL\Microservice_Socket_DB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Microservice_Socket_DB_log', FILENAME = N'D:\Data Sources\MSSQL\Microservice_Socket_DB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Microservice_Socket_DB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Microservice_Socket_DB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Microservice_Socket_DB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Microservice_Socket_DB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Microservice_Socket_DB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Microservice_Socket_DB] SET ARITHABORT OFF 
GO
ALTER DATABASE [Microservice_Socket_DB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Microservice_Socket_DB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Microservice_Socket_DB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Microservice_Socket_DB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Microservice_Socket_DB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Microservice_Socket_DB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Microservice_Socket_DB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Microservice_Socket_DB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Microservice_Socket_DB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Microservice_Socket_DB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Microservice_Socket_DB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Microservice_Socket_DB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Microservice_Socket_DB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Microservice_Socket_DB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Microservice_Socket_DB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Microservice_Socket_DB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Microservice_Socket_DB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Microservice_Socket_DB] SET RECOVERY FULL 
GO
ALTER DATABASE [Microservice_Socket_DB] SET  MULTI_USER 
GO
ALTER DATABASE [Microservice_Socket_DB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Microservice_Socket_DB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Microservice_Socket_DB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Microservice_Socket_DB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Microservice_Socket_DB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Microservice_Socket_DB', N'ON'
GO
ALTER DATABASE [Microservice_Socket_DB] SET QUERY_STORE = OFF
GO
USE [Microservice_Socket_DB]
GO
/****** Object:  Table [dbo].[WEBSOCKETS]    Script Date: 6.10.2021 18:38:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WEBSOCKETS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NAME] [nvarchar](50) NULL,
	[ENDPOINT] [nvarchar](500) NULL,
	[DELETE_DATE] [datetime] NULL,
	[METHOD] [nvarchar](50) NULL,
 CONSTRAINT [PK_WEBSOCKETS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[WEBSOCKETS] ON 

INSERT [dbo].[WEBSOCKETS] ([ID], [NAME], [ENDPOINT], [DELETE_DATE], [METHOD]) VALUES (1, N'websockets.security.tokenshub.gettokenmessages', N'http://localhost:43449/TokensHub', NULL, N'GetTokenMessages')
INSERT [dbo].[WEBSOCKETS] ([ID], [NAME], [ENDPOINT], [DELETE_DATE], [METHOD]) VALUES (2, N'websockets.reliability.errorhub.geterrormessages', N'http://localhost:23681/ErrorHub', NULL, N'GetErrorMessages')
SET IDENTITY_INSERT [dbo].[WEBSOCKETS] OFF
GO
USE [master]
GO
ALTER DATABASE [Microservice_Socket_DB] SET  READ_WRITE 
GO
