/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
BEGIN TRANSACTION
--START OF DEFAULT CULTURES
SET IDENTITY_INSERT [dbo].[Culture] ON
DECLARE @CULTURE NVARCHAR(50) = 'en-US'
IF NOT EXISTS (SELECT * FROM [dbo].[Culture] WHERE [Name] = @CULTURE)
BEGIN
    INSERT INTO Culture([CultureId],[Name]) VALUES(1, @CULTURE)
END
SET @CULTURE='es-CR'
IF NOT EXISTS (SELECT * FROM [dbo].[Culture] WHERE [Name] = @CULTURE)
BEGIN
    INSERT INTO Culture([CultureId],[Name]) VALUES(2, @CULTURE)
END
SET @CULTURE='fr-CA'
IF NOT EXISTS (SELECT * FROM [dbo].[Culture] WHERE [Name] = @CULTURE)
BEGIN
    INSERT INTO Culture([CultureId],[Name]) VALUES(3, @CULTURE)
END
SET @CULTURE='it'
IF NOT EXISTS (SELECT * FROM [dbo].[Culture] WHERE [Name] = @CULTURE)
BEGIN
    INSERT INTO Culture([CultureId],[Name]) VALUES(4, @CULTURE)
END
SET @CULTURE='pt-BR'
IF NOT EXISTS (SELECT * FROM [dbo].[Culture] WHERE [Name] = @CULTURE)
BEGIN
    INSERT INTO Culture([CultureId],[Name]) VALUES(5, @CULTURE)
END
SET @CULTURE='la-VA'
IF NOT EXISTS (SELECT * FROM [dbo].[Culture] WHERE [Name] = @CULTURE)
BEGIN
    INSERT INTO Culture([CultureId],[Name]) VALUES(6, @CULTURE)
END
SET IDENTITY_INSERT [dbo].[Culture] OFF
--END OF DEFAULT CULTURES
COMMIT