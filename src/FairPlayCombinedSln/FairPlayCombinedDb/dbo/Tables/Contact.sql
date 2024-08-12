CREATE TABLE [dbo].[Contact]
(
	[ContactId] BIGINT NOT NULL CONSTRAINT PK_Contact PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Lastname] NVARCHAR(50) NOT NULL, 
    [EmailAddress] NVARCHAR(50) NOT NULL, 
    [LinkedInProfileUrl] NVARCHAR(50) NULL, 
    [YouTubeChannelUrl] NVARCHAR(50) NULL, 
    [BusinessPhoneNumber] NVARCHAR(50) NULL, 
    [MobilePhoneNumber] NVARCHAR(50) NULL, 
    [BirthDate] DATETIMEOFFSET NULL, 
    [OwnerApplicationUserId] NVARCHAR(450) NOT NULL, 
    CONSTRAINT [FK_Contact_AspNetUsers] FOREIGN KEY ([OwnerApplicationUserId]) REFERENCES [AspNetUsers]([Id])
)
