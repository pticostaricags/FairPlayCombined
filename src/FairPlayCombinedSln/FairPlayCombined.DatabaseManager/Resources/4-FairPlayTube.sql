BEGIN TRANSACTION
--START OF DEFAULT VIDEO INDEX STATUSES
DECLARE @VIDEO_INDEX_STATUS NVARCHAR(50) = 'Pending'
IF NOT EXISTS (SELECT * FROM [FairPlayTube].[VideoIndexStatus] VIS WHERE [VIS].[Name] = @VIDEO_INDEX_STATUS)
BEGIN
    INSERT INTO [FairPlayTube].[VideoIndexStatus]([VideoIndexStatusId],[Name]) VALUES(0, @VIDEO_INDEX_STATUS)
END
SET @VIDEO_INDEX_STATUS = 'Processing'
IF NOT EXISTS (SELECT * FROM [FairPlayTube].[VideoIndexStatus] VIS WHERE [VIS].[Name] = @VIDEO_INDEX_STATUS)
BEGIN
    INSERT INTO [FairPlayTube].[VideoIndexStatus]([VideoIndexStatusId],[Name]) VALUES(1, @VIDEO_INDEX_STATUS)
END
SET @VIDEO_INDEX_STATUS = 'Processed'
IF NOT EXISTS (SELECT * FROM [FairPlayTube].[VideoIndexStatus] VIS WHERE [VIS].[Name] = @VIDEO_INDEX_STATUS)
BEGIN
    INSERT INTO [FairPlayTube].[VideoIndexStatus]([VideoIndexStatusId],[Name]) VALUES(2, @VIDEO_INDEX_STATUS)
END
SET @VIDEO_INDEX_STATUS = 'Deleted'
IF NOT EXISTS (SELECT * FROM [FairPlayTube].[VideoIndexStatus] VIS WHERE [VIS].[Name] = @VIDEO_INDEX_STATUS)
BEGIN
    INSERT INTO [FairPlayTube].[VideoIndexStatus]([VideoIndexStatusId],[Name]) VALUES(3, @VIDEO_INDEX_STATUS)
END
--END OF DEFAULT VIDEO INDEX STATUSES
--START OF DEFAULT VIDEO VISIBILITY CATEGORIES
IF NOT EXISTS (SELECT * FROM [FairPlayTube].[VideoVisibility] WHERE [Name] = 'Public')
BEGIN
	INSERT INTO [FairPlayTube].VideoVisibility([VideoVisibilityId],[Name]) VALUES(1,'Public')
END
IF NOT EXISTS (SELECT * FROM [FairPlayTube].[VideoVisibility] WHERE [Name] = 'Private')
BEGIN
	INSERT INTO [FairPlayTube].VideoVisibility([VideoVisibilityId],[Name]) VALUES(2,'Private')
END
--END OF DEFAULT VIDEO VISIBILITY CATEGORIES

--IORIGINATOR INFO
DECLARE @ORIGINATORIPADDRESS NVARCHAR(100) = CONVERT(NVARCHAR(100), ISNULL(CONNECTIONPROPERTY('client_net_address'), ''))
DECLARE @SOURCEAPPLICATION NVARCHAR(250) = APP_NAME()
DECLARE @ROWCREATIONUSER NVARCHAR(256) = 'Post Deployment Script'

--START OF DEFAULT VIDEO INDEXING COSTS
SET IDENTITY_INSERT [FairPlayTube].[VideoIndexingCost] ON
DECLARE @COST_PER_MINUTE MONEY = 0.21
IF NOT EXISTS (SELECT 1 FROM [FairPlayTube].[VideoIndexingCost])
BEGIN 
    INSERT INTO [FairPlayTube].[VideoIndexingCost]([VideoIndexingCostId],[CostPerMinute],[RowCreationDateTime],[RowCreationUser],[SourceApplication],[OriginatorIPAddress]) 
    VALUES(1, @COST_PER_MINUTE, SYSUTCDATETIME(), @ROWCREATIONUSER, @SOURCEAPPLICATION, @ORIGINATORIPADDRESS)
