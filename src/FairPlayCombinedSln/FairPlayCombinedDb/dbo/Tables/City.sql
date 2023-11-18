CREATE TABLE [dbo].[City]
(
	[CityId] INT NOT NULL CONSTRAINT PK_City PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(500) NOT NULL, 
    [StateOrProvinceId] INT NOT NULL, 
    CONSTRAINT [FK_City_StateOrProvince] FOREIGN KEY ([StateOrProvinceId]) REFERENCES [StateOrProvince]([StateOrProvinceId])
)

GO

CREATE UNIQUE INDEX [UI_City_Name] ON [dbo].[City] ([StateOrProvinceId],[Name])