USE [master]
GO
/****** Object:  Database [CashboxDB]    Script Date: 09.11.2019 17:39:12 ******/
CREATE DATABASE [CashboxDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CashboxDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.HOMESERVER\MSSQL\DATA\CashboxDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CashboxDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.HOMESERVER\MSSQL\DATA\CashboxDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [CashboxDB] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CashboxDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CashboxDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CashboxDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CashboxDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CashboxDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CashboxDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [CashboxDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CashboxDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CashboxDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CashboxDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CashboxDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CashboxDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CashboxDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CashboxDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CashboxDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CashboxDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CashboxDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CashboxDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CashboxDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CashboxDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CashboxDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CashboxDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CashboxDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CashboxDB] SET RECOVERY FULL 
GO
ALTER DATABASE [CashboxDB] SET  MULTI_USER 
GO
ALTER DATABASE [CashboxDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CashboxDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CashboxDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CashboxDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CashboxDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CashboxDB] SET QUERY_STORE = OFF
GO
USE [CashboxDB]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [CashboxDB]
GO
/****** Object:  Table [dbo].[FreeItems]    Script Date: 09.11.2019 17:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FreeItems](
	[SId] [bigint] NOT NULL,
	[CashSum] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_FreeItems] PRIMARY KEY CLUSTERED 
(
	[SId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Items]    Script Date: 09.11.2019 17:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Items](
	[IId] [varchar](50) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Number] [bigint] NOT NULL,
	[Discount] [int] NOT NULL,
 CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED 
(
	[IId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Returns]    Script Date: 09.11.2019 17:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Returns](
	[SId] [bigint] NOT NULL,
	[IId] [varchar](50) NOT NULL,
	[Number] [bigint] NOT NULL,
 CONSTRAINT [PK_Returns] PRIMARY KEY CLUSTERED 
(
	[SId] ASC,
	[IId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sales]    Script Date: 09.11.2019 17:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sales](
	[SId] [bigint] NOT NULL,
	[IId] [varchar](50) NOT NULL,
	[Number] [bigint] NOT NULL,
 CONSTRAINT [PK_Sales] PRIMARY KEY CLUSTERED 
(
	[SId] ASC,
	[IId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Shifts]    Script Date: 09.11.2019 17:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Shifts](
	[SId] [bigint] IDENTITY(1,1) NOT NULL,
	[UId] [varchar](50) NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[EndDateTime] [datetime] NULL,
	[CashReceived] [decimal](18, 2) NOT NULL,
	[CashReturned] [decimal](18, 2) NOT NULL,
	[CashAdded] [decimal](18, 2) NOT NULL,
	[CashWithdrawn] [decimal](18, 2) NOT NULL,
	[CurrentCash] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Shift] PRIMARY KEY CLUSTERED 
(
	[SId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 09.11.2019 17:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UId] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[SurName] [varchar](50) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[FatherName] [varchar](50) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[FreeItems] ([SId], [CashSum]) VALUES (116, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[FreeItems] ([SId], [CashSum]) VALUES (117, CAST(500.00 AS Decimal(18, 2)))
INSERT [dbo].[FreeItems] ([SId], [CashSum]) VALUES (118, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Items] ([IId], [Name], [Price], [Number], [Discount]) VALUES (N'123456', N'Молоко "Норов"', CAST(35.90 AS Decimal(18, 2)), 45, 10)
INSERT [dbo].[Items] ([IId], [Name], [Price], [Number], [Discount]) VALUES (N'555555', N'Хлеб "Мокшанский"', CAST(29.50 AS Decimal(18, 2)), 30, 0)
INSERT [dbo].[Items] ([IId], [Name], [Price], [Number], [Discount]) VALUES (N'666666', N'Клей "Момент"', CAST(14.00 AS Decimal(18, 2)), 102, 5)
INSERT [dbo].[Returns] ([SId], [IId], [Number]) VALUES (112, N'666666', 20)
INSERT [dbo].[Returns] ([SId], [IId], [Number]) VALUES (114, N'666666', 2)
INSERT [dbo].[Returns] ([SId], [IId], [Number]) VALUES (117, N'123456', 2)
INSERT [dbo].[Sales] ([SId], [IId], [Number]) VALUES (117, N'123456', 5)
SET IDENTITY_INSERT [dbo].[Shifts] ON 

INSERT [dbo].[Shifts] ([SId], [UId], [StartDateTime], [EndDateTime], [CashReceived], [CashReturned], [CashAdded], [CashWithdrawn], [CurrentCash]) VALUES (116, N'root', CAST(N'2019-11-09T16:40:20.677' AS DateTime), CAST(N'2019-11-09T16:40:24.407' AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(600.00 AS Decimal(18, 2)))
INSERT [dbo].[Shifts] ([SId], [UId], [StartDateTime], [EndDateTime], [CashReceived], [CashReturned], [CashAdded], [CashWithdrawn], [CurrentCash]) VALUES (117, N'root', CAST(N'2019-11-09T17:26:18.760' AS DateTime), CAST(N'2019-11-09T17:27:56.450' AS DateTime), CAST(679.50 AS Decimal(18, 2)), CAST(71.80 AS Decimal(18, 2)), CAST(400.00 AS Decimal(18, 2)), CAST(107.70 AS Decimal(18, 2)), CAST(1500.00 AS Decimal(18, 2)))
INSERT [dbo].[Shifts] ([SId], [UId], [StartDateTime], [EndDateTime], [CashReceived], [CashReturned], [CashAdded], [CashWithdrawn], [CurrentCash]) VALUES (118, N'root', CAST(N'2019-11-09T17:33:30.780' AS DateTime), CAST(N'2019-11-09T17:33:32.607' AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1500.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Shifts] OFF
INSERT [dbo].[Users] ([UId], [Password], [SurName], [Name], [FatherName]) VALUES (N'root', N'1243', N'Куроедов', N'Роман', N'Александрович')
ALTER TABLE [dbo].[Shifts] ADD  CONSTRAINT [DF_Shifts_EndDateTime]  DEFAULT (NULL) FOR [EndDateTime]
GO
ALTER TABLE [dbo].[Shifts] ADD  CONSTRAINT [DF_Shifts_CashReceived]  DEFAULT ((0)) FOR [CashReceived]
GO
ALTER TABLE [dbo].[Shifts] ADD  CONSTRAINT [DF_Shifts_CashReturned]  DEFAULT ((0)) FOR [CashReturned]
GO
ALTER TABLE [dbo].[Shifts] ADD  CONSTRAINT [DF_Shifts_CashAdded]  DEFAULT ((0)) FOR [CashAdded]
GO
ALTER TABLE [dbo].[Shifts] ADD  CONSTRAINT [DF_Shifts_CashWithdrawn]  DEFAULT ((0)) FOR [CashWithdrawn]
GO
USE [master]
GO
ALTER DATABASE [CashboxDB] SET  READ_WRITE 
GO
