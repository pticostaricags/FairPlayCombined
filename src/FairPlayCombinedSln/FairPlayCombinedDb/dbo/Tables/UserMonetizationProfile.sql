CREATE TABLE [dbo].[UserMonetizationProfile]
(
	[UserMonetizationProfileId] BIGINT NOT NULL CONSTRAINT PK_UserMonetizationProfile PRIMARY KEY IDENTITY, 
    [ApplicationUserId] NVARCHAR(450) NOT NULL,
    [GitHubSponsors] NVARCHAR(MAX) NULL, 
    [BuyMeACoffee] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_UserMonetizationProfile_AspNetUsers] FOREIGN KEY ([ApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id]), 
)

GO

CREATE UNIQUE INDEX [UI_UserMonetizationProfile_ApplicationUserId] ON [dbo].[UserMonetizationProfile] ([ApplicationUserId])
