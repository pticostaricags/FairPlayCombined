CREATE TABLE [FairPlayTube].[VideoDigitalMarketingDailyPosts]
(
	[VideoDigitalMarketingDailyPostsId] BIGINT NOT NULL CONSTRAINT PK_VideoDigitalMarketingDailyPosts PRIMARY KEY IDENTITY, 
    [VideoInfoId] BIGINT NOT NULL, 
    [SocialNetworkName] NVARCHAR(50) NOT NULL, 
    [HtmlVideoDigitalMarketingDailyPostsIdeas] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_VideoDigitalMarketingDailyPosts_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo]([VideoInfoId])
)

GO

CREATE INDEX [IX_VideoDigitalMarketingDailyPosts_SocialNetworkPlan] ON [FairPlayTube].[VideoDigitalMarketingDailyPosts] ([VideoInfoId], [SocialNetworkName])
