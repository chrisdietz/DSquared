INSERT INTO [dbo].[Codes]
           ([CodeCategory]
           ,[CodeValue]
           ,[UpdatedDate]
           ,[UpdatedBy]
           ,[CreatedDate]
           ,[CreatedBy])
     VALUES
           ('Controller', 'DailyDeposit', GetDate(), 'Seed Method', GetDate(), 'Seed Method'),
		   ('Controller', 'SalesForecast', GetDate(), 'Seed Method', GetDate(), 'Seed Method'),
		   ('Action', 'Index', GetDate(), 'Seed Method', GetDate(), 'Seed Method'),
		   ('Action', 'DepositReport', GetDate(), 'Seed Method', GetDate(), 'Seed Method'),
		   ('Action', 'Create', GetDate(), 'Seed Method', GetDate(), 'Seed Method'),
		   ('Action', 'Details', GetDate(), 'Seed Method', GetDate(), 'Seed Method'),
		   ('Action', 'Edit', GetDate(), 'Seed Method', GetDate(), 'Seed Method'),
		   ('Action', 'Delete', GetDate(), 'Seed Method', GetDate(), 'Seed Method')
GO


