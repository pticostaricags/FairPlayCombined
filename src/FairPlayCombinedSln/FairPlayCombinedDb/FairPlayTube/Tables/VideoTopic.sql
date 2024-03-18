CREATE TABLE [FairPlayTube].[VideoTopic]
(
	[VideoTopicId] BIGINT NOT NULL CONSTRAINT PK_VideoTopic PRIMARY KEY IDENTITY, 
    [VideoInfoId] BIGINT NOT NULL, 
    [Topic] NVARCHAR(500) NOT NULL, 
    [Confidence] FLOAT NOT NULL, 
    CONSTRAINT [FK_VideoTopic_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo]([VideoInfoId])
)

GO

CREATE UNIQUE INDEX [UI_VideoTopic_VideoInfoId_Topic] ON [FairPlayTube].[VideoTopic] ([VideoInfoId],[Topic])
