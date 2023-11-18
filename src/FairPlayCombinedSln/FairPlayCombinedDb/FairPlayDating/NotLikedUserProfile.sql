CREATE TABLE [FairPlayDating].[NotLikedUserProfile]
(
	[NotLikedUserProfileId] BIGINT NOT NULL CONSTRAINT PK_NotLikedUserProfile PRIMARY KEY IDENTITY, 
    [NotLikingApplicationUserId] NVARCHAR(450) NOT NULL, 
    [NotLikedApplicationUserId] NVARCHAR(450) NOT NULL,
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL,
    CONSTRAINT [FK_NotLikedUserProfile_NotLikingApplicactionUser] FOREIGN KEY ([NotLikingApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id]),
    CONSTRAINT [FK_NotLikedUserProfile_NotLikedApplicactionUser] FOREIGN KEY ([NotLikedApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
)

GO


CREATE UNIQUE INDEX [UI_NotLikedUserProfile_NotLikingApplicationUserId_NotLikedApplicationUserId] ON [FairPlayDating].[NotLikedUserProfile] ([NotLikingApplicationUserId], [NotLikedApplicationUserId])