CREATE TABLE [FairPlayTube].[VideoInfo] (
    [VideoInfoId]                       BIGINT             IDENTITY (1, 1) NOT NULL,
    [AccountId]                         UNIQUEIDENTIFIER   NOT NULL,
    [VideoId]                           NVARCHAR (50)      NULL,
    [Location]                          NVARCHAR (50)      NOT NULL,
    [Name]                              NVARCHAR (50)      NOT NULL,
    [Description]                       NVARCHAR (500)     NULL,
    [FileName]                          NVARCHAR (50)      NOT NULL,
    [VideoBloblUrl]                     NVARCHAR (500)     NULL,
    [IndexedVideoUrl]                   NVARCHAR (500)     NULL,
    [ApplicationUserId]                 NVARCHAR (450)     NOT NULL,
    [VideoIndexStatusId]                INT                NOT NULL,
    [VideoDurationInSeconds]            FLOAT (53)         DEFAULT ((0)) NOT NULL,
    [VideoIndexSourceClass]             NVARCHAR (500)     NULL,
    [Price]                             MONEY              DEFAULT ((0)) NOT NULL,
    [ExternalVideoSourceUrl]            NVARCHAR (500)     NULL,
    [VideoLanguageCode]                 NVARCHAR (10)      NULL,
    [VideoVisibilityId]                 SMALLINT           DEFAULT ((1)) NOT NULL,
    [ThumbnailUrl]                      NVARCHAR (500)     NULL,
    [RowCreationDateTime]               DATETIMEOFFSET (7) DEFAULT (getutcdate()) NOT NULL,
    [RowCreationUser]                   NVARCHAR (256)     DEFAULT ('Unknown') NOT NULL,
    [SourceApplication]                 NVARCHAR (250)     DEFAULT ('Unknown') NOT NULL,
    [OriginatorIPAddress]               NVARCHAR (100)     NOT NULL,
    [YouTubeVideoId]                    NVARCHAR (11)      NULL,
    [VideoIndexJSON]                    NVARCHAR (MAX)     NULL,
    [VideoThumbnailPhotoId]             BIGINT             NULL,
    [VideoIndexingProcessingPercentage] INT                DEFAULT ((0)) NULL,
    [PublishedUrl]                      NVARCHAR (1000)    NULL,
    [IsVideoGeneratedWithAI]            BIT                DEFAULT ((0)) NULL,
    CONSTRAINT [PK_VideoInfo] PRIMARY KEY CLUSTERED ([VideoInfoId] ASC),
    CONSTRAINT [FK_VideoInfo_ApplicationUser] FOREIGN KEY ([ApplicationUserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_VideoInfo_Photo_Thumbnail] FOREIGN KEY ([VideoThumbnailPhotoId]) REFERENCES [dbo].[Photo] ([PhotoId]),
    CONSTRAINT [FK_VideoInfo_VideoIndexStatus] FOREIGN KEY ([VideoIndexStatusId]) REFERENCES [FairPlayTube].[VideoIndexStatus] ([VideoIndexStatusId]),
    CONSTRAINT [FK_VideoInfo_VideoVisibility] FOREIGN KEY ([VideoVisibilityId]) REFERENCES [FairPlayTube].[VideoVisibility] ([VideoVisibilityId])
);



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

CREATE UNIQUE INDEX [UI_VideoInfo_YouTubeVideoId] ON [FairPlayTube].[VideoInfo] ([YouTubeVideoId]) WHERE YouTubeVideoId IS NOT NULL

GO

CREATE FULLTEXT INDEX ON [FairPlayTube].[VideoInfo]
    ([Description] LANGUAGE 1033)
    KEY INDEX [PK_VideoInfo]
    ON [ftDefaultCatalog];

