BEGIN TRANSACTION

--START OF DEFAULT FREQUENCY
SET IDENTITY_INSERT [FairPlayDating].[Frequency] ON

DECLARE @Frequency TABLE (FrequencyId INT, Name NVARCHAR(50))
INSERT INTO @Frequency (FrequencyId, Name) VALUES
    (1, 'Never'),
    (2, '1 day per week'),
    (3, '2 days per week'),
    (4, '3 days per week'),
    (5, '4 days per week'),
    (6, '5 days per week'),
    (7, '6 days per week'),
    (8, '7 days per week')

MERGE INTO [FairPlayDating].[Frequency] AS target
USING @Frequency AS source
ON target.Name = source.Name
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([FrequencyId], [Name]) VALUES (source.FrequencyId, source.Name);

SET IDENTITY_INSERT [FairPlayDating].[Frequency] OFF
--END OF DEFAULT FREQUENCY

--START OF DEFAULT ACTIVITY
MERGE INTO [FairPlayDating].[Activity] AS target
USING (VALUES
    ('Archery'), ('Astronomy'), ('Backpacking'), ('Badminton'), ('Baking'), ('Basketball'), 
    ('Birdwatching'), ('Board games'), ('Bowling'), ('Calligraphy'), ('Camping'), ('Canoeing'), 
    ('Chess'), ('Climbing'), ('Collecting stamps'), ('Cooking'), ('Crafting'), ('Crochet'), 
    ('Cycling'), ('Dancing'), ('Drawing'), ('Embroidery'), ('Fencing'), ('Fishing'), 
    ('Gardening'), ('Geocaching'), ('Golf'), ('Handball'), ('Hiking'), ('Horseback riding'), 
    ('Ice skating'), ('Inline skating'), ('Jigsaw puzzles'), ('Jogging'), ('Kayaking'), 
    ('Knitting'), ('Lacrosse'), ('Lego building'), ('Magic tricks'), ('Martial arts'), 
    ('Meditation'), ('Metal detecting'), ('Model building'), ('Mountain biking'), 
    ('Painting'), ('Paragliding'), ('Photography'), ('Pilates'), ('Pottery'), ('Quilting'), 
    ('Racquetball'), ('Reading'), ('Rock climbing'), ('Rollerblading'), ('Rowing'), ('Running'), 
    ('Sailing'), ('Scrapbooking'), ('Scuba diving'), ('Singing'), ('Skateboarding'), 
    ('Sketching'), ('Skiing'), ('Snowboarding'), ('Snowshoeing'), ('Soccer'), ('Softball'), 
    ('Surfing'), ('Swimming'), ('Table tennis'), ('Taekwondo'), ('Tennis'), ('Traveling'), 
    ('Triathlon'), ('Ultimate Frisbee'), ('Video gaming'), ('Volleyball'), ('Wakeboarding'), 
    ('Walking'), ('Watching movies'), ('Weightlifting'), ('Wind surfing'), ('Wine tasting'), 
    ('Woodworking'), ('Writing'), ('Yoga'), ('Zumba')
) AS source (Name)
ON target.Name = source.Name
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Name) VALUES (source.Name);
--END OF DEFAULT ACTIVITY

--START OF DEFAULT GENDERS
SET IDENTITY_INSERT [FairPlayDating].[Gender] ON

DECLARE @Gender TABLE (GenderId INT, Name NVARCHAR(50))
INSERT INTO @Gender (GenderId, Name) VALUES
    (1, 'Male'),
    (2, 'Female')

MERGE INTO [FairPlayDating].[Gender] AS target
USING @Gender AS source
ON target.Name = source.Name
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([GenderId], [Name]) VALUES (source.GenderId, source.Name);

SET IDENTITY_INSERT [FairPlayDating].[Gender] OFF
--END OF DEFAULT GENDERS

--START OF DEFAULT RELIGIONS
SET IDENTITY_INSERT [FairPlayDating].[Religion] ON

DECLARE @Religion TABLE (ReligionId INT, Name NVARCHAR(50))
INSERT INTO @Religion (ReligionId, Name) VALUES
    (1, 'Catholic'),
    (2, 'Christian'),
    (3, 'Jewish'),
    (4, 'Muslim'),
    (5, 'Other')

MERGE INTO [FairPlayDating].[Religion] AS target
USING @Religion AS source
ON target.Name = source.Name
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([ReligionId], [Name]) VALUES (source.ReligionId, source.Name);

SET IDENTITY_INSERT [FairPlayDating].[Religion] OFF
--END OF DEFAULT RELIGIONS

--START OF DEFAULT DATEOBJECTIVE
SET IDENTITY_INSERT [FairPlayDating].[DateObjective] ON

DECLARE @DateObjective TABLE (DateObjectiveId INT, Name NVARCHAR(50))
INSERT INTO @DateObjective (DateObjectiveId, Name) VALUES
    (1, 'Friendship'),
    (2, 'Dating'),
    (3, 'Marriage')

