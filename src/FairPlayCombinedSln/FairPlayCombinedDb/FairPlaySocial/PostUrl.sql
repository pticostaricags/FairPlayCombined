CREATE TABLE [FairPlaySocial].[PostUrl]
(
	[PostUrlId] BIGINT NOT NULL CONSTRAINT PK_PostUrl PRIMARY KEY IDENTITY, 
    [PostId] BIGINT NOT NULL, 
    [Url] NVARCHAR(1000) NOT NULL, 
    CONSTRAINT [FK_PostUrl_Post] FOREIGN KEY ([PostId]) REFERENCES [FairPlaySocial].[Post]([PostId])
)

GO

CREATE UNIQUE INDEX [UI_PostUrl_PostId_Url] ON [FairPlaySocial].[PostUrl] ([PostId], [Url])