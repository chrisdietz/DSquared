/****** Object:  Table [dbo].[SalesForecasts]    Script Date: 11/8/2017 5:11:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SalesForecasts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BusinessDate] [datetime] NOT NULL,
	[StoreNumber] [nvarchar](3) NULL,
	[ActualPriorYear] [decimal](18, 2) NOT NULL,
	[AvgPrior4Weeks] [decimal](18, 2) NOT NULL,
	[LaborForecast] [decimal](18, 2) NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ForecastAmount] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_dbo.SalesForecasts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SalesForecasts] ADD  DEFAULT ((0)) FOR [ForecastAmount]
GO


