CREATE TABLE [FairPlayDating].[Profession]
(
	[ProfessionId] INT NOT NULL CONSTRAINT PK_Profession PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(100) NOT NULL
)

GO

CREATE UNIQUE INDEX [UI_Profession_Name] ON [FairPlayDating].[Profession] ([Name])
