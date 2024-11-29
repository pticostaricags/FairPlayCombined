CREATE TABLE [FairPlayBlogs].[Blog]
(
	[BlogId] BIGINT NOT NULL CONSTRAINT PK_Blog PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(500) NOT NULL, 
    [HeaderPhotoId] BIGINT NOT NULL,
    [CustomDomain] NVARCHAR(100) NULL,
    [IsCustomDomainVerified] BIT NOT NULL,
    [OwnerApplicationUserId] NVARCHAR(450) NOT NULL,
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [FK_Blog_AspNetUsers] FOREIGN KEY ([OwnerApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id]), 
    CONSTRAINT [FK_Blog_Photo] FOREIGN KEY ([HeaderPhotoId]) REFERENCES [dbo].[Photo]([PhotoId])
     
)

GO

CREATE UNIQUE INDEX [UI_Blog_Name] ON [FairPlayBlogs].[Blog] ([Name])

GO

CREATE UNIQUE INDEX [UI_Blog_Url] ON [FairPlayBlogs].[Blog] ([CustomDomain])