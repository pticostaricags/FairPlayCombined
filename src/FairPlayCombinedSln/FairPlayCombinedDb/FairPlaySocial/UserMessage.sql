CREATE TABLE [FairPlaySocial].[UserMessage]
(
	[UserMessageId] BIGINT NOT NULL CONSTRAINT PK_UserMessage PRIMARY KEY IDENTITY,
	[FromApplicationUserId] NVARCHAR(450) NOT NULL CONSTRAINT FK_FromApplicationUserId_ApplicationUser FOREIGN KEY REFERENCES [dbo].[AspNetUsers]([Id]),
	[ToApplicationUserId] NVARCHAR(450) NOT NULL CONSTRAINT FK_ToApplicationUserId_ApplicationUser FOREIGN KEY REFERENCES [dbo].[AspNetUsers]([Id]),
	[Message] NVARCHAR(MAX) NOT NULL,
	[ReadByDestinatary] BIT NOT NULL DEFAULT 0,
	[SourceApplication] NVARCHAR(250) NOT NULL,
	[OriginatorIpaddress] NVARCHAR(100) NOT NULL,
	[RowCreationDateTime] DATETIMEOFFSET NOT NULL,
	[RowCreationUser] NVARCHAR(256) NOT NULL
)
GO

CREATE INDEX [IX_UserMessage_FromApplicationUserId] ON [FairPlaySocial].[UserMessage] ([FromApplicationUserId])

GO

CREATE INDEX [IX_UserMessage_ToApplicationUserId] ON [FairPlaySocial].[UserMessage] ([ToApplicationUserId])