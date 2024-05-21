CREATE TABLE [FairPlayDating].[PersonalityType]
(
	[PersonalityTypeId] INT NOT NULL CONSTRAINT PK_PersonalityType PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL
)

GO

CREATE UNIQUE INDEX [UI_PersonalityType_Name] ON [FairPlayDating].[PersonalityType] ([Name])
