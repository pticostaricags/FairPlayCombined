CREATE TABLE [FairPlayTube].[VideoThumbnail]
(
	[VideoThumbnailId] BIGINT NOT NULL CONSTRAINT PK_VideoThumbnail PRIMARY KEY IDENTITY, 
    [VideoInfoId] BIGINT NOT NULL, 
    [PhotoId] BIGINT NOT NULL, 
    [OpenAIPromptId] BIGINT NOT NULL, 
    CONSTRAINT [FK_VideoThumbnail_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo]([VideoInfoId]), 
    CONSTRAINT [FK_VideoThumbnail_Photo] FOREIGN KEY ([PhotoId]) REFERENCES [dbo].[Photo]([PhotoId]), 
    CONSTRAINT [FK_VideoThumbnail_OpenAIPrompt] FOREIGN KEY ([OpenAIPromptId]) REFERENCES [dbo].[OpenAIPrompt]([OpenAIPromptId])
)

GO

CREATE UNIQUE INDEX [UI_VideoThumbnail_VideoPhoto] ON [FairPlayTube].[VideoThumbnail] ([VideoInfoId],[PhotoId])
