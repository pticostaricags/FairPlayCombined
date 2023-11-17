CREATE TABLE [FairPlayShop].[Store] (
    [StoreId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (50)  NOT NULL,
    [OwnerId] NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_Store] PRIMARY KEY CLUSTERED ([StoreId] ASC),
    CONSTRAINT [FK_Store_AspNetUsers] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);

