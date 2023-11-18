CREATE TABLE [FairPlayShop].[ProductStatus]
(
	[ProductStatusId] TINYINT NOT NULL CONSTRAINT PK_ProductStatus PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL
)