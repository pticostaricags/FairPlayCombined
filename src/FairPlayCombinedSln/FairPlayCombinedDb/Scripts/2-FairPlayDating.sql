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
GO
BEGIN TRANSACTION
--START OF DEFAULT FREQUENCY
SET IDENTITY_INSERT [FairPlayDating].[Frequency] ON
DECLARE @FREQUENCYNAME NVARCHAR(50) = 'Never'
DECLARE @FREQUENCYID INT = 1
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[Frequency] F WHERE [F].[Name] = @FREQUENCYNAME)
BEGIN
    INSERT INTO [FairPlayDating].[Frequency]([FrequencyId], [Name]) VALUES(@FREQUENCYID, @FREQUENCYNAME)
END
SET @FREQUENCYNAME = '1 day per week'
SET @FREQUENCYID = 2
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[Frequency] F WHERE [F].[Name] = @FREQUENCYNAME)
BEGIN
    INSERT INTO [FairPlayDating].[Frequency]([FrequencyId], [Name]) VALUES(@FREQUENCYID, @FREQUENCYNAME)
END
SET @FREQUENCYNAME = '2 days per week'
SET @FREQUENCYID = 3
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[Frequency] F WHERE [F].[Name] = @FREQUENCYNAME)
BEGIN
    INSERT INTO [FairPlayDating].[Frequency]([FrequencyId], [Name]) VALUES(@FREQUENCYID, @FREQUENCYNAME)
END
SET @FREQUENCYNAME = '3 days per week'
SET @FREQUENCYID = 4
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[Frequency] F WHERE [F].[Name] = @FREQUENCYNAME)
BEGIN
    INSERT INTO [FairPlayDating].[Frequency]([FrequencyId], [Name]) VALUES(@FREQUENCYID, @FREQUENCYNAME)
END
SET @FREQUENCYNAME = '4 days per week'
SET @FREQUENCYID = 5
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[Frequency] F WHERE [F].[Name] = @FREQUENCYNAME)
BEGIN
    INSERT INTO [FairPlayDating].[Frequency]([FrequencyId], [Name]) VALUES(@FREQUENCYID, @FREQUENCYNAME)
END
SET @FREQUENCYNAME = '5 days per week'
SET @FREQUENCYID = 6
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[Frequency] F WHERE [F].[Name] = @FREQUENCYNAME)
BEGIN
    INSERT INTO [FairPlayDating].[Frequency]([FrequencyId], [Name]) VALUES(@FREQUENCYID, @FREQUENCYNAME)
END
SET @FREQUENCYNAME = '6 days per week'
SET @FREQUENCYID = 7
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[Frequency] F WHERE [F].[Name] = @FREQUENCYNAME)
BEGIN
    INSERT INTO [FairPlayDating].[Frequency]([FrequencyId], [Name]) VALUES(@FREQUENCYID, @FREQUENCYNAME)
END
SET @FREQUENCYNAME = '7 days per week'
SET @FREQUENCYID = 8
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[Frequency] F WHERE [F].[Name] = @FREQUENCYNAME)
BEGIN
    INSERT INTO [FairPlayDating].[Frequency]([FrequencyId], [Name]) VALUES(@FREQUENCYID, @FREQUENCYNAME)
END
SET IDENTITY_INSERT [FairPlayDating].[Frequency] OFF
--END OF DEFAULT FREQUENCY
--START OF DEFAULT ACTIVITY
SET IDENTITY_INSERT [FairPlayDating].[Activity] ON
DECLARE @ACTIVITYNAME NVARCHAR(50) = 'Excercise'
DECLARE @ACTIVITYID INT = 1
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[Activity] A WHERE [A].[Name] = @ACTIVITYNAME)
BEGIN
    INSERT INTO [FairPlayDating].[Activity]([ActivityId],[Name]) VALUES(@ACTIVITYID, @ACTIVITYNAME)
END
SET @ACTIVITYNAME = 'Smoking'
SET @ACTIVITYID = 2
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[Activity] A WHERE [A].[Name] = @ACTIVITYNAME)
BEGIN
    INSERT INTO [FairPlayDating].[Activity]([ActivityId],[Name]) VALUES(@ACTIVITYID, @ACTIVITYNAME)
