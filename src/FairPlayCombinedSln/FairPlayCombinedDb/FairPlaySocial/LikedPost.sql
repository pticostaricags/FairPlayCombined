CREATE TABLE [FairPlaySocial].[LikedPost]
(
	[LikedPostId] BIGINT NOT NULL CONSTRAINT PK_LikedPost PRIMARY KEY IDENTITY, 
    [PostId] BIGINT NOT NULL, 
    [LikingApplicationUserId] NVARCHAR(450) NOT NULL, 
    CONSTRAINT [FK_LikedPost_Post] FOREIGN KEY ([PostId]) REFERENCES [FairPlaySocial].[Post]([PostId]), 
    CONSTRAINT [FK_LikedPost_ApplicationUser] FOREIGN KEY ([LikingApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
)

GO

CREATE UNIQUE INDEX [UI_LikedPost_PoistId_LikingApplicationUserId] ON [FairPlaySocial].[LikedPost] ([PostId], [LikingApplicationUserId])