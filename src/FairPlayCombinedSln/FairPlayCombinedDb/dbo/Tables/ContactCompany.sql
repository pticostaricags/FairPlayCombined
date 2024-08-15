CREATE TABLE [dbo].[ContactCompany]
(
	[ContactCompanyId] BIGINT NOT NULL CONSTRAINT PL_ContactCompany PRIMARY KEY IDENTITY, 
    [ContactId] BIGINT NOT NULL, 
    [CompanyId] BIGINT NOT NULL, 
    [JobTitle] NVARCHAR(50) NOT NULL, 
    [EmailAddress] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_ContactCompany_Contact] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contact]([ContactId]), 
    CONSTRAINT [FK_ContactCompany_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company]([CompanyId])
)

GO

CREATE UNIQUE INDEX [IX_ContactCompany_ContactId_CompanyId] ON [dbo].[ContactCompany] ([ContactId],[CompanyId])
