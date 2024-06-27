CREATE TABLE [dbo].[OpenAIPromptCost]
(
	[OpenAIPromptCostId] INT NOT NULL CONSTRAINT PK_OpenAIPromptCost PRIMARY KEY IDENTITY, 
    [CostPerPrompt] MONEY NOT NULL,
	[RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL,
)
