CREATE TABLE [dbo].[VisitorTracking]
(
	[VisitorTrackingId] BIGINT NOT NULL CONSTRAINT PK_VisitorTracking PRIMARY KEY IDENTITY, 
    [ApplicationUserId] NVARCHAR(450) NULL, 
 [RemoteIpAddress] NVARCHAR(250) NOT NULL, 
    [Country] NVARCHAR(250) NOT NULL, 
    [VisitDateTime] DATETIMEOFFSET NOT NULL, 
    [UserAgent] NVARCHAR(MAX) NOT NULL, 
    [Host] NVARCHAR(MAX) NOT NULL, 
    [VisitedUrl] NVARCHAR(MAX) NOT NULL
    CONSTRAINT [FK_VisitorTracking_ApplicationUser] FOREIGN KEY ([ApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id]), 
    [VideoInfoId] BIGINT NULL, 
    [SessionId] UNIQUEIDENTIFIER NULL, 
    [LastTrackedDateTime] DATETIMEOFFSET NULL, 
    [Referer] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_VisitorTracking_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo]([VideoInfoId])
)

GO

CREATE INDEX [IX_VisitorTracking_VideoId] ON [dbo].[VisitorTracking] ([VideoInfoId])