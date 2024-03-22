CREATE TABLE [FairPlayTube].[VideoInfographic]
(
	[VideoInfographicId] BIGINT NOT NULL CONSTRAINT PK_VideoInfographic PRIMARY KEY, 
    [VideoInfoId] BIGINT NOT NULL IDENTITY, 
    [ImageBytes] VARBINARY(50) NOT NULL, 
    CONSTRAINT [FK_Video_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo]([VideoInfoId])
)
