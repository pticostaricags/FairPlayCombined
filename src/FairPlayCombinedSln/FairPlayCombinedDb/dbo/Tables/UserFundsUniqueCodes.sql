CREATE TABLE [dbo].[UserFundsUniqueCodes]
(
	[UserFundsUniqueCodesId] INT NOT NULL CONSTRAINT PK_UserFundsUniqueCodes PRIMARY KEY IDENTITY, 
    [Code] UNIQUEIDENTIFIER NOT NULL, 
    [IsClaimed] BIT NOT NULL,
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL
)

GO

CREATE UNIQUE INDEX [UI_UserFundsUniqueCodes_Column] ON [dbo].[UserFundsUniqueCodes] ([Code])
