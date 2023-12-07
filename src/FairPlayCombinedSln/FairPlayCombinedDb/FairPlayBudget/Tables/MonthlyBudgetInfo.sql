CREATE TABLE [FairPlayBudget].[MonthlyBudgetInfo]
(
	[MonthlyBudgetInfoId] BIGINT NOT NULL CONSTRAINT PK_MonthlyBudgetInfo PRIMARY KEY IDENTITY, 
    [Description] NVARCHAR(150) NOT NULL,
	[OwnerId] NVARCHAR(450) NOT NULL, 
    CONSTRAINT [FK_MonthlyBudgetInfo_AspNetUsers] FOREIGN KEY ([OwnerId]) REFERENCES [AspNetUsers]([Id])
)

GO

CREATE UNIQUE INDEX [UI_MonthlyBudgetInfo_Description] ON [FairPlayBudget].[MonthlyBudgetInfo] ([Description])