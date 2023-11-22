CREATE TABLE [FairPlaySocial].[ApplicationUserFollow]
(
	[ApplicationUserFollowId] BIGINT NOT NULL CONSTRAINT PK_ApplicationUserFollow PRIMARY KEY IDENTITY, 
    [FollowerApplicationUserId] NVARCHAR(450) NOT NULL, 
    [FollowedApplicationUserId] NVARCHAR(450) NOT NULL,
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [FK_ApplicationUserFollow_FollowerApplicationUser] FOREIGN KEY ([FollowerApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id]),
    CONSTRAINT [FK_ApplicationUserFollow_FollowedApplicationUser] FOREIGN KEY ([FollowedApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
)

GO

CREATE UNIQUE INDEX [UI_ApplicationUserFollow_FollowerApplicationUserId_FollowedApplicationUserId] ON [FairPlaySocial].[ApplicationUserFollow] ([FollowerApplicationUserId], [FollowedApplicationUserId])