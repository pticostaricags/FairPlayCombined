CREATE TABLE [FairPlayTube].[NewVideoRecommendation]
(
	[NewVideoRecommendationId] BIGINT NOT NULL CONSTRAINT PK_NewVideoRecommendation PRIMARY KEY IDENTITY, 
    [ApplicationUserId] NVARCHAR(450) NOT NULL, 
    [HtmlNewVideoRecommendation] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_NewVideoRecommendation_AspNetUsers] FOREIGN KEY ([ApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
)
