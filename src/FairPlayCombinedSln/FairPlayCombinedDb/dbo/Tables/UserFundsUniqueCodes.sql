CREATE TABLE [dbo].[UserFundsUniqueCodes]
(
	[UserFundsUniqueCodesId] INT NOT NULL CONSTRAINT PK_UserFundsUniqueCodes PRIMARY KEY IDENTITY, 
    [Code] UNIQUEIDENTIFIER NOT NULL, 
    [IsClaimed] BIT NOT NULL,
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL, 
    [OwnerFullName] NVARCHAR(100) NULL, 
    [OwnerEmailAddress] NVARCHAR(100) NULL, 
    [OwnerLinkedProfileUrl] NVARCHAR(500) NULL, 
    [ClaimedByApplicationUserId] NVARCHAR(450) NULL, 
    CONSTRAINT [FK_UserFundsUniqueCodes_AspNetUsers] FOREIGN KEY ([ClaimedByApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
)

GO

CREATE UNIQUE INDEX [UI_UserFundsUniqueCodes_Column] ON [dbo].[UserFundsUniqueCodes] ([Code])
