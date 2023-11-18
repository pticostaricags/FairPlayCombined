CREATE TABLE [FairPlayShop].[StoreCustomerOrder]
(
	[StoreCustomerOrderId] BIGINT NOT NULL CONSTRAINT PK_StoreCustomerOrder PRIMARY KEY IDENTITY, 
    [StoreCustomerId] BIGINT NOT NULL, 
    [OrderDateTime] DATETIMEOFFSET NOT NULL, 
    [OrderSubTotal] MONEY NOT NULL, 
    [TaxTotal] MONEY NOT NULL, 
    [OrderTotal] MONEY NOT NULL, 
    CONSTRAINT [FK_StoreCustomerOrder_StoreCustomer] FOREIGN KEY ([StoreCustomerId]) REFERENCES [FairPlayShop].[StoreCustomer]([StoreCustomerId])
)