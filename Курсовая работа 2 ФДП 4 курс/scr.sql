USE [master]
GO
/****** Object:  Database [CashboxDB]    Script Date: 15.03.2020 18:26:30 ******/
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
/****** Object:  User [romedov]    Script Date: 15.03.2020 18:26:30 ******/
CREATE USER [romedov] FOR LOGIN [romedov] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [romedov]
GO
ALTER ROLE [db_accessadmin] ADD MEMBER [romedov]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [romedov]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [romedov]
GO
ALTER ROLE [db_backupoperator] ADD MEMBER [romedov]
GO
ALTER ROLE [db_datareader] ADD MEMBER [romedov]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [romedov]
GO
ALTER ROLE [db_denydatareader] ADD MEMBER [romedov]
GO
ALTER ROLE [db_denydatawriter] ADD MEMBER [romedov]
GO
/****** Object:  Table [dbo].[FreeItems]    Script Date: 15.03.2020 18:26:30 ******/
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
/****** Object:  Table [dbo].[Items]    Script Date: 15.03.2020 18:26:30 ******/
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
/****** Object:  Table [dbo].[Returns]    Script Date: 15.03.2020 18:26:30 ******/
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
/****** Object:  Table [dbo].[Sales]    Script Date: 15.03.2020 18:26:30 ******/
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
/****** Object:  Table [dbo].[Shifts]    Script Date: 15.03.2020 18:26:30 ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 15.03.2020 18:26:30 ******/
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
INSERT [dbo].[FreeItems] ([SId], [CashSum]) VALUES (10119, CAST(30.00 AS Decimal(18, 2)))
INSERT [dbo].[FreeItems] ([SId], [CashSum]) VALUES (10120, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[FreeItems] ([SId], [CashSum]) VALUES (10121, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[FreeItems] ([SId], [CashSum]) VALUES (10122, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[FreeItems] ([SId], [CashSum]) VALUES (10123, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[FreeItems] ([SId], [CashSum]) VALUES (10124, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[FreeItems] ([SId], [CashSum]) VALUES (10125, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[FreeItems] ([SId], [CashSum]) VALUES (10126, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Items] ([IId], [Name], [Price], [Number], [Discount]) VALUES (N'101010', N'Вино "Онив"', CAST(500.00 AS Decimal(18, 2)), 20, 0)
INSERT [dbo].[Items] ([IId], [Name], [Price], [Number], [Discount]) VALUES (N'111111', N'Пицца', CAST(30.00 AS Decimal(18, 2)), 20, 0)
INSERT [dbo].[Items] ([IId], [Name], [Price], [Number], [Discount]) VALUES (N'123423', N'Шаурма', CAST(130.00 AS Decimal(18, 2)), 5, 5)
INSERT [dbo].[Items] ([IId], [Name], [Price], [Number], [Discount]) VALUES (N'123456', N'Молоко "Норов"', CAST(35.90 AS Decimal(18, 2)), 15, 0)
INSERT [dbo].[Items] ([IId], [Name], [Price], [Number], [Discount]) VALUES (N'333333', N'Пельмени "Поспелъ"', CAST(65.00 AS Decimal(18, 2)), 34, 5)
INSERT [dbo].[Items] ([IId], [Name], [Price], [Number], [Discount]) VALUES (N'555555', N'Хлеб "Мокшанский"', CAST(29.50 AS Decimal(18, 2)), 30, 0)
INSERT [dbo].[Items] ([IId], [Name], [Price], [Number], [Discount]) VALUES (N'666666', N'Клей "Момент"', CAST(10.00 AS Decimal(18, 2)), 50, 5)
INSERT [dbo].[Returns] ([SId], [IId], [Number]) VALUES (10119, N'101010', 2)
INSERT [dbo].[Sales] ([SId], [IId], [Number]) VALUES (10119, N'333333', 1)
SET IDENTITY_INSERT [dbo].[Shifts] ON 

INSERT [dbo].[Shifts] ([SId], [UId], [StartDateTime], [EndDateTime], [CashReceived], [CashReturned], [CashAdded], [CashWithdrawn], [CurrentCash]) VALUES (10119, N'root', CAST(N'2019-12-08T12:52:38.423' AS DateTime), CAST(N'2019-12-08T12:54:08.493' AS DateTime), CAST(95.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1095.00 AS Decimal(18, 2)))
INSERT [dbo].[Shifts] ([SId], [UId], [StartDateTime], [EndDateTime], [CashReceived], [CashReturned], [CashAdded], [CashWithdrawn], [CurrentCash]) VALUES (10120, N'root', CAST(N'2019-12-16T19:59:05.170' AS DateTime), CAST(N'2019-12-16T20:43:34.497' AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1095.00 AS Decimal(18, 2)))
INSERT [dbo].[Shifts] ([SId], [UId], [StartDateTime], [EndDateTime], [CashReceived], [CashReturned], [CashAdded], [CashWithdrawn], [CurrentCash]) VALUES (10121, N'root', CAST(N'2019-12-16T20:43:54.913' AS DateTime), CAST(N'2019-12-16T20:44:39.303' AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1095.00 AS Decimal(18, 2)))
INSERT [dbo].[Shifts] ([SId], [UId], [StartDateTime], [EndDateTime], [CashReceived], [CashReturned], [CashAdded], [CashWithdrawn], [CurrentCash]) VALUES (10122, N'root', CAST(N'2019-12-16T21:12:51.170' AS DateTime), CAST(N'2019-12-16T22:03:15.560' AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1095.00 AS Decimal(18, 2)))
INSERT [dbo].[Shifts] ([SId], [UId], [StartDateTime], [EndDateTime], [CashReceived], [CashReturned], [CashAdded], [CashWithdrawn], [CurrentCash]) VALUES (10123, N'root', CAST(N'2020-01-25T22:29:35.507' AS DateTime), CAST(N'2020-01-25T22:29:37.547' AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1095.00 AS Decimal(18, 2)))
INSERT [dbo].[Shifts] ([SId], [UId], [StartDateTime], [EndDateTime], [CashReceived], [CashReturned], [CashAdded], [CashWithdrawn], [CurrentCash]) VALUES (10124, N'root', CAST(N'2020-01-26T00:41:06.287' AS DateTime), CAST(N'2020-01-26T00:41:09.943' AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1095.00 AS Decimal(18, 2)))
INSERT [dbo].[Shifts] ([SId], [UId], [StartDateTime], [EndDateTime], [CashReceived], [CashReturned], [CashAdded], [CashWithdrawn], [CurrentCash]) VALUES (10125, N'root', CAST(N'2020-01-26T00:43:57.317' AS DateTime), CAST(N'2020-01-26T00:43:59.657' AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1095.00 AS Decimal(18, 2)))
INSERT [dbo].[Shifts] ([SId], [UId], [StartDateTime], [EndDateTime], [CashReceived], [CashReturned], [CashAdded], [CashWithdrawn], [CurrentCash]) VALUES (10126, N'root', CAST(N'2020-01-26T00:47:43.123' AS DateTime), CAST(N'2020-01-26T00:47:45.190' AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1095.00 AS Decimal(18, 2)))
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
ALTER TABLE [dbo].[FreeItems]  WITH CHECK ADD  CONSTRAINT [FK_FreeItemsToShifts] FOREIGN KEY([SId])
REFERENCES [dbo].[Shifts] ([SId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FreeItems] CHECK CONSTRAINT [FK_FreeItemsToShifts]
GO
ALTER TABLE [dbo].[Returns]  WITH CHECK ADD  CONSTRAINT [FK_ReturnsToItems] FOREIGN KEY([IId])
REFERENCES [dbo].[Items] ([IId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Returns] CHECK CONSTRAINT [FK_ReturnsToItems]
GO
ALTER TABLE [dbo].[Returns]  WITH CHECK ADD  CONSTRAINT [FK_ReturnsToShifts] FOREIGN KEY([SId])
REFERENCES [dbo].[Shifts] ([SId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Returns] CHECK CONSTRAINT [FK_ReturnsToShifts]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_SalesToItems] FOREIGN KEY([IId])
REFERENCES [dbo].[Items] ([IId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_SalesToItems]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_SalesToShifts] FOREIGN KEY([SId])
REFERENCES [dbo].[Shifts] ([SId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_SalesToShifts]
GO
ALTER TABLE [dbo].[Shifts]  WITH CHECK ADD  CONSTRAINT [FK_ShiftsToUsers] FOREIGN KEY([UId])
REFERENCES [dbo].[Users] ([UId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Shifts] CHECK CONSTRAINT [FK_ShiftsToUsers]
GO
USE [master]
GO
ALTER DATABASE [CashboxDB] SET  READ_WRITE 
GO
