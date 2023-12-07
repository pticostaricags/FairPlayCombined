CREATE TABLE [FairPlayBudget].[Currency]
(
	[CurrencyId] INT NOT NULL CONSTRAINT PK_Currency PRIMARY KEY IDENTITY, 
    [Description] NVARCHAR(50) NOT NULL
)