CREATE TABLE [dbo].[OpenAIPrompt]
(
	[OpenAIPromptId] BIGINT NOT NULL CONSTRAINT PK_OpenAIPrompt PRIMARY KEY IDENTITY, 
    [OriginalPrompt] NVARCHAR(MAX) NOT NULL, 
    [RevisedPrompt] NVARCHAR(MAX) NOT NULL, 
    [Model] NVARCHAR(50) NOT NULL,
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL
)
