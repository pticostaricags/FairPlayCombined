CREATE TABLE [dbo].[ImageStyle]
(
	[ImageStyleId] INT NOT NULL CONSTRAINT PK_ImageStyle PRIMARY KEY IDENTITY, 
    [StyleName] NVARCHAR(50) NOT NULL,
	[RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL
)

GO

CREATE UNIQUE INDEX [UX_ImageStyle_StyleName] ON [dbo].[ImageStyle] ([StyleName])
