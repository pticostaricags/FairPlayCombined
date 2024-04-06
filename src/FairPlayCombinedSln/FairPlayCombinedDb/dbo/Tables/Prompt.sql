CREATE TABLE [dbo].[Prompt]
(
	[PromptId] INT NOT NULL CONSTRAINT PK_Prompt PRIMARY KEY IDENTITY, 
	[PromptName] NVARCHAR(100) NOT NULL,
    [BaseText] NVARCHAR(MAX) NOT NULL, 
)

GO

CREATE UNIQUE INDEX [UI_Prompt_PromptName] ON [dbo].[Prompt] ([PromptName])
