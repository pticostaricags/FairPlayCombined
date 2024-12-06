CREATE TABLE [FairPlayBlogs].[BlogSubscriber]
(
	[BlogSubscriberId] BIGINT NOT NULL CONSTRAINT PK_BlogSubscriber PRIMARY KEY IDENTITY, 
	[BlogId] BIGINT NOT NULL,
    [Email] NVARCHAR(256) NOT NULL, 
	[RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL
    CONSTRAINT [FK_BlogSubscriber_Blog] FOREIGN KEY ([BlogId]) REFERENCES [FairPlayBlogs].[Blog]([BlogId]), 
)

GO

CREATE UNIQUE INDEX [UI_BlogSubscriber_BlogId_Email] ON [FairPlayBlogs].[BlogSubscriber] ([BlogId],[Email])