MERGE INTO [FairPlayDating].[DateObjective] AS target
USING @DateObjective AS source
ON target.Name = source.Name
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([DateObjectiveId], [Name]) VALUES (source.DateObjectiveId, source.Name);

SET IDENTITY_INSERT [FairPlayDating].[DateObjective] OFF
--END OF DEFAULT DATEOBJECTIVE

--START OF DEFAULT KIDSTATUS
SET IDENTITY_INSERT [FairPlayDating].[KidStatus] ON

DECLARE @KidStatus TABLE (KidStatusId INT, Name NVARCHAR(50))
INSERT INTO @KidStatus (KidStatusId, Name) VALUES
    (1, 'Don''t have & Don''t want'),
    (2, 'Don''t have & Want'),
    (3, 'Have & Don''t want more'),
    (4, 'Have & Want more')

MERGE INTO [FairPlayDating].[KidStatus] AS target
USING @KidStatus AS source
ON target.Name = source.Name
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([KidStatusId], [Name]) VALUES (source.KidStatusId, source.Name);

SET IDENTITY_INSERT [FairPlayDating].[KidStatus] OFF
--END OF DEFAULT KIDSTATUS

--START OF DEFAULT HAIRCOLOR
SET IDENTITY_INSERT [FairPlayDating].[HairColor] ON

DECLARE @HairColor TABLE (HairColorId INT, Name NVARCHAR(50))
INSERT INTO @HairColor (HairColorId, Name) VALUES
    (1, 'Black'),
    (2, 'Brown'),
    (3, 'Blond'),
    (4, 'Red'),
    (5, 'Gray'),
    (6, 'White'),
    (7, 'Bald')

MERGE INTO [FairPlayDating].[HairColor] AS target
USING @HairColor AS source
ON target.Name = source.Name
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([HairColorId], [Name]) VALUES (source.HairColorId, source.Name);

SET IDENTITY_INSERT [FairPlayDating].[HairColor] OFF
--END OF DEFAULT HAIRCOLOR

--START OF DEFAULT EYECOLOR
SET IDENTITY_INSERT [FairPlayDating].[EyesColor] ON

DECLARE @EyesColor TABLE (EyesColorId INT, Name NVARCHAR(50))
INSERT INTO @EyesColor (EyesColorId, Name) VALUES
    (1, 'Black'),
    (2, 'Brown'),
    (3, 'Blue'),
    (4, 'Green'),
    (5, 'Hazel'),
    (6, 'Gray'),
    (7, 'Amber'),
    (8, 'Other')

MERGE INTO [FairPlayDating].[EyesColor] AS target
USING @EyesColor AS source
ON target.Name = source.Name
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([EyesColorId], [Name]) VALUES (source.EyesColorId, source.Name);

SET IDENTITY_INSERT [FairPlayDating].[EyesColor] OFF
--END OF DEFAULT EYECOLOR

--START OF DEFAULT TATOO STATUS
SET IDENTITY_INSERT [FairPlayDating].[TattooStatus] ON

DECLARE @TattooStatus TABLE (TattooStatusId INT, Name NVARCHAR(50))
INSERT INTO @TattooStatus (TattooStatusId, Name) VALUES
    (1, 'Don''t have & Don''t want'),
    (2, 'Don''t have & Want'),
    (3, 'Have & Don''t want more'),
    (4, 'Have & Want more')

MERGE INTO [FairPlayDating].[TattooStatus] AS target
USING @TattooStatus AS source
ON target.Name = source.Name
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([TattooStatusId], [Name]) VALUES (source.TattooStatusId, source.Name);

SET IDENTITY_INSERT [FairPlayDating].[TattooStatus] OFF
--END OF DEFAULT TATOO STATUS

