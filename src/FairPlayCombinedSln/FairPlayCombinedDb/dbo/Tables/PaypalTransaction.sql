CREATE TABLE [dbo].[PaypalTransaction]
(
	[PaypalTransactionId] BIGINT NOT NULL CONSTRAINT PK_PaypalTransaction PRIMARY KEY IDENTITY, 
    [OrderId] NVARCHAR(50) NOT NULL, 
    [OrderAmount] MONEY NOT NULL, 
    [ApplicationUserId] NVARCHAR(450) NOT NULL,
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [FK_PaypalTransaction_AspNetUsers] FOREIGN KEY ([ApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
)

GO

CREATE UNIQUE INDEX [UI_PaypalTransaction_OrderId] ON [dbo].[PaypalTransaction] ([OrderId])