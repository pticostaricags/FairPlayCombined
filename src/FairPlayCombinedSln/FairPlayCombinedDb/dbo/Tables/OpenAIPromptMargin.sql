CREATE TABLE [dbo].[OpenAIPromptMargin]
(
    [OpenAIPromptMarginId] BIGINT NOT NULL CONSTRAINT PK_OpenAIPromptMargin PRIMARY KEY IDENTITY, 
    [Margin] DECIMAL(5,4) NOT NULL,
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL,

    CONSTRAINT BETWEEN_0_AND_1 CHECK ( 0 <= [Margin] AND [Margin] <= 1)
)