CREATE TABLE [FairPlayTube].[VideoFaceThumbnail]
(
	[VideoFaceThumbnailId] BIGINT NOT NULL CONSTRAINT PK_VideoFaceThumbnail PRIMARY KEY IDENTITY, 
    [VideoInfoId] BIGINT NOT NULL, 
    [PhotoId] BIGINT NOT NULL, 
    [FaceName] NVARCHAR(100) NULL, 
    CONSTRAINT [FK_VideoFaceThumbnail_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo]([VideoInfoId]), 
    CONSTRAINT [FK_VideoFaceThumbnail_Photo] FOREIGN KEY ([PhotoId]) REFERENCES [dbo].[Photo]([PhotoId])
)

GO



CREATE UNIQUE INDEX [UI_VideoFaceThumbnail_VideoFaceThumbnail] ON [FairPlayTube].[VideoFaceThumbnail] ([VideoInfoId],[PhotoId])
