CREATE TABLE [FairPlayTube].[VideoIndexerSupportedLanguage]
(
	[VideoIndexerSupportedLanguageId] INT NOT NULL CONSTRAINT PK_VideoIndexerSupportedLanguage PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [LanguageCode] NVARCHAR(10) NOT NULL
)

GO

CREATE UNIQUE INDEX [UI_VideoIndexerSupportedLanguage_Name] ON [FairPlayTube].[VideoIndexerSupportedLanguage] ([Name])

GO

CREATE UNIQUE INDEX [UI_VideoIndexerSupportedLanguage_LanguageCode] ON [FairPlayTube].[VideoIndexerSupportedLanguage] ([LanguageCode])
