CREATE TABLE [dbo].[VideoJobApplication]
(
	[VideoJobApplicationId] BIGINT NOT NULL CONSTRAINT PK_VideoJobApplication PRIMARY KEY IDENTITY,
	[VideoJobId] BIGINT NOT NULL CONSTRAINT FK_VideoJobApplication_VideoJobId REFERENCES [FairPlayTube].[VideoJob]([VideoJobId]),
	[ApplicantApplicationUserId] NVARCHAR(450) NOT NULL CONSTRAINT FK_VideoJobApplication_ApplicationUser REFERENCES [dbo].[AspNetUsers]([Id]),
	[ApplicantCoverLetter] TEXT NOT NULL,
	[VideoJobApplicationStatusId] SMALLINT NOT NULL, 
	[RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [FK_VideoJobApplication_VideoJobApplicationStatus] FOREIGN KEY ([VideoJobApplicationStatusId]) REFERENCES [FairPlayTube].[VideoJobApplicationStatus]([VideoJobApplicationStatusId]), 
)

GO