END
SET @ACTIVITYNAME = 'Drinking'
SET @ACTIVITYID = 3
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[Activity] A WHERE [A].[Name] = @ACTIVITYNAME)
BEGIN
    INSERT INTO [FairPlayDating].[Activity]([ActivityId],[Name]) VALUES(@ACTIVITYID, @ACTIVITYNAME)
END
SET @ACTIVITYNAME = 'Swimming'
SET @ACTIVITYID = 4
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[Activity] A WHERE [A].[Name] = @ACTIVITYNAME)
BEGIN
    INSERT INTO [FairPlayDating].[Activity]([ActivityId],[Name]) VALUES(@ACTIVITYID, @ACTIVITYNAME)
END
SET @ACTIVITYNAME = 'Road Bike'
SET @ACTIVITYID = 5
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[Activity] A WHERE [A].[Name] = @ACTIVITYNAME)
BEGIN
    INSERT INTO [FairPlayDating].[Activity]([ActivityId],[Name]) VALUES(@ACTIVITYID, @ACTIVITYNAME)
END
SET @ACTIVITYNAME = 'Mountain Bike'
SET @ACTIVITYID = 6
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[Activity] A WHERE [A].[Name] = @ACTIVITYNAME)
BEGIN
    INSERT INTO [FairPlayDating].[Activity]([ActivityId],[Name]) VALUES(@ACTIVITYID, @ACTIVITYNAME)
END
SET @ACTIVITYNAME = 'Play Volleyball'
SET @ACTIVITYID = 7
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[Activity] A WHERE [A].[Name] = @ACTIVITYNAME)
BEGIN
    INSERT INTO [FairPlayDating].[Activity]([ActivityId],[Name]) VALUES(@ACTIVITYID, @ACTIVITYNAME)
END
SET @ACTIVITYNAME = 'Play Tennis'
SET @ACTIVITYID = 8
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[Activity] A WHERE [A].[Name] = @ACTIVITYNAME)
BEGIN
    INSERT INTO [FairPlayDating].[Activity]([ActivityId],[Name]) VALUES(@ACTIVITYID, @ACTIVITYNAME)
END
SET @ACTIVITYNAME = 'Bowling'
SET @ACTIVITYID = 9
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[Activity] A WHERE [A].[Name] = @ACTIVITYNAME)
BEGIN
    INSERT INTO [FairPlayDating].[Activity]([ActivityId],[Name]) VALUES(@ACTIVITYID, @ACTIVITYNAME)
END
SET @ACTIVITYNAME = 'Play Soccer'
SET @ACTIVITYID = 10
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[Activity] A WHERE [A].[Name] = @ACTIVITYNAME)
BEGIN
    INSERT INTO [FairPlayDating].[Activity]([ActivityId],[Name]) VALUES(@ACTIVITYID, @ACTIVITYNAME)
END
SET IDENTITY_INSERT [FairPlayDating].[Activity] OFF
--END OF DEFAULT ACTIVITY
--START OF DEFAULT GENDERS
DECLARE @MALE VARCHAR(20) = 'Male'
DECLARE @MALE_ID INT = 1
DECLARE @FEMALE VARCHAR(20) = 'Female'
DECLARE @FEMALE_ID INT = 2

IF NOT EXISTS (SELECT 1 FROM [FairPlayDating].[Gender] WHERE [Name] = @MALE)
BEGIN
    INSERT INTO [FairPlayDating].[Gender]([GenderId],[Name]) VALUES (@MALE_ID,@MALE)
END

IF NOT EXISTS (SELECT 1 FROM [FairPlayDating].[Gender] WHERE [Name] = @FEMALE)
BEGIN
    INSERT INTO [FairPlayDating].[Gender]([GenderId],[Name]) VALUES (@FEMALE_ID,@FEMALE)
END
--END OF DEFAULT GENDERS
--START OF DEFAULT RELIGIONS
DECLARE @Catholic VARCHAR(20) = 'Catholic'
DECLARE @Catholic_ID INT = 1
DECLARE @Christian VARCHAR(20) = 'Christian'
DECLARE @Christian_ID INT = 2
DECLARE @Jewish VARCHAR(20) = 'Jewish'
DECLARE @Jewish_ID INT = 3
DECLARE @Muslim VARCHAR(20) = 'Muslim'
DECLARE @Muslim_ID INT = 4
DECLARE @Other VARCHAR(20) = 'Other'
DECLARE @Other_ID INT = 5

