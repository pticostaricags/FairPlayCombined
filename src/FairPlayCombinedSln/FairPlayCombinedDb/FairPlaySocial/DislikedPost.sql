CREATE TABLE [FairPlaySocial].[DislikedPost]
(
	[DislikedPostId] BIGINT NOT NULL CONSTRAINT PK_DislikedPost PRIMARY KEY IDENTITY, 
    [PostId] BIGINT NOT NULL, 
    [DislikingApplicationUserId] NVARCHAR(450) NOT NULL, 
    CONSTRAINT [FK_DislikedPost_Post] FOREIGN KEY ([PostId]) REFERENCES [FairPlaySocial].[Post]([PostId]), 
    CONSTRAINT [FK_DislikedPost_ApplicationUser] FOREIGN KEY ([DislikingApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
)

GO

CREATE UNIQUE INDEX [UI_DislikedPost_PoistId_DislikingApplicationUserId] ON [FairPlaySocial].[DislikedPost] ([PostId], [DislikingApplicationUserId])