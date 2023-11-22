CREATE TABLE [FairPlaySocial].[PostTag]
(
	[PostTagId] BIGINT NOT NULL CONSTRAINT PK_PostTag PRIMARY KEY IDENTITY, 
    [PostId] BIGINT NOT NULL, 
    [Tag] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_PostTag_Post] FOREIGN KEY ([PostId]) REFERENCES [FairPlaySocial].[Post]([PostId])
)

GO

CREATE UNIQUE INDEX [UI_PostTag_PostId_Tag] ON [FairPlaySocial].[PostTag] ([PostId], [Tag])