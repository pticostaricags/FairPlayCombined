CREATE TABLE [FairPlayBlogs].[BlogPostStatus]
(
	[BlogPostStatusId] INT NOT NULL CONSTRAINT PK_BlogPostStatus PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(10) NOT NULL
)

GO

CREATE UNIQUE INDEX [UI_BlogPostStatus_Name] ON [FairPlayBlogs].[BlogPostStatus] ([Name])