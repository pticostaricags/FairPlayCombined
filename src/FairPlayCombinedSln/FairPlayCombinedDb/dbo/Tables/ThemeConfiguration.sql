CREATE TABLE [dbo].[ThemeConfiguration]
(
	[ThemeConfigurationId] INT NOT NULL CONSTRAINT PK_ThemeConfiguration PRIMARY KEY IDENTITY, 
    [Key] NVARCHAR(50) NOT NULL, 
    [Value] NVARCHAR(50) NOT NULL
)
