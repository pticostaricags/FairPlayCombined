CREATE TABLE [FairPlaySocial].[UserProfile]
(
	[UserProfileId] BIGINT NOT NULL CONSTRAINT PK_UserProfile PRIMARY KEY IDENTITY, 
    [ApplicationUserId] NVARCHAR(450) NOT NULL, 
    [Bio] NVARCHAR(500) NOT NULL, 
    [LinkedInNickname] NVARCHAR(50) NULL, 
    [TwitterNickname] NVARCHAR(50) NULL, 
    [FacebookNickname] NVARCHAR(50) NULL, 
    [InstagramNickname] NVARCHAR(50) NULL, 
    [YouTubeNickname] NVARCHAR(50) NULL, 
    [BuyMeACoffeeNickname] NVARCHAR(50) NULL, 
    [GithubSponsorsNickname] NVARCHAR(50) NULL, 
    CONSTRAINT [FK_UserProfile_ApplicationUser] FOREIGN KEY ([ApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
)

GO

CREATE UNIQUE INDEX [UI_UserProfile_ApplicationUserId] ON [FairPlaySocial].[UserProfile] ([ApplicationUserId])