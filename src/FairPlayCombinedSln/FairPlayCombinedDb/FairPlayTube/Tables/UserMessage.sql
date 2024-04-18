CREATE TABLE [FairPlayTube].[UserMessage]
(
	[UserMessageId] BIGINT NOT NULL CONSTRAINT PK_UserMessage PRIMARY KEY IDENTITY,
	[FromApplicationUserId] NVARCHAR(450) NOT NULL CONSTRAINT FK_FromApplicationUserId_AspNetUsers FOREIGN KEY REFERENCES [dbo].[AspNetUsers]([Id]),
	[ToApplicationUserId] NVARCHAR(450) NOT NULL CONSTRAINT FK_ToApplicationUserId_AspNetUsers FOREIGN KEY REFERENCES [dbo].[AspNetUsers]([Id]),
	[Message] NVARCHAR(MAX) NOT NULL,
	[SourceApplication] NVARCHAR(250) NOT NULL,
	[OriginatorIpaddress] NVARCHAR(100) NOT NULL,
	[RowCreationDateTime] DATETIMEOFFSET NOT NULL,
	[RowCreationUser] NVARCHAR(256) NOT NULL, 
    [ReadByDestinatary] BIT NOT NULL DEFAULT 0
)