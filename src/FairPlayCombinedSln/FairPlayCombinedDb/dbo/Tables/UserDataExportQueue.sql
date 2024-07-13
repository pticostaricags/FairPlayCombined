CREATE TABLE [dbo].[UserDataExportQueue](
	[UserDataExportQueueId] BIGINT NOT NULL CONSTRAINT PK_UserDataExportQueue PRIMARY KEY IDENTITY, 
    [ApplicationUserId] NVARCHAR(450) NOT NULL, 
    [IsCompleted] BIT NOT NULL,
    [FileUrl] NVARCHAR(MAX) NULL, 
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [FK_UserDataExportQueue_AspNetUsers] FOREIGN KEY ([ApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id]), 
)

GO

CREATE INDEX [IX_UserDataExportQueue_ApplicationUserId] ON [dbo].[UserDataExportQueue] ([ApplicationUserId])
