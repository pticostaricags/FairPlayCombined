CREATE TABLE [FairPlayTube].[VideoDigitalMarketingPlan]
(
	[VideoDigitalMarketingPlan] BIGINT NOT NULL CONSTRAINT PK_VideoDigitalMarketingPlan PRIMARY KEY IDENTITY, 
    [VideoInfoId] BIGINT NOT NULL, 
    [SocialNetworkName] NVARCHAR(50) NOT NULL, 
    [HtmlDigitalMarketingPlan] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_VideoDigitalMarketingPlan_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo]([VideoInfoId])
)

GO

CREATE UNIQUE INDEX [IX_VideoDigitalMarketingPlan_SocialNetworkPlan] ON [FairPlayTube].[VideoDigitalMarketingPlan] ([VideoInfoId], [SocialNetworkName])
