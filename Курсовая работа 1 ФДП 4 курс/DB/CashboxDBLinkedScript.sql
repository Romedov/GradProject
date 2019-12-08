USE [CashboxDB]
GO
/****** Object:  Table [dbo].[FreeItems]    Script Date: 08.12.2019 13:04:11 ******/
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
/****** Object:  Table [dbo].[Items]    Script Date: 08.12.2019 13:04:11 ******/
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
/****** Object:  Table [dbo].[Returns]    Script Date: 08.12.2019 13:04:11 ******/
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
/****** Object:  Table [dbo].[Sales]    Script Date: 08.12.2019 13:04:11 ******/
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
/****** Object:  Table [dbo].[Shifts]    Script Date: 08.12.2019 13:04:11 ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 08.12.2019 13:04:11 ******/
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
INSERT [dbo].[Items] ([IId], [Name], [Price], [Number], [Discount]) VALUES (N'101010', N'Вино "Онив"', CAST(500.00 AS Decimal(18, 2)), 20, 0)
INSERT [dbo].[Items] ([IId], [Name], [Price], [Number], [Discount]) VALUES (N'111111', N'Пицца', CAST(30.00 AS Decimal(18, 2)), 20, 0)
INSERT [dbo].[Items] ([IId], [Name], [Price], [Number], [Discount]) VALUES (N'123423', N'Шаурма', CAST(130.00 AS Decimal(18, 2)), 5, 5)
INSERT [dbo].[Items] ([IId], [Name], [Price], [Number], [Discount]) VALUES (N'123456', N'Молоко "Норов"', CAST(35.90 AS Decimal(18, 2)), 15, 10)
INSERT [dbo].[Items] ([IId], [Name], [Price], [Number], [Discount]) VALUES (N'333333', N'Пельмени "Поспелъ"', CAST(65.00 AS Decimal(18, 2)), 34, 5)
INSERT [dbo].[Items] ([IId], [Name], [Price], [Number], [Discount]) VALUES (N'555555', N'Хлеб "Мокшанский"', CAST(29.50 AS Decimal(18, 2)), 30, 0)
INSERT [dbo].[Items] ([IId], [Name], [Price], [Number], [Discount]) VALUES (N'666666', N'Клей "Момент"', CAST(10.00 AS Decimal(18, 2)), 50, 5)
INSERT [dbo].[Sales] ([SId], [IId], [Number]) VALUES (10119, N'333333', 1)
SET IDENTITY_INSERT [dbo].[Shifts] ON 

INSERT [dbo].[Shifts] ([SId], [UId], [StartDateTime], [EndDateTime], [CashReceived], [CashReturned], [CashAdded], [CashWithdrawn], [CurrentCash]) VALUES (10119, N'root', CAST(N'2019-12-08T12:52:38.423' AS DateTime), CAST(N'2019-12-08T12:54:08.493' AS DateTime), CAST(95.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1095.00 AS Decimal(18, 2)))
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
