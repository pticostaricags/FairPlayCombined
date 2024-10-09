CREATE TABLE [dbo].[CompanyContact]
(
	[CompanyContactId] BIGINT NOT NULL CONSTRAINT PK_CompanyContact PRIMARY KEY IDENTITY, 
    [CompanyId] BIGINT NOT NULL, 
    [ContactId] BIGINT NOT NULL, 
    [IsPrimaryContact] BIT NOT NULL, 
    CONSTRAINT [FK_CompanyContact_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company]([CompanyId]), 
    CONSTRAINT [FK_CompanyContact_Contactt] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contact]([ContactId])
)

GO

CREATE UNIQUE INDEX [UI_CompanyContact_CompanyId_ContactId] ON [dbo].[CompanyContact] ([CompanyId],[ContactId])

GO

CREATE UNIQUE INDEX [UI_CompanyContact_CompanyId_IsPrimaryContact] ON [dbo].[CompanyContact] ([CompanyId],[IsPrimaryContact])
