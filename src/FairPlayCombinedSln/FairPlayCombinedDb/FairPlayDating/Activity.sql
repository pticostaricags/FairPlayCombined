CREATE TABLE [FairPlayDating].[Activity]
(
	[ActivityId] INT NOT NULL CONSTRAINT PK_Activity PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL
)
GO

CREATE UNIQUE INDEX [UI_Activity_Activity] ON [FairPlayDating].[Activity] ([Name])
