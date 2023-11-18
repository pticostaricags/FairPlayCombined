CREATE TABLE [FairPlaySocial].[Group]
(
    [GroupId] BIGINT NOT NULL CONSTRAINT PK_Group PRIMARY KEY IDENTITY,
    [OwnerApplicationUserId]  NVARCHAR(450) NOT NULL,
    [Name] NVARCHAR(50) NOT NULL,
    [Description] NVARCHAR(250) NOT NULL,
    [TopicTag] NVARCHAR(100) NOT NULL,
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL,
    CONSTRAINT [FK_Group_ApplicationUser] FOREIGN KEY ([OwnerApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
)

GO

CREATE UNIQUE INDEX [UI_Group_Name] ON [FairPlaySocial].[Group] ([Name])