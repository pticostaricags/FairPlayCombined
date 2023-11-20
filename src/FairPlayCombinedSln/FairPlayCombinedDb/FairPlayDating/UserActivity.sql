CREATE TABLE [FairPlayDating].[UserActivity]
(
	[UserActivityId] BIGINT NOT NULL  CONSTRAINT PK_UserActivity PRIMARY KEY IDENTITY, 
    [ApplicationUserId] NVARCHAR(450) NOT NULL, 
    [ActivityId] INT NOT NULL, 
    [FrequencyId] INT NOT NULL, 
    CONSTRAINT [FK_UserActivity_ApplicationUser] FOREIGN KEY ([ApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id]), 
    CONSTRAINT [FK_UserActivity_Activity] FOREIGN KEY ([ActivityId]) REFERENCES [FairPlayDating].[Activity]([ActivityId]), 
    CONSTRAINT [FK_UserActivity_Frequency] FOREIGN KEY ([FrequencyId]) REFERENCES [FairPlayDating].[Frequency]([FrequencyId])
)

GO

CREATE UNIQUE INDEX [UI_UserActivity_ApplicationUserId_ActivityId] ON [FairPlayDating].[UserActivity] ([ApplicationUserId], [ActivityId])