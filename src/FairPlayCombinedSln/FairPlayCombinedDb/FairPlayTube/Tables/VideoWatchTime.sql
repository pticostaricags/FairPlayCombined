CREATE TABLE [FairPlayTube].[VideoWatchTime]
(
	[VideoWatchTimeId] BIGINT NOT NULL CONSTRAINT PK_VideoWatchTime PRIMARY KEY IDENTITY, 
    [VideoInfoId] BIGINT NOT NULL, 
    [SessionId] UNIQUEIDENTIFIER NOT NULL, 
    [SessionStartDatetime] DATETIMEOFFSET NOT NULL, 
    [WatchTime] FLOAT NOT NULL, 
    [WatchedByApplicationUserId] NVARCHAR(450) NULL, 
    CONSTRAINT [FK_VideoWatchTime_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo]([VideoInfoId]), 
    CONSTRAINT [FK_VideoWatchTime_AspNetUsers] FOREIGN KEY ([WatchedByApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
)

GO

CREATE UNIQUE INDEX [UI_VideoWatchTime_SessionId] ON [FairPlayTube].[VideoWatchTime] ([SessionId])

GO

CREATE UNIQUE INDEX [UI_VideoWatchTime_VideoInfoId_SessionId] ON [FairPlayTube].[VideoWatchTime] ([VideoInfoId],[SessionId])