--START OF DEFAULT PROFESSIONS
MERGE INTO [FairPlayDating].[Profession] AS target
USING (VALUES 
    ('Accountant'), ('Actor'), ('Actuary'), ('Administrative Assistant'), ('Advertising Manager'), 
    ('Aerospace Engineer'), ('Agricultural Engineer'), ('Air Traffic Controller'), ('Airline Pilot'), 
    ('Animator'), ('Anthropologist'), ('Archaeologist'), ('Architect'), ('Archivist'), ('Art Director'), 
    ('Artist'), ('Astronomer'), ('Athlete'), ('Attorney'), ('Audiologist'), ('Author'), ('Auto Mechanic'), 
    ('Baker'), ('Banker'), ('Barber'), ('Bartender'), ('Biochemist'), ('Biomedical Engineer'), 
    ('Biophysicist'), ('Bookkeeper'), ('Botanist'), ('Broadcast Technician'), ('Budget Analyst'), 
    ('Building Inspector'), ('Bus Driver'), ('Business Analyst'), ('Butcher'), ('Buyer'), ('Carpenter'), 
    ('Cartographer'), ('Cashier'), ('Chef'), ('Chemical Engineer'), ('Chemist'), ('Childcare Worker'), 
    ('Chiropractor'), ('Civil Engineer'), ('Claims Adjuster'), ('Clinical Laboratory Technician'), 
    ('Coach'), ('Commercial Diver'), ('Computer Programmer'), ('Computer Support Specialist'), 
    ('Construction Manager'), ('Consultant'), ('Content Writer'), ('Copywriter'), ('Cost Estimator'), 
    ('Counselor'), ('Court Reporter'), ('Curator'), ('Customer Service Representative'), ('Dancer'), 
    ('Database Administrator'), ('Dental Assistant'), ('Dental Hygienist'), ('Dentist'), ('Detective'), 
    ('Dietitian'), ('Dispatcher'), ('Doctor'), ('Economist'), ('Editor'), ('Electrician'), 
    ('Elementary School Teacher'), ('Elevator Installer'), ('Emergency Medical Technician (EMT)'), 
    ('Engineer'), ('Environmental Scientist'), ('Event Planner'), ('Executive Assistant'), ('Farmer'), 
    ('Fashion Designer'), ('Film Director'), ('Financial Analyst'), ('Firefighter'), ('Fitness Trainer'), 
    ('Flight Attendant'), ('Floral Designer'), ('Forester'), ('Game Designer'), ('Gardener'), 
    ('Genetic Counselor'), ('Geographer'), ('Geologist'), ('Graphic Designer'), ('Guidance Counselor'), 
    ('Hairdresser'), ('Health Educator'), ('High School Teacher'), ('Home Health Aide'), 
    ('Human Resources Specialist'), ('HVAC Technician'), ('Industrial Designer'), ('Industrial Engineer'), 
    ('Information Security Analyst'), ('Interpreter'), ('IT Manager'), ('Janitor'), ('Jeweler'), 
    ('Journalist'), ('Judge'), ('Kindergarten Teacher'), ('Landscape Architect'), ('Lawyer'), 
    ('Librarian'), ('Licensed Practical Nurse (LPN)'), ('Locksmith'), ('Machinist'), ('Management Analyst'), 
    ('Market Research Analyst'), ('Marketing Manager'), ('Massage Therapist'), ('Mathematician'), 
    ('Mechanical Engineer'), ('Medical Assistant'), ('Medical Laboratory Technician'), 
    ('Medical Transcriptionist'), ('Meteorologist'), ('Microbiologist'), ('Middle School Teacher'), 
    ('Millwright'), ('Multimedia Artist'), ('Music Director'), ('Musician'), ('Network Administrator'), 
    ('Nuclear Engineer'), ('Nurse'), ('Nurse Practitioner'), ('Nutritionist'), ('Occupational Therapist'), 
    ('Operations Manager'), ('Optician'), ('Optometrist'), ('Paralegal'), ('Park Ranger'), 
    ('Pediatrician'), ('Personal Trainer'), ('Pharmacist'), ('Photographer'), ('Physical Therapist'), 
    ('Physician'), ('Physician Assistant'), ('Physicist'), ('Pilot'), ('Plumber'), ('Police Officer'), 
    ('Political Scientist'), ('Postal Service Worker'), ('Preschool Teacher'), ('Private Detective'), 
    ('Producer'), ('Professor'), ('Programmer'), ('Project Manager'), ('Property Manager'), 
    ('Psychiatrist'), ('Psychologist'), ('Public Relations Specialist'), ('Purchasing Manager'), 
    ('Radiologic Technologist'), ('Real Estate Agent'), ('Receptionist'), ('Registered Nurse (RN)'), 
    ('Reporter'), ('Respiratory Therapist'), ('Sales Manager'), ('Sales Representative'), ('Scientist'), 
    ('Sculptor'), ('Security Guard'), ('Social Media Manager'), ('Social Worker'), ('Software Architect'), 
    ('Software Developer'), ('Software Engineer'), ('Sound Engineer'), ('Speech-Language Pathologist'), 
    ('Statistician'), ('Stockbroker'), ('Structural Engineer'), ('Surgeon'), ('Surveyor'), 
    ('Systems Analyst'), ('Tailor'), ('Teacher'), ('Technical Writer'), ('Telecommunications Technician'), 
    ('Translator'), ('Travel Agent'), ('Truck Driver'), ('Urban Planner'), ('Veterinarian'), 
    ('Video Editor'), ('Waiter/Waitress'), ('Web Developer'), ('Welder'), ('Writer'), ('Zoologist')
) AS source ([Name])
ON target.[Name] = source.[Name]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([Name]) VALUES (source.[Name]);
--END OF DEFAULT PROFESSIONS

--START OF DEFAULT PERSONALITYTYPE
MERGE INTO [FairPlayDating].[PersonalityType] AS target
USING (VALUES
    ('Ambivert'), ('Anxious Introvert'), ('Extrovert'), ('Extraverted Introvert'), 
    ('Introvert'), ('Introverted Extrovert'), ('Omnivert'), ('Restrained Introvert'), 
    ('Social Introvert'), ('Thinking Introvert')
) AS source ([Name])
ON target.[Name] = source.[Name]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([Name]) VALUES (source.[Name]);
--END OF DEFAULT PERSONALITYTYPE

COMMIT