IF NOT EXISTS (SELECT 1 FROM [FairPlayDating].[Religion] WHERE [Name] = @Catholic)
BEGIN
    INSERT INTO [FairPlayDating].[Religion]([ReligionId],[Name]) VALUES (@Catholic_ID,@Catholic)
END

IF NOT EXISTS (SELECT 1 FROM [FairPlayDating].[Religion] WHERE [Name] = @Christian)
BEGIN
    INSERT INTO [FairPlayDating].[Religion]([ReligionId],[Name]) VALUES (@Christian_ID,@Christian)
END

IF NOT EXISTS (SELECT 1 FROM [FairPlayDating].[Religion] WHERE [Name] = @Jewish)
BEGIN
    INSERT INTO [FairPlayDating].[Religion]([ReligionId],[Name]) VALUES (@Jewish_ID,@Jewish)
END

IF NOT EXISTS (SELECT 1 FROM [FairPlayDating].[Religion] WHERE [Name] = @Muslim)
BEGIN
    INSERT INTO [FairPlayDating].[Religion]([ReligionId],[Name]) VALUES (@Muslim_ID,@Muslim)
END

IF NOT EXISTS (SELECT 1 FROM [FairPlayDating].[Religion] WHERE [Name] = @Other)
BEGIN
    INSERT INTO [FairPlayDating].[Religion]([ReligionId],[Name]) VALUES (@Other_ID,@Other)
END
--END OF DEFAULT RELIGIONS
--START OF DEFAULT DATEOBJECTIVE
DECLARE @FRIENDSHIP VARCHAR(20) = 'Friendship'
DECLARE @FRIENDSHIP_ID INT = 1
DECLARE @DATING VARCHAR(20) = 'Dating'
DECLARE @DATING_ID INT = 2
DECLARE @MARRIAGE VARCHAR(20) = 'Marriage'
DECLARE @MARRIAGE_ID INT = 3

IF NOT EXISTS (SELECT 1 FROM [FairPlayDating].[DateObjective] WHERE [Name] = @FRIENDSHIP)
BEGIN
    INSERT INTO [FairPlayDating].[DateObjective]([DateObjectiveId],[Name]) VALUES (@FRIENDSHIP_ID,@FRIENDSHIP)
END

IF NOT EXISTS (SELECT 1 FROM [FairPlayDating].[DateObjective] WHERE [Name] = @DATING)
BEGIN
    INSERT INTO [FairPlayDating].[DateObjective]([DateObjectiveId],[Name]) VALUES (@DATING_ID,@DATING)
END

IF NOT EXISTS (SELECT 1 FROM [FairPlayDating].[DateObjective] WHERE [Name] = @MARRIAGE)
BEGIN
    INSERT INTO [FairPlayDating].[DateObjective]([DateObjectiveId],[Name]) VALUES (@MARRIAGE_ID,@MARRIAGE)
END
SET IDENTITY_INSERT [FairPlayDating].[Activity] OFF

--END OF DEFAULT DATEOBJECTIVE
--START OF DEFAULT KIDSTATUS
SET IDENTITY_INSERT [FairPlayDating].[KidStatus] ON
DECLARE @KIDSTATUSNAME NVARCHAR(50) = 'Don''t have & Don''t want'
DECLARE @KIDSTATUSID INT = 1
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[KidStatus] k WHERE [K].[Name] = @KIDSTATUSNAME)
BEGIN
    INSERT INTO [FairPlayDating].[KidStatus]([KidStatusId],[Name]) VALUES(@KIDSTATUSID, @KIDSTATUSNAME)
END
SET @KIDSTATUSNAME = 'Don''t have & Want'
SET @KIDSTATUSID = 2
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[KidStatus] k WHERE [K].[Name] = @KIDSTATUSNAME)
BEGIN
    INSERT INTO [FairPlayDating].[KidStatus]([KidStatusId],[Name]) VALUES(@KIDSTATUSID, @KIDSTATUSNAME)
END
SET @KIDSTATUSNAME = 'Have & Don''t want more'
SET @KIDSTATUSID = 3
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[KidStatus] k WHERE [K].[Name] = @KIDSTATUSNAME)
BEGIN
    INSERT INTO [FairPlayDating].[KidStatus]([KidStatusId],[Name]) VALUES(@KIDSTATUSID, @KIDSTATUSNAME)
END
SET @KIDSTATUSNAME = 'Have & Want more'
SET @KIDSTATUSID = 4
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[KidStatus] k WHERE [K].[Name] = @KIDSTATUSNAME)
BEGIN
    INSERT INTO [FairPlayDating].[KidStatus]([KidStatusId],[Name]) VALUES(@KIDSTATUSID, @KIDSTATUSNAME)
END
SET IDENTITY_INSERT [FairPlayDating].[KidStatus] OFF
--END OF DEFAULT KIDSTATUS
--START OF DEFAULT HAIRCOLOR
SET IDENTITY_INSERT [FairPlayDating].[HairColor] ON
DECLARE @HAIRCOLORNAME NVARCHAR(50) = 'Black'
DECLARE @HAIRCOLORID INT = 1
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[HairColor] h WHERE [H].[Name] = @HAIRCOLORNAME)
BEGIN
	INSERT INTO [FairPlayDating].[HairColor]([HairColorId],[Name]) VALUES(@HAIRCOLORID, @HAIRCOLORNAME)
END
SET @HAIRCOLORNAME = 'Brown'
SET @HAIRCOLORID = 2
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[HairColor] h WHERE [H].[Name] = @HAIRCOLORNAME)
BEGIN
	INSERT INTO [FairPlayDating].[HairColor]([HairColorId],[Name]) VALUES(@HAIRCOLORID, @HAIRCOLORNAME)
END
SET @HAIRCOLORNAME = 'Blond'
SET @HAIRCOLORID = 3
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[HairColor] h WHERE [H].[Name] = @HAIRCOLORNAME)
BEGIN
	INSERT INTO [FairPlayDating].[HairColor]([HairColorId],[Name]) VALUES(@HAIRCOLORID, @HAIRCOLORNAME)
END
SET @HAIRCOLORNAME = 'Red'
SET @HAIRCOLORID = 4
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[HairColor] h WHERE [H].[Name] = @HAIRCOLORNAME)
BEGIN
	INSERT INTO [FairPlayDating].[HairColor]([HairColorId],[Name]) VALUES(@HAIRCOLORID, @HAIRCOLORNAME)
END
SET @HAIRCOLORNAME = 'Gray'
SET @HAIRCOLORID = 5
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[HairColor] h WHERE [H].[Name] = @HAIRCOLORNAME)
BEGIN
	INSERT INTO [FairPlayDating].[HairColor]([HairColorId],[Name]) VALUES(@HAIRCOLORID, @HAIRCOLORNAME)
END
SET @HAIRCOLORNAME = 'White'
SET @HAIRCOLORID = 6
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[HairColor] h WHERE [H].[Name] = @HAIRCOLORNAME)
BEGIN
	INSERT INTO [FairPlayDating].[HairColor]([HairColorId],[Name]) VALUES(@HAIRCOLORID, @HAIRCOLORNAME)
END
SET @HAIRCOLORNAME = 'Bald'
SET @HAIRCOLORID = 7
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[HairColor] h WHERE [H].[Name] = @HAIRCOLORNAME)
BEGIN
	INSERT INTO [FairPlayDating].[HairColor]([HairColorId],[Name]) VALUES(@HAIRCOLORID, @HAIRCOLORNAME)
END
SET IDENTITY_INSERT [FairPlayDating].[HairColor] OFF
--END OF DEFAULT HAIRCOLOR
--START OF DEFAULT EYECOLOR
SET IDENTITY_INSERT [FairPlayDating].[EyesColor] ON
DECLARE @EyesColorNAME NVARCHAR(50) = 'Black'
DECLARE @EyesColorID INT = 1
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[EyesColor] e WHERE [E].[Name] = @EyesColorNAME)
BEGIN
	INSERT INTO [FairPlayDating].[EyesColor]([EyesColorId],[Name]) VALUES(@EyesColorID, @EyesColorNAME)
END
SET @EyesColorNAME = 'Brown'
SET @EyesColorID = 2
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[EyesColor] e WHERE [E].[Name] = @EyesColorNAME)
BEGIN
	INSERT INTO [FairPlayDating].[EyesColor]([EyesColorId],[Name]) VALUES(@EyesColorID, @EyesColorNAME)
END
SET @EyesColorNAME = 'Blue'
SET @EyesColorID = 3
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[EyesColor] e WHERE [E].[Name] = @EyesColorNAME)
BEGIN
	INSERT INTO [FairPlayDating].[EyesColor]([EyesColorId],[Name]) VALUES(@EyesColorID, @EyesColorNAME)
END
SET @EyesColorNAME = 'Green'
SET @EyesColorID = 4
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[EyesColor] e WHERE [E].[Name] = @EyesColorNAME)
BEGIN
	INSERT INTO [FairPlayDating].[EyesColor]([EyesColorId],[Name]) VALUES(@EyesColorID, @EyesColorNAME)
END
SET @EyesColorNAME = 'Hazel'
SET @EyesColorID = 5
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[EyesColor] e WHERE [E].[Name] = @EyesColorNAME)
BEGIN
	INSERT INTO [FairPlayDating].[EyesColor]([EyesColorId],[Name]) VALUES(@EyesColorID, @EyesColorNAME)
END
SET @EyesColorNAME = 'Gray'
SET @EyesColorID = 6
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[EyesColor] e WHERE [E].[Name] = @EyesColorNAME)
BEGIN
	INSERT INTO [FairPlayDating].[EyesColor]([EyesColorId],[Name]) VALUES(@EyesColorID, @EyesColorNAME)
END
SET @EyesColorNAME = 'Amber'
SET @EyesColorID = 7
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[EyesColor] e WHERE [E].[Name] = @EyesColorNAME)
BEGIN
	INSERT INTO [FairPlayDating].[EyesColor]([EyesColorId],[Name]) VALUES(@EyesColorID, @EyesColorNAME)
END
SET @EyesColorNAME = 'Other'
SET @EyesColorID = 8
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[EyesColor] e WHERE [E].[Name] = @EyesColorNAME)
BEGIN
	INSERT INTO [FairPlayDating].[EyesColor]([EyesColorId],[Name]) VALUES(@EyesColorID, @EyesColorNAME)
END
SET IDENTITY_INSERT [FairPlayDating].[EyesColor] OFF
--END OF DEFAULT EYECOLOR
--START OF TATOO STATUS values from KidStatus.Name
SET IDENTITY_INSERT [FairPlayDating].[TattooStatus] ON
DECLARE @TattooStatusNAME NVARCHAR(50) = 'Don''t have & Don''t want'
DECLARE @TattooStatusID INT = 1
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[TattooStatus] t WHERE [T].[Name] = @TattooStatusNAME)
BEGIN
	INSERT INTO [FairPlayDating].[TattooStatus]([TattooStatusId],[Name]) VALUES(@TattooStatusID, @TattooStatusNAME)
END
SET @TattooStatusNAME = 'Don''t have & Want'
SET @TattooStatusID = 2
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[TattooStatus] t WHERE [T].[Name] = @TattooStatusNAME)
BEGIN
	INSERT INTO [FairPlayDating].[TattooStatus]([TattooStatusId],[Name]) VALUES(@TattooStatusID, @TattooStatusNAME)
END
SET @TattooStatusNAME = 'Have & Don''t want more'
SET @TattooStatusID = 3
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[TattooStatus] t WHERE [T].[Name] = @TattooStatusNAME)
BEGIN
	INSERT INTO [FairPlayDating].[TattooStatus]([TattooStatusId],[Name]) VALUES(@TattooStatusID, @TattooStatusNAME)
END
SET @TattooStatusNAME = 'Have & Want more'
SET @TattooStatusID = 4
IF NOT EXISTS (SELECT * FROM [FairPlayDating].[TattooStatus] t WHERE [T].[Name] = @TattooStatusNAME)
BEGIN
	INSERT INTO [FairPlayDating].[TattooStatus]([TattooStatusId],[Name]) VALUES(@TattooStatusID, @TattooStatusNAME)
END
SET IDENTITY_INSERT [FairPlayDating].[TattooStatus] OFF
--END OF DEFAULT TATOO STATUS
COMMIT
GO