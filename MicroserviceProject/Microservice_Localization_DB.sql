USE [master]
GO
/****** Object:  Database [Microservice_Localization_DB]    Script Date: 4.12.2021 20:50:13 ******/
CREATE DATABASE [Microservice_Localization_DB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Microservice_Localization_DB', FILENAME = N'C:\Data Sources\MSSQL\Microservice_Localization_DB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Microservice_Localization_DB_log', FILENAME = N'C:\Data Sources\MSSQL\Microservice_Localization_DB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Microservice_Localization_DB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Microservice_Localization_DB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Microservice_Localization_DB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Microservice_Localization_DB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Microservice_Localization_DB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Microservice_Localization_DB] SET ARITHABORT OFF 
GO
ALTER DATABASE [Microservice_Localization_DB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Microservice_Localization_DB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Microservice_Localization_DB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Microservice_Localization_DB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Microservice_Localization_DB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Microservice_Localization_DB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Microservice_Localization_DB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Microservice_Localization_DB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Microservice_Localization_DB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Microservice_Localization_DB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Microservice_Localization_DB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Microservice_Localization_DB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Microservice_Localization_DB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Microservice_Localization_DB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Microservice_Localization_DB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Microservice_Localization_DB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Microservice_Localization_DB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Microservice_Localization_DB] SET RECOVERY FULL 
GO
ALTER DATABASE [Microservice_Localization_DB] SET  MULTI_USER 
GO
ALTER DATABASE [Microservice_Localization_DB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Microservice_Localization_DB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Microservice_Localization_DB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Microservice_Localization_DB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Microservice_Localization_DB', N'ON'
GO
USE [Microservice_Localization_DB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 4.12.2021 20:50:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TRANSLATIONS]    Script Date: 4.12.2021 20:50:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TRANSLATIONS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[KEY] [nvarchar](250) NULL,
	[TEXT] [nvarchar](4000) NULL,
	[REGION] [nvarchar](50) NULL,
	[CREATEDATE] [datetime] NOT NULL,
	[UPDATEDATE] [datetime] NOT NULL,
	[DELETEDATE] [datetime] NULL,
 CONSTRAINT [PK_TRANSLATIONS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20211204174840_init', N'5.0.12')
GO
SET IDENTITY_INSERT [dbo].[TRANSLATIONS] ON 

INSERT [dbo].[TRANSLATIONS] ([ID], [KEY], [TEXT], [REGION], [CREATEDATE], [UPDATEDATE], [DELETEDATE]) VALUES (3, N'Tanimsiz.Geri.Alma', N'Tanımlanmamış geri alma biçimi', N'tr', CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[TRANSLATIONS] ([ID], [KEY], [TEXT], [REGION], [CREATEDATE], [UPDATEDATE], [DELETEDATE]) VALUES (4, N'Tanimsiz.Geri.Alma', N'Undefined rollback', N'en', CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[TRANSLATIONS] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_TRANSLATIONS_KEY_REGION]    Script Date: 4.12.2021 20:50:13 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_TRANSLATIONS_KEY_REGION] ON [dbo].[TRANSLATIONS]
(
	[KEY] ASC,
	[REGION] ASC
)
WHERE ([KEY] IS NOT NULL AND [REGION] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [Microservice_Localization_DB] SET  READ_WRITE 
GO
