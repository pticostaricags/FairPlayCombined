CREATE TABLE [FairPlaySocial].[PostType]
(
	[PostTypeId] INT NOT NULL CONSTRAINT PK_PostType PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(10) NOT NULL
)

GO

CREATE UNIQUE INDEX [UI_PostType_Name] ON [FairPlaySocial].[PostType] ([Name])