CREATE TABLE [dbo].[ContactCompany]
(
	[ContactCompanyId] BIGINT NOT NULL CONSTRAINT PL_ContactCompany PRIMARY KEY IDENTITY, 
    [ContactId] BIGINT NOT NULL, 
    [CompanyId] BIGINT NOT NULL, 
    [JobTitle] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_ContactCompany_Contact] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contact]([ContactId]), 
    CONSTRAINT [FK_ContactCompany_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company]([CompanyId])
)
