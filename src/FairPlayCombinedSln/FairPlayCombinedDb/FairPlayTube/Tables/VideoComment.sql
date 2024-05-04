CREATE TABLE [FairPlayTube].[VideoComment]
(
	[VideoCommentId] BIGINT NOT NULL CONSTRAINT PK_VideoComment PRIMARY KEY IDENTITY,
	[VideoInfoId] BIGINT NOT NULL CONSTRAINT FK_VideoInfo_VideoComment REFERENCES [FairPlayTube].[VideoInfo]([VideoInfoId]),
	[ApplicationUserId] NVARCHAR(450) NOT NULL CONSTRAINT FK_ApplicationUserId_VideoComment REFERENCES [dbo].[AspNetUsers]([Id]),
	[Comment] NVARCHAR(500) NOT NULL,
	[RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256),
    [SourceApplication] NVARCHAR(250), 
    [OriginatorIPAddress] NVARCHAR(100),
)
