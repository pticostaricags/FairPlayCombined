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
--START OF DEFAULT POST VISIBILITY
SET IDENTITY_INSERT [FairPlaySocial].[PostVisibility] ON
DECLARE @POST_VISIBILITY_ID SMALLINT = 1
IF NOT EXISTS (SELECT * FROM [FairPlaySocial].[PostVisibility] PV WHERE [PV].[Name] = 'Public')
BEGIN
    INSERT INTO [FairPlaySocial].[PostVisibility]([PostVisibilityId],[Name],[Description])
    VALUES (@POST_VISIBILITY_ID, 'Public', 'Visible to everyone')
END
SET @POST_VISIBILITY_ID = 2
IF NOT EXISTS (SELECT * FROM [FairPlaySocial].[PostVisibility] PV WHERE [PV].[Name] = 'Subscribers')
BEGIN
    INSERT INTO [FairPlaySocial].[PostVisibility]([PostVisibilityId],[Name],[Description])
    VALUES (@POST_VISIBILITY_ID, 'Subscribers', 'Visible to subscribers only')
END
SET IDENTITY_INSERT [FairPlaySocial].[PostVisibility] OFF
--END OF DEFAULT POST VISIBILITY
--START OF DEFAULT POST TYPE
SET IDENTITY_INSERT [FairPlaySocial].[PostType] ON
DECLARE @POST_TYPE_ID TiNYINT = 1
IF NOT EXISTS (SELECT * FROM [FairPlaySocial].[PostType] PV WHERE [PV].[Name] = 'Post')
BEGIN
    INSERT INTO [FairPlaySocial].[PostType]([PostTypeId],[Name])
    VALUES (@POST_TYPE_ID, 'Post')
END
SET @POST_TYPE_ID = 2
IF NOT EXISTS (SELECT * FROM [FairPlaySocial].[PostType] PV WHERE [PV].[Name] = 'Comment')
BEGIN
    INSERT INTO [FairPlaySocial].[PostType]([PostTypeId],[Name])
    VALUES (@POST_TYPE_ID, 'Comment')
END
SET IDENTITY_INSERT [FairPlaySocial].[PostType] OFF
--END OF DEFAULT POST TYPE
COMMIT