CREATE TABLE [FairPlayBudget].[Expense]
(
	[ExpenseId] BIGINT NOT NULL CONSTRAINT PK_Expense PRIMARY KEY IDENTITY, 
    [ExpenseDateTime] DATETIMEOFFSET NOT NULL, 
    [Description] NVARCHAR(500) NOT NULL, 
    [Amount] MONEY NOT NULL, 
    [OwnerId] NVARCHAR(450) NOT NULL, 
    [MonthlyBudgetInfoId] BIGINT NOT NULL, 
    [CurrencyId] INT NOT NULL, 
    CONSTRAINT [FK_Expense_AspNetUsers] FOREIGN KEY ([OwnerId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_Expense_MonthlyBudgetInfo] FOREIGN KEY ([MonthlyBudgetInfoId]) REFERENCES [FairPlayBudget].[MonthlyBudgetInfo]([MonthlyBudgetInfoId]), 
    CONSTRAINT [FK_Expense_Currency] FOREIGN KEY ([CurrencyId]) REFERENCES [FairPlayBudget].[Currency]([CurrencyId])
)