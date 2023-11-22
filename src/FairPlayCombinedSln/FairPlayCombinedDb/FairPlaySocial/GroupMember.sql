CREATE TABLE [FairPlaySocial].[GroupMember]
(
    [GroupMemberId] BIGINT NOT NULL CONSTRAINT PK_GroupMember PRIMARY KEY IDENTITY,
    [GroupId] BIGINT NOT NULL, 
    [MemberApplicationUserId]  NVARCHAR(450) NOT NULL,
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL,
    CONSTRAINT [FK_GroupMember_ApplicationUser] FOREIGN KEY ([MemberApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id]), 
    CONSTRAINT [FK_GroupMember_Group] FOREIGN KEY ([GroupId]) REFERENCES [FairPlaySocial].[Group]([GroupId])
)

GO

CREATE UNIQUE INDEX [UI_GroupMember_GroupMemberId_MemberApplicationUserId] ON [FairPlaySocial].[GroupMember] ([GroupMemberId], [MemberApplicationUserId])