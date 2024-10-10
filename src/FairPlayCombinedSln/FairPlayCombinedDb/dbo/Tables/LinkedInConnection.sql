CREATE TABLE [dbo].[LinkedInConnection]
(
	[LinkedInConnectionId] BIGINT NOT NULL CONSTRAINT PK_LinkedInConnection PRIMARY KEY IDENTITY, 
    [ApplicationUserId] NVARCHAR(450) NOT NULL, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NULL, 
    [ProfileUrl] NVARCHAR(1000) NOT NULL, 
    [EmailAddress] NVARCHAR(50) NULL, 
    [Company] NVARCHAR(1000) NULL, 
    [Position] NVARCHAR(1000) NULL, 
    [ConnectedOn] DATE NOT NULL, 
    CONSTRAINT [FK_LinkedInConnection_AspNetUsers] FOREIGN KEY ([ApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
)
