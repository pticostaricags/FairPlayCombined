CREATE TABLE [FairPlaySocial].[ProfileVisitor]
(
	[ProfileVisitorId] BIGINT NOT NULL CONSTRAINT PK_ProfileVisitor PRIMARY KEY IDENTITY, 
    [VisitorApplicationUserId] NVARCHAR(450) NOT NULL, 
    [VisitedApplicationUserId] NVARCHAR(450) NOT NULL,
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL,
    CONSTRAINT [FK_ProfileVisitor_Visitor_ApplicationUser] FOREIGN KEY ([VisitorApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id]), 
    CONSTRAINT [FK_ProfileVisitor_Visited_ApplicationUser] FOREIGN KEY ([VisitedApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
)