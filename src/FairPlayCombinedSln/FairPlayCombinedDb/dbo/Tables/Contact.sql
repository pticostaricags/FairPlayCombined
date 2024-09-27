CREATE TABLE [dbo].[Contact]
(
	[ContactId] BIGINT NOT NULL CONSTRAINT PK_Contact PRIMARY KEY IDENTITY, 
    [OwnerApplicationUserId] NVARCHAR(450) NOT NULL, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Lastname] NVARCHAR(50) NOT NULL, 
    [EmailAddress] NVARCHAR(50) NOT NULL, 
    [LinkedInProfileUrl] NVARCHAR(50) NULL, 
    [YouTubeChannelUrl] NVARCHAR(50) NULL, 
    [InstagramUrl] NVARCHAR(MAX) NULL, 
    [BusinessPhoneNumber] NVARCHAR(50) NULL, 
    [MobilePhoneNumber] NVARCHAR(50) NULL, 
    [BirthDate] DATETIMEOFFSET NULL, 
    CONSTRAINT [FK_Contact_AspNetUsers] FOREIGN KEY ([OwnerApplicationUserId]) REFERENCES [AspNetUsers]([Id])
)

GO

CREATE UNIQUE INDEX [UI_Contact_EmailAddress] ON [dbo].[Contact] ([EmailAddress])
