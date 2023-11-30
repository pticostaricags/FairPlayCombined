CREATE TABLE [FairPlayTube].[VideoCaptions]
(
	[VideoCaptionsId] BIGINT NOT NULL CONSTRAINT PK_VideoCaptions PRIMARY KEY IDENTITY,
	[VideoInfoId] BIGINT NOT NULL,
    [Content] NVARCHAR(MAX) NOT NULL, 
    [Language] NVARCHAR(10) NOT NULL, 
    CONSTRAINT [FK_VideoCaptions_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo]([VideoInfoId]) 
)
