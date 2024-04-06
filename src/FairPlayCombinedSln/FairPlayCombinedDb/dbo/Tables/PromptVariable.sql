CREATE TABLE [dbo].[PromptVariable]
(
	[PromptVariableId] INT NOT NULL CONSTRAINT PK_PromptVariable PRIMARY KEY IDENTITY, 
    [PromptId] INT NOT NULL, 
    [VariableName] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_PromptVariable_Prompt] FOREIGN KEY ([PromptId]) REFERENCES [dbo].[Prompt]([PromptId])
)

GO

CREATE UNIQUE INDEX [UI_PromptVariable_PromptId_VariableName] ON [dbo].[PromptVariable] ([PromptId],[VariableName])
