/****** Object:  Table [dbo].[DailyDeposits]    Script Date: 10/9/2017 1:57:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DailyDeposits](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BusinessDate] [datetime] NOT NULL,
	[StoreNumber] [nvarchar](3) NULL,
	[GlAccount] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_dbo.DailyDeposits] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO