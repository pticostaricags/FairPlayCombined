CREATE TABLE [FairPlayTube].[VideoPassiveIncome]
(
	[VideoPassiveIncomeIdeaId] BIGINT NOT NULL CONSTRAINT PK_VideoPassiveIncome PRIMARY KEY IDENTITY, 
    [VideoInfoId] BIGINT NOT NULL, 
    [HtmlPassiveIncomeIdea] NVARCHAR(MAX) NOT NULL, 
    [OpenAIPromptId] BIGINT NOT NULL, 
    CONSTRAINT [FK_VideoPassiveIncome_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo]([VideoInfoId]), 
    CONSTRAINT [FK_VideoPassiveIncome_OpenAIPrompt] FOREIGN KEY ([OpenAIPromptId]) REFERENCES [dbo].[OpenAIPrompt]([OpenAIPromptId])
)
