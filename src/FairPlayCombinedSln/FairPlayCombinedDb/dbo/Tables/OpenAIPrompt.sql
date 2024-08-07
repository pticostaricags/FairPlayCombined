﻿CREATE TABLE [dbo].[OpenAIPrompt]
(
	[OpenAIPromptId] BIGINT NOT NULL CONSTRAINT PK_OpenAIPrompt PRIMARY KEY IDENTITY, 
    [OwnerApplicationUserId] NVARCHAR(450) NOT NULL, 
    [OperationCost] MONEY NOT NULL, 
    [OriginalPrompt] NVARCHAR(MAX) NOT NULL, 
    [RevisedPrompt] NVARCHAR(MAX) NULL, 
    [Model] NVARCHAR(50) NOT NULL,
    [GeneratedImageBytes] VARBINARY(MAX) NULL,
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [FK_OpenAIPrompt_AspNetUsers] FOREIGN KEY ([OwnerApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id]), 
)
