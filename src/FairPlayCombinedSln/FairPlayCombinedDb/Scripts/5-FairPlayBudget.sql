BEGIN TRANSACTION
IF NOT EXISTS(
SELECT * FROM [FairPlayBudget].Currency C WHERE C.CurrencyId = 1
)
BEGIN
    SET IDENTITY_INSERT [FairPlayBudget].Currency ON;
    INSERT INTO [FairPlayBudget].Currency(CurrencyId, [Description]) VALUES(1,'USD')
    SET IDENTITY_INSERT [FairPlayBudget].Currency OFF;
END
IF NOT EXISTS(
SELECT * FROM [FairPlayBudget].Currency C WHERE C.CurrencyId = 2
)
BEGIN
    SET IDENTITY_INSERT [FairPlayBudget].Currency ON;
    INSERT INTO [FairPlayBudget].Currency(CurrencyId, [Description]) VALUES(2,'CRC')
    SET IDENTITY_INSERT [FairPlayBudget].Currency OFF;
END
COMMIT