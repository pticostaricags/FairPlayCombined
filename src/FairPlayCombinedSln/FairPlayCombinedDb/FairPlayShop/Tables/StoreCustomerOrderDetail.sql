CREATE TABLE [FairPlayShop].[StoreCustomerOrderDetail]
(
	[StoreCustomerOrderDetail] BIGINT NOT NULL CONSTRAINT PK_StoreCustomerOrderDetail PRIMARY KEY IDENTITY, 
    [StoreCustomerOrderId] BIGINT NOT NULL, 
    [ProductId] BIGINT NOT NULL, 
    [UnityPrice] MONEY NOT NULL, 
    [Quantity] MONEY NOT NULL, 
    [LineTotal] MONEY NOT NULL,
    CONSTRAINT [FK_StoreCustomerOrderDetail_StoreCustomerOrder] FOREIGN KEY ([StoreCustomerOrderId]) REFERENCES [FairPlayShop].[StoreCustomerOrder]([StoreCustomerOrderId]), 
    CONSTRAINT [FK_StoreCustomerOrderDetail_Product] FOREIGN KEY ([ProductId]) REFERENCES [FairPlayShop].[Product]([ProductId])
)