CREATE TABLE [FairPlayTube].[VideoKeyword]
(
	[VideoKeywordId] BIGINT NOT NULL CONSTRAINT PK_VideoKeyword PRIMARY KEY IDENTITY, 
    [VideoInfoId] BIGINT NOT NULL, 
    [Keyword] NVARCHAR(500) NOT NULL, 
    CONSTRAINT [FK_VideoKeyword_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo]([VideoInfoId])
)

GO

CREATE UNIQUE INDEX [UI_VideoKeyword_VideoInfoId_Keyword] ON [FairPlayTube].[VideoKeyword] ([VideoInfoId],[Keyword])
