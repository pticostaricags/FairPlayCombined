CREATE TABLE [dbo].[UserFunds]
(
	[UserFunds] BIGINT NOT NULL CONSTRAINT PK_UserFunds PRIMARY KEY IDENTITY, 
    [ApplicationUserId] NVARCHAR(450) NOT NULL, 
    [AvailableFunds] MONEY NOT NULL, 
    CONSTRAINT [FK_UserFunds_AspNetUsers] FOREIGN KEY ([ApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
)

GO

CREATE UNIQUE INDEX [UI_UserFunds_ApplicationUserId] ON [dbo].[UserFunds] ([ApplicationUserId])
