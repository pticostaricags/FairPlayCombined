CREATE TABLE [FairPlayDating].[UserMessage]
(
	[UserMessageId] BIGINT NOT NULL CONSTRAINT PK_UserMessage PRIMARY KEY IDENTITY,
	[FromApplicationUserId] NVARCHAR(450) NOT NULL CONSTRAINT FK_FromApplicationUserId_ApplicationUser FOREIGN KEY REFERENCES [dbo].[AspNetUsers]([Id]),
	[ToApplicationUserId] NVARCHAR(450) NOT NULL CONSTRAINT FK_ToApplicationUserId_ApplicationUser FOREIGN KEY REFERENCES [dbo].[AspNetUsers]([Id]),
	[Message] NVARCHAR(MAX) NOT NULL
)