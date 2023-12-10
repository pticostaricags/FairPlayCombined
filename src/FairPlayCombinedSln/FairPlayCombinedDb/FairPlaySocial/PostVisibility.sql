CREATE TABLE [FairPlaySocial].[PostVisibility]
(
	[PostVisibilityId] INT NOT NULL CONSTRAINT PK_PostVisibility PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(11) NOT NULL, 
    [Description] NVARCHAR(50) NOT NULL
)

GO

CREATE UNIQUE INDEX [UI_PostVisibility_Name] ON [FairPlaySocial].[PostVisibility] ([Name])