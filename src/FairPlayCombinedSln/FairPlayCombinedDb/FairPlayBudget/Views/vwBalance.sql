CREATE VIEW [FairPlayBudget].[vwBalance]
AS 
SELECT 
E.OwnerId AS OwnerId, 
E.Amount AS Amount,
CAST('Debit' AS NVARCHAR(10)) AS TransactionType,
E.ExpenseDateTime AS [DateTime],
E.[Description] AS [Description],
C.CurrencyId AS CurrencyId,
C.[Description] AS Currency,
MBI.[Description] AS MonthlyBudgetDescription
FROM [FairPlayBudget].[Expense] E
LEFT JOIN [FairPlayBudget].MonthlyBudgetInfo MBI ON MBI.MonthlyBudgetInfoId = E.MonthlyBudgetInfoId
INNER JOIN [FairPlayBudget].Currency C ON C.CurrencyId = E.CurrencyId
UNION ALL
SELECT 
I.OwnerId AS OwnerId, 
I.Amount AS Amount,
CAST('Credit' AS NVARCHAR(10)) AS TransactionType,
I.IncomeDateTime AS [DateTime],
I.[Description] AS [Description],
I.CurrencyId AS CurrencyId,
C.[Description] AS Currency,
MBI.[Description] AS MonthlyBudgetDescription
FROM [FairPlayBudget].[Income] I
LEFT JOIN [FairPlayBudget].MonthlyBudgetInfo MBI ON MBI.MonthlyBudgetInfoId = I.MonthlyBudgetInfoId
INNER JOIN [FairPlayBudget].Currency C ON C.CurrencyId = I.CurrencyId