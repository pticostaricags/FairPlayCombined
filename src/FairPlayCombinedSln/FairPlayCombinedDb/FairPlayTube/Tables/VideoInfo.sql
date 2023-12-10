CREATE TABLE [FairPlayTube].[VideoInfo]
(
	[VideoInfoId] BIGINT NOT NULL CONSTRAINT PK_VideoInfo PRIMARY KEY IDENTITY, 
    [AccountId] UNIQUEIDENTIFIER NOT NULL, 
    [VideoId] NVARCHAR(50) NULL, 
    [Location] NVARCHAR(50) NOT NULL, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(500) NULL, 
    [FileName] NVARCHAR(50) NOT NULL, 
    [VideoBloblUrl] NVARCHAR(500) NULL,
    [IndexedVideoUrl] NVARCHAR(500) NULL,
    [ApplicationUserId] NVARCHAR(450) NOT NULL, 
    [VideoIndexStatusId] INT NOT NULL, 
    [VideoDurationInSeconds] FLOAT NOT NULL DEFAULT 0, 
    [VideoIndexSourceClass] NVARCHAR(500) NULL, 
    [Price] MONEY NOT NULL DEFAULT 0,
    [ExternalVideoSourceUrl] NVARCHAR(500),
    [VideoLanguageCode] NVARCHAR(10) NULL, 
    [VideoVisibilityId] SMALLINT NOT NULL DEFAULT 1, 
    [ThumbnailUrl] NVARCHAR(500) NULL,
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(), 
    [RowCreationUser] NVARCHAR(256) NOT NULL DEFAULT 'Unknown',
    [SourceApplication] NVARCHAR(250) NOT NULL DEFAULT 'Unknown', 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL
    CONSTRAINT [FK_VideoInfo_ApplicationUser] FOREIGN KEY ([ApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id]), 
    [YouTubeVideoId] NVARCHAR(11) NULL, 
    CONSTRAINT [FK_VideoInfo_VideoIndexStatus] FOREIGN KEY ([VideoIndexStatusId]) REFERENCES [FairPlayTube].[VideoIndexStatus]([VideoIndexStatusId]), 
    CONSTRAINT [FK_VideoInfo_VideoVisibility] FOREIGN KEY ([VideoVisibilityId]) REFERENCES [FairPlayTube].[VideoVisibility]([VideoVisibilityId])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Video Owner Id',
    @level0type = N'SCHEMA',
    @level0name = N'FairPlayTube',
    @level1type = N'TABLE',
    @level1name = N'VideoInfo',
    @level2type = N'COLUMN',
    @level2name = N'ApplicationUserId'
GO

CREATE UNIQUE INDEX [UI_VideoInfo_VideoId] ON [FairPlayTube].[VideoInfo] ([VideoId])

GO

CREATE UNIQUE INDEX [UI_VideoInfo_YouTubeVideoId] ON [FairPlayTube].[VideoInfo] ([YouTubeVideoId])
