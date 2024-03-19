CREATE TABLE [dbo].[ConfigurationSecret]
(
	[ConfigurationSecretId] INT NOT NULL CONSTRAINT PK_ConfigurationSecret PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Value] NVARCHAR(100) NOT NULL
)
