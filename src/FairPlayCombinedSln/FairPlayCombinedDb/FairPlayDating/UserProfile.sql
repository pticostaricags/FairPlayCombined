﻿CREATE TABLE [FairPlayDating].[UserProfile]
(
	[UserProfileId] BIGINT NOT NULL CONSTRAINT PK_UserProfile PRIMARY KEY IDENTITY,
	[ApplicationUserId] NVARCHAR(450) NOT NULL CONSTRAINT FK_ApplicationUserId_UserProfile FOREIGN KEY REFERENCES [dbo].[AspNetUsers]([Id]),
	[About] NVARCHAR(100) NOT NULL, 
    [HairColorId] INT NOT NULL,
	[PreferredHairColorId] INT NOT NULL,
    [EyesColorId] INT NOT NULL,
    [PreferredEyesColorId] INT NOT NULL,
    [BiologicalGenderId] INT NOT NULL, 
    [CurrentDateObjectiveId] INT NOT NULL,
    [ReligionId] INT NOT NULL,
	[PreferredReligionId] INT NOT NULL,
    [CurrentLatitude] FLOAT NOT NULL,
    [CurrentLongitude] FLOAT NOT NULL,
    [ProfilePhotoId] BIGINT NOT NULL,
    [KidStatusId] INT NOT NULL, 
    [PreferredKidStatusId] INT NOT NULL, 
    [TattooStatusId] INT NOT NULL, 
    [PreferredTattooStatusId] INT NOT NULL, 
    [BirthDate] DATETIMEOFFSET NOT NULL, 
    [CurrentGeoLocation] [sys].[geography] NOT NULL, 
    [MainProfessionId] INT NOT NULL, 
    CONSTRAINT [FK_UserProfile_HairColor] FOREIGN KEY ([HairColorId]) REFERENCES [FairPlayDating].[HairColor]([HairColorId]),
	CONSTRAINT [FK_UserProfile_PreferredHairColor] FOREIGN KEY ([PreferredHairColorId]) REFERENCES [FairPlayDating].[HairColor]([HairColorId]),
    CONSTRAINT [FK_UserProfile_EyesColor] FOREIGN KEY ([EyesColorId]) REFERENCES [FairPlayDating].[EyesColor]([EyesColorId]),
	CONSTRAINT [FK_UserProfile_PreferredEyesColor] FOREIGN KEY ([PreferredEyesColorId]) REFERENCES [FairPlayDating].[EyesColor]([EyesColorId]),
    CONSTRAINT [FK_UserProfile_BiologicalGenderId] FOREIGN KEY ([BiologicalGenderId]) REFERENCES [FairPlayDating].[Gender]([GenderId]),
    CONSTRAINT [FK_UserProfile_CurrentDateObjectiveId] FOREIGN KEY ([CurrentDateObjectiveId]) REFERENCES [FairPlayDating].[DateObjective]([DateObjectiveId]),
    CONSTRAINT [FK_UserProfile_ReligionId] FOREIGN KEY ([ReligionId]) REFERENCES [FairPlayDating].[Religion]([ReligionId]),
	CONSTRAINT [FK_UserProfile_PreferredReligionId] FOREIGN KEY ([PreferredReligionId]) REFERENCES [FairPlayDating].[Religion]([ReligionId]),
    CONSTRAINT [FK_UserProfile_Photo] FOREIGN KEY ([ProfilePhotoId]) REFERENCES [Photo]([PhotoId]),
    CONSTRAINT [FK_UserProfile_KidStatus] FOREIGN KEY ([KidStatusId]) REFERENCES [FairPlayDating].[KidStatus]([KidStatusId]),
    CONSTRAINT [FK_UserProfile_PreferredKidStatus] FOREIGN KEY ([PreferredKidStatusId]) REFERENCES [FairPlayDating].[KidStatus]([KidStatusId]), 
    CONSTRAINT [FK_UserProfile_TattooStatus] FOREIGN KEY ([TattooStatusId]) REFERENCES [FairPlayDating].[TattooStatus]([TattooStatusId]), 
    CONSTRAINT [FK_UserProfile_PreferredTattooStatus] FOREIGN KEY ([PreferredTattooStatusId]) REFERENCES [FairPlayDating].[TattooStatus]([TattooStatusId]), 
    CONSTRAINT [FK_UserProfile_Profession] FOREIGN KEY ([MainProfessionId]) REFERENCES [FairPlayDating].[Profession]([ProfessionId])
)

GO

CREATE UNIQUE INDEX [UI_UserProfile_ApplicationUserId] ON [FairPlayDating].[UserProfile] ([ApplicationUserId])