END
SET IDENTITY_INSERT [FairPlayTube].[VideoIndexingCost] OFF
--END OF DEFAULT VIDEO INDEXING COSTS

--START OF DEFAULT VIDEO INDEXING MARGINS
SET IDENTITY_INSERT [FairPlayTube].[VideoIndexingMargin] ON
DECLARE @MARGIN DECIMAL(5,4) = 0.5
IF NOT EXISTS (SELECT 1 FROM [FairPlayTube].[VideoIndexingMargin])
BEGIN 
    INSERT INTO [FairPlayTube].[VideoIndexingMargin]([VideoIndexingMarginId],[Margin],[RowCreationDateTime],[RowCreationUser],[SourceApplication],[OriginatorIPAddress]) 
    VALUES(1, @MARGIN, SYSUTCDATETIME(), @ROWCREATIONUSER, @SOURCEAPPLICATION, @ORIGINATORIPADDRESS)
END
SET IDENTITY_INSERT [FairPlayTube].[VideoIndexingMargin] OFF
--END OF DEFAULT VIDEO INDEXING MARGINS
--START OF VIDEO JOB APPLICATION STATUS
DECLARE @VIDEOJOBAPPLICATIONSTATUSNAME NVARCHAR(50) = 'New'
IF NOT EXISTS (SELECT * FROM [FairPlayTube].[VideoJobApplicationStatus] WHERE [Name] = @VIDEOJOBAPPLICATIONSTATUSNAME)
BEGIN
    INSERT INTO FairPlayTube.VideoJobApplicationStatus([VideoJobApplicationStatusId],[Name],[Description]) VALUES(1, @VIDEOJOBAPPLICATIONSTATUSNAME, 'Application is new')
END
SET @VIDEOJOBAPPLICATIONSTATUSNAME = 'Selected'
IF NOT EXISTS (SELECT * FROM [FairPlayTube].[VideoJobApplicationStatus] WHERE [Name] = @VIDEOJOBAPPLICATIONSTATUSNAME)
BEGIN
    INSERT INTO FairPlayTube.VideoJobApplicationStatus([VideoJobApplicationStatusId],[Name], [Description]) VALUES(2, @VIDEOJOBAPPLICATIONSTATUSNAME, 'Application has been selected')
END
SET @VIDEOJOBAPPLICATIONSTATUSNAME = 'Not Selected'
IF NOT EXISTS (SELECT * FROM [FairPlayTube].[VideoJobApplicationStatus] WHERE [Name] = @VIDEOJOBAPPLICATIONSTATUSNAME)
BEGIN
    INSERT INTO FairPlayTube.VideoJobApplicationStatus([VideoJobApplicationStatusId],[Name],[Description]) VALUES(3, @VIDEOJOBAPPLICATIONSTATUSNAME, 'Application has not been selected')
END
SET @VIDEOJOBAPPLICATIONSTATUSNAME = 'Pending Payment'
IF NOT EXISTS (SELECT * FROM [FairPlayTube].[VideoJobApplicationStatus] WHERE [Name] = @VIDEOJOBAPPLICATIONSTATUSNAME)
BEGIN
    INSERT INTO FairPlayTube.VideoJobApplicationStatus([VideoJobApplicationStatusId],[Name],[Description]) VALUES(4, @VIDEOJOBAPPLICATIONSTATUSNAME, 'Work has been performed. Payment is pending')
END
SET @VIDEOJOBAPPLICATIONSTATUSNAME = 'Paid'
IF NOT EXISTS (SELECT * FROM [FairPlayTube].[VideoJobApplicationStatus] WHERE [Name] = @VIDEOJOBAPPLICATIONSTATUSNAME)
BEGIN
    INSERT INTO FairPlayTube.VideoJobApplicationStatus([VideoJobApplicationStatusId],[Name],[Description]) VALUES(5, @VIDEOJOBAPPLICATIONSTATUSNAME, 'Work has been performed and it is already paid')
END
--END OF VIDEO JOB APPLICATION STATUS
COMMIT