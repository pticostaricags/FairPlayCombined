CREATE TABLE [FairPlayDating].[ApplicationUserVouch]
(
	[ApplicationUserVouchId] BIGINT NOT NULL CONSTRAINT PK_ApplicationUserVouch PRIMARY KEY IDENTITY,
	[FromApplicationUserId] NVARCHAR(450) NOT NULL,
	[ToApplicationUserId] NVARCHAR(450) NOT NULL,
	[Description] NVARCHAR(500) NOT NULL,
	[RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [FK_ApplicationUserVouch_FromUser] FOREIGN KEY ([FromApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id]),
	CONSTRAINT [FK_ApplicationUserVouch_ToUser] FOREIGN KEY ([ToApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
)

GO

CREATE UNIQUE INDEX [UI_ApplicationUserVouch_FromApplicationUserId_ToApplicationUserId] ON [FairPlayDating].[ApplicationUserVouch] ([FromApplicationUserId], [ToApplicationUserId])