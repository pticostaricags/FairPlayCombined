CREATE TABLE [FairPlayDating].[LikedUserProfile]
(
	[LikedUserProfileId] BIGINT NOT NULL CONSTRAINT PK_LikedUserProfile PRIMARY KEY IDENTITY, 
    [LikingApplicationUserId] NVARCHAR(450) NOT NULL, 
    [LikedApplicationUserId] NVARCHAR(450) NOT NULL,
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL,
    CONSTRAINT [FK_LikedUserProfile_LikingApplicactionUser] FOREIGN KEY ([LikingApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id]),
    CONSTRAINT [FK_LikedUserProfile_LikedApplicactionUser] FOREIGN KEY ([LikedApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
)

GO


CREATE UNIQUE INDEX [UI_LikedUserProfile_LikingApplicationUserId_LikedApplicationUserId] ON [FairPlayDating].[LikedUserProfile] ([LikingApplicationUserId], [LikedApplicationUserId])