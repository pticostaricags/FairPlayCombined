CREATE TABLE [FairPlayTube].[VideoInfographic]
(
	[VideoInfographicId] BIGINT NOT NULL CONSTRAINT PK_VideoInfographic PRIMARY KEY IDENTITY, 
    [VideoInfoId] BIGINT NOT NULL, 
    [PhotoId] BIGINT NOT NULL, 
    CONSTRAINT [FK_Video_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo]([VideoInfoId]), 
    CONSTRAINT [FK_VideoInfographic_Photo] FOREIGN KEY ([PhotoId]) REFERENCES [dbo].[Photo]([PhotoId])
)

GO

CREATE UNIQUE INDEX [UI_VideoInfographic_VideoPhoto] ON [FairPlayTube].[VideoInfographic] ([VideoInfoId],[PhotoId])
