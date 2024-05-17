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
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Archery');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Astronomy');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Backpacking');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Badminton');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Baking');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Basketball');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Birdwatching');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Board games');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Bowling');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Calligraphy');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Camping');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Canoeing');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Chess');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Climbing');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Collecting stamps');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Cooking');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Crafting');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Crochet');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Cycling');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Dancing');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Drawing');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Embroidery');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Fencing');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Fishing');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Gardening');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Geocaching');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Golf');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Handball');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Hiking');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Horseback riding');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Ice skating');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Inline skating');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Jigsaw puzzles');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Jogging');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Kayaking');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Knitting');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Lacrosse');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Lego building');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Magic tricks');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Martial arts');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Meditation');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Metal detecting');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Model building');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Mountain biking');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Painting');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Paragliding');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Photography');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Pilates');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Pottery');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Quilting');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Racquetball');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Reading');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Rock climbing');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Rollerblading');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Rowing');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Running');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Sailing');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Scrapbooking');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Scuba diving');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Singing');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Skateboarding');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Sketching');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Skiing');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Snowboarding');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Snowshoeing');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Soccer');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Softball');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Surfing');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Swimming');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Table tennis');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Taekwondo');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Tennis');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Traveling');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Triathlon');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Ultimate Frisbee');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Video gaming');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Volleyball');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Wakeboarding');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Walking');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Watching movies');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Weightlifting');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Wind surfing');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Wine tasting');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Woodworking');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Writing');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Yoga');
INSERT INTO [FairPlayDating].[Activity] ([Name]) VALUES ('Zumba');
--END OF DEFAULT ACTIVITY
--START OF DEFAULT GENDERS
SET IDENTITY_INSERT [FairPlayDating].[Gender] ON
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
SET IDENTITY_INSERT [FairPlayDating].[Gender] OFF
--END OF DEFAULT GENDERS
--START OF DEFAULT RELIGIONS
SET IDENTITY_INSERT [FairPlayDating].[Religion] ON
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
SET IDENTITY_INSERT [FairPlayDating].[Religion] OFF
--END OF DEFAULT RELIGIONS
--START OF DEFAULT DATEOBJECTIVE
SET IDENTITY_INSERT [FairPlayDating].[DateObjective] ON
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
SET IDENTITY_INSERT [FairPlayDating].[DateObjective] OFF
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
--START OF DEFAULT PROFESSIONS values from Professions.Name
INSERT INTO [FairPlayDating].[Profession] ([Name])
VALUES
    ('Accountant'),
    ('Actor'),
    ('Actuary'),
    ('Administrative Assistant'),
    ('Advertising Manager'),
    ('Aerospace Engineer'),
    ('Agricultural Engineer'),
    ('Air Traffic Controller'),
    ('Airline Pilot'),
    ('Animator'),
    ('Anthropologist'),
    ('Archaeologist'),
    ('Architect'),
    ('Archivist'),
    ('Art Director'),
    ('Artist'),
    ('Astronomer'),
    ('Athlete'),
    ('Attorney'),
    ('Audiologist'),
    ('Author'),
    ('Auto Mechanic'),
    ('Baker'),
    ('Banker'),
    ('Barber'),
    ('Bartender'),
    ('Biochemist'),
    ('Biomedical Engineer'),
    ('Biophysicist'),
    ('Bookkeeper'),
    ('Botanist'),
    ('Broadcast Technician'),
    ('Budget Analyst'),
    ('Building Inspector'),
    ('Bus Driver'),
    ('Business Analyst'),
    ('Butcher'),
    ('Buyer'),
    ('Carpenter'),
    ('Cartographer'),
    ('Cashier'),
    ('Chef'),
    ('Chemical Engineer'),
    ('Chemist'),
    ('Childcare Worker'),
    ('Chiropractor'),
    ('Civil Engineer'),
    ('Claims Adjuster'),
    ('Clinical Laboratory Technician'),
    ('Coach'),
    ('Commercial Diver'),
    ('Computer Programmer'),
    ('Computer Support Specialist'),
    ('Construction Manager'),
    ('Consultant'),
    ('Content Writer'),
    ('Copywriter'),
    ('Cost Estimator'),
    ('Counselor'),
    ('Court Reporter'),
    ('Curator'),
    ('Customer Service Representative'),
    ('Dancer'),
    ('Database Administrator'),
    ('Dental Assistant'),
    ('Dental Hygienist'),
    ('Dentist'),
    ('Detective'),
    ('Dietitian'),
    ('Dispatcher'),
    ('Doctor'),
    ('Economist'),
    ('Editor'),
    ('Electrician'),
    ('Elementary School Teacher'),
    ('Elevator Installer'),
    ('Emergency Medical Technician (EMT)'),
    ('Engineer'),
    ('Environmental Scientist'),
    ('Event Planner'),
    ('Executive Assistant'),
    ('Farmer'),
    ('Fashion Designer'),
    ('Film Director'),
    ('Financial Analyst'),
    ('Firefighter'),
    ('Fitness Trainer'),
    ('Flight Attendant'),
    ('Floral Designer'),
    ('Forester'),
    ('Game Designer'),
    ('Gardener'),
    ('Genetic Counselor'),
    ('Geographer'),
    ('Geologist'),
    ('Graphic Designer'),
    ('Guidance Counselor'),
    ('Hairdresser'),
    ('Health Educator'),
    ('High School Teacher'),
    ('Home Health Aide'),
    ('Human Resources Specialist'),
    ('HVAC Technician'),
    ('Industrial Designer'),
    ('Industrial Engineer'),
    ('Information Security Analyst'),
    ('Interpreter'),
    ('IT Manager'),
    ('Janitor'),
    ('Jeweler'),
    ('Journalist'),
    ('Judge'),
    ('Kindergarten Teacher'),
    ('Landscape Architect'),
    ('Lawyer'),
    ('Librarian'),
    ('Licensed Practical Nurse (LPN)'),
    ('Locksmith'),
    ('Machinist'),
    ('Management Analyst'),
    ('Market Research Analyst'),
    ('Marketing Manager'),
    ('Massage Therapist'),
    ('Mathematician'),
    ('Mechanical Engineer'),
    ('Medical Assistant'),
    ('Medical Laboratory Technician'),
    ('Medical Transcriptionist'),
    ('Meteorologist'),
    ('Microbiologist'),
    ('Middle School Teacher'),
    ('Millwright'),
    ('Multimedia Artist'),
    ('Music Director'),
    ('Musician'),
    ('Network Administrator'),
    ('Nuclear Engineer'),
    ('Nurse'),
    ('Nurse Practitioner'),
    ('Nutritionist'),
    ('Occupational Therapist'),
    ('Operations Manager'),
    ('Optician'),
    ('Optometrist'),
    ('Paralegal'),
    ('Park Ranger'),
    ('Pediatrician'),
    ('Personal Trainer'),
    ('Pharmacist'),
    ('Photographer'),
    ('Physical Therapist'),
    ('Physician'),
    ('Physician Assistant'),
    ('Physicist'),
    ('Pilot'),
    ('Plumber'),
    ('Police Officer'),
    ('Political Scientist'),
    ('Postal Service Worker'),
    ('Preschool Teacher'),
    ('Private Detective'),
    ('Producer'),
    ('Professor'),
    ('Programmer'),
    ('Project Manager'),
    ('Property Manager'),
    ('Psychiatrist'),
    ('Psychologist'),
    ('Public Relations Specialist'),
    ('Purchasing Manager'),
    ('Radiologic Technologist'),
    ('Real Estate Agent'),
    ('Receptionist'),
    ('Registered Nurse (RN)'),
    ('Reporter'),
    ('Respiratory Therapist'),
    ('Sales Manager'),
    ('Sales Representative'),
    ('Scientist'),
    ('Sculptor'),
    ('Security Guard'),
    ('Social Media Manager'),
    ('Social Worker'),
    ('Software Architect'),
    ('Software Developer'),
    ('Software Engineer'),
    ('Sound Engineer'),
    ('Speech-Language Pathologist'),
    ('Statistician'),
    ('Stockbroker'),
    ('Structural Engineer'),
    ('Surgeon'),
    ('Surveyor'),
    ('Systems Analyst'),
    ('Tailor'),
    ('Teacher'),
    ('Technical Writer'),
    ('Telecommunications Technician'),
    ('Translator'),
    ('Travel Agent'),
    ('Truck Driver'),
    ('Urban Planner'),
    ('Veterinarian'),
    ('Video Editor'),
    ('Waiter/Waitress'),
    ('Web Developer'),
    ('Welder'),
    ('Writer'),
    ('Zoologist');

--END OF DEFAULT PROFESSIONS values from Professions.Name
COMMIT