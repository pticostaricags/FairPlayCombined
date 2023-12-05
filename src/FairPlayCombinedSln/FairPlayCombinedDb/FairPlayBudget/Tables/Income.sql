CREATE TABLE [FairPlayBudget].[Income]
(
	[IncomeId] BIGINT NOT NULL CONSTRAINT PK_Income PRIMARY KEY IDENTITY,
    [IncomeDateTime] DATETIMEOFFSET NOT NULL, 
    [Description] NVARCHAR(500) NOT NULL, 
    [Amount] MONEY NOT NULL, 
    [OwnerId] NVARCHAR(450) NOT NULL, 
    [MonthlyBudgetInfoId] BIGINT NOT NULL, 
    [CurrencyId] INT NOT NULL, 
    CONSTRAINT [FK_Income_AspNetUsers] FOREIGN KEY ([OwnerId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_Income_MonthlyBudgetInfo] FOREIGN KEY ([MonthlyBudgetInfoId]) REFERENCES [FairPlayBudget].[MonthlyBudgetInfo]([MonthlyBudgetInfoId]), 
    CONSTRAINT [FK_Income_Currency] FOREIGN KEY ([CurrencyId]) REFERENCES [FairPlayBudget].[Currency]([CurrencyId]), 
)