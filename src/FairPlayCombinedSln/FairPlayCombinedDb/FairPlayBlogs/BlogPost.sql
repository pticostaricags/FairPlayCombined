CREATE TABLE [FairPlayBlogs].[BlogPost]
(
	[BlogPostId] BIGINT NOT NULL CONSTRAINT PK_BlogPost PRIMARY KEY IDENTITY, 
    [BlogId] BIGINT NOT NULL, 
    [Title] NVARCHAR(50) NOT NULL, 
    [PreviewText] NVARCHAR(250) NOT NULL,
    [Content] NVARCHAR(MAX) NOT NULL,
    [ThumbnailPhotoId] BIGINT NOT NULL,
    [BlogPostStatusId] INT NOT NULL,
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL
    CONSTRAINT [FK_BlogPost_Blog] FOREIGN KEY ([BlogId]) REFERENCES [FairPlayBlogs].[Blog]([BlogId]),  
    CONSTRAINT [FK_BlogPost_Photo] FOREIGN KEY ([ThumbnailPhotoId]) REFERENCES [Photo]([PhotoId]), 
    CONSTRAINT [FK_BlogPost_BlogPostStatus] FOREIGN KEY ([BlogPostStatusId]) REFERENCES [FairPlayBlogs].[BlogPostStatus]([BlogPostStatusId]), 
)