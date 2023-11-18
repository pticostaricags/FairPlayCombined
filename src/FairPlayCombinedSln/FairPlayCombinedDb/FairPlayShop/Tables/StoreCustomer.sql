CREATE TABLE [FairPlayShop].[StoreCustomer]
(
	[StoreCustomerId] BIGINT NOT NULL CONSTRAINT PK_StoreCustomer PRIMARY KEY IDENTITY, 
    [StoreId] BIGINT NOT NULL, 
    [Name] NVARCHAR(50) NOT NULL, 
    [FirstSurname] NVARCHAR(50) NOT NULL, 
    [SecondSurname] NVARCHAR(50) NOT NULL, 
    [EmailAddress] NVARCHAR(50) NOT NULL, 
    [PhoneNumber] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_StoreCustomer_Store] FOREIGN KEY ([StoreId]) REFERENCES [FairPlayShop].[Store]([StoreId])
)