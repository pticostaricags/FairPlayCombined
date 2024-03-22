CREATE TABLE [dbo].[ConfigurationSecret]
(
	[ConfigurationSecretId] INT NOT NULL CONSTRAINT PK_ConfigurationSecret PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Value] NVARCHAR(100) NOT NULL
)

GO

CREATE UNIQUE INDEX [UI_ConfigurationSecret_Name] ON [dbo].[ConfigurationSecret] ([Name])
