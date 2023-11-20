CREATE TABLE [FairPlayDating].[TattooStatus]
(
	[TattooStatusId] INT NOT NULL CONSTRAINT PK_TattooStatus PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL
)

GO

CREATE UNIQUE INDEX [UI_TattooStatus_Name] ON [FairPlayDating].[TattooStatus] ([Name])