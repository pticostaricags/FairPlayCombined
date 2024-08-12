CREATE TABLE [dbo].[Company]
(
	[CompanyId] BIGINT NOT NULL CONSTRAINT PK_Company PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [WebsiteUrl] NVARCHAR(MAX) NULL, 
    [Phone] NVARCHAR(50) NULL, 
    [PrimaryContactId] BIGINT NULL, 
    [YouTubeChannelUrl] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_Company_Contact] FOREIGN KEY ([PrimaryContactId]) REFERENCES [dbo].[Contact]([ContactId])
)
