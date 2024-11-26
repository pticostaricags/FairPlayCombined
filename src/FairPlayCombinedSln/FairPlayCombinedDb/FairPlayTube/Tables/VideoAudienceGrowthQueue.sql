CREATE TABLE [FairPlayTube].[VideoAudienceGrowthQueue] (
    [VideoAudienceGrowthQueueId] BIGINT             IDENTITY (1, 1) NOT NULL,
    [VideoInfoId]                BIGINT             NOT NULL,
    [LastTimePublished]          DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_VideoAudienceGrowthQueue] PRIMARY KEY CLUSTERED ([VideoAudienceGrowthQueueId] ASC),
    CONSTRAINT [FK_VideoAudienceGrowthQueue_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo] ([VideoInfoId])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UI_VideoAudienceGrowthQueue_VideoInfoId]
    ON [FairPlayTube].[VideoAudienceGrowthQueue]([VideoInfoId] ASC);

