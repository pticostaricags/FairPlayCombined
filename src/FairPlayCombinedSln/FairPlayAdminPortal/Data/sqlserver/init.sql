IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'FairPlayCombinedDb')
BEGIN
  CREATE DATABASE FairPlayCombinedDb;
END;
GO

USE FairPlayCombinedDb;
GO

IF SCHEMA_ID(N'FairPlayDating') IS NULL EXEC(N'CREATE SCHEMA [FairPlayDating];');
GO


IF SCHEMA_ID(N'FairPlayBudget') IS NULL EXEC(N'CREATE SCHEMA [FairPlayBudget];');
GO


IF SCHEMA_ID(N'FairPlaySocial') IS NULL EXEC(N'CREATE SCHEMA [FairPlaySocial];');
GO


IF SCHEMA_ID(N'FairPlayShop') IS NULL EXEC(N'CREATE SCHEMA [FairPlayShop];');
GO


IF SCHEMA_ID(N'FairPlayTube') IS NULL EXEC(N'CREATE SCHEMA [FairPlayTube];');
GO


CREATE TABLE [FairPlayDating].[Activity] (
    [ActivityId] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Activity] PRIMARY KEY ([ActivityId])
);
GO


CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [ConfigurationSecret] (
    [ConfigurationSecretId] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Value] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_ConfigurationSecret] PRIMARY KEY ([ConfigurationSecretId])
);
GO


CREATE TABLE [Country] (
    [CountryId] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Country] PRIMARY KEY ([CountryId])
);
GO


CREATE TABLE [Culture] (
    [CultureId] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Culture] PRIMARY KEY ([CultureId])
);
GO


CREATE TABLE [FairPlayBudget].[Currency] (
    [CurrencyId] int NOT NULL IDENTITY,
    [Description] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Currency] PRIMARY KEY ([CurrencyId])
);
GO


CREATE TABLE [FairPlayDating].[DateObjective] (
    [DateObjectiveId] int NOT NULL IDENTITY,
    [Name] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_DateObjective] PRIMARY KEY ([DateObjectiveId])
);
GO


CREATE TABLE [ErrorLog] (
    [ErrorLogId] bigint NOT NULL IDENTITY,
    [Message] nvarchar(max) NOT NULL,
    [StackTrace] nvarchar(max) NOT NULL,
    [FullException] nvarchar(max) NOT NULL,
    [RowCreationDateTime] datetimeoffset NOT NULL,
    [RowCreationUser] nvarchar(256) NOT NULL,
    [SourceApplication] nvarchar(250) NOT NULL,
    [OriginatorIPAddress] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_ErrorLog] PRIMARY KEY ([ErrorLogId])
);
GO


CREATE TABLE [FairPlayDating].[EyesColor] (
    [EyesColorId] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_EyesColor] PRIMARY KEY ([EyesColorId])
);
GO


CREATE TABLE [FairPlayDating].[Frequency] (
    [FrequencyId] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Frequency] PRIMARY KEY ([FrequencyId])
);
GO


CREATE TABLE [FairPlayDating].[Gender] (
    [GenderId] int NOT NULL IDENTITY,
    [Name] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_Gender] PRIMARY KEY ([GenderId])
);
GO


CREATE TABLE [FairPlayDating].[HairColor] (
    [HairColorId] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_HairColor] PRIMARY KEY ([HairColorId])
);
GO


CREATE TABLE [FairPlayDating].[KidStatus] (
    [KidStatusId] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_KidStatus] PRIMARY KEY ([KidStatusId])
);
GO


CREATE TABLE [OpenAIPrompt] (
    [OpenAIPromptId] bigint NOT NULL IDENTITY,
    [OriginalPrompt] nvarchar(max) NOT NULL,
    [RevisedPrompt] nvarchar(max) NOT NULL,
    [Model] nvarchar(50) NOT NULL,
    [GeneratedImageBytes] varbinary(max) NOT NULL,
    [RowCreationDateTime] datetimeoffset NOT NULL,
    [RowCreationUser] nvarchar(256) NOT NULL,
    [SourceApplication] nvarchar(250) NOT NULL,
    [OriginatorIPAddress] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_OpenAIPrompt] PRIMARY KEY ([OpenAIPromptId])
);
GO


CREATE TABLE [Photo] (
    [PhotoId] bigint NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    [Filename] nvarchar(50) NOT NULL,
    [PhotoBytes] varbinary(max) NOT NULL,
    CONSTRAINT [PK_Photo] PRIMARY KEY ([PhotoId])
);
GO


CREATE TABLE [FairPlaySocial].[PostType] (
    [PostTypeId] int NOT NULL IDENTITY,
    [Name] nvarchar(10) NOT NULL,
    CONSTRAINT [PK_PostType] PRIMARY KEY ([PostTypeId])
);
GO


CREATE TABLE [FairPlaySocial].[PostVisibility] (
    [PostVisibilityId] int NOT NULL IDENTITY,
    [Name] nvarchar(11) NOT NULL,
    [Description] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_PostVisibility] PRIMARY KEY ([PostVisibilityId])
);
GO


CREATE TABLE [FairPlayShop].[ProductStatus] (
    [ProductStatusId] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_ProductStatus] PRIMARY KEY ([ProductStatusId])
);
GO


CREATE TABLE [FairPlayDating].[Profession] (
    [ProfessionId] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Profession] PRIMARY KEY ([ProfessionId])
);
GO


CREATE TABLE [Prompt] (
    [PromptId] int NOT NULL IDENTITY,
    [PromptName] nvarchar(100) NOT NULL,
    [BaseText] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Prompt] PRIMARY KEY ([PromptId])
);
GO


CREATE TABLE [FairPlayDating].[Religion] (
    [ReligionId] int NOT NULL IDENTITY,
    [Name] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_Religion] PRIMARY KEY ([ReligionId])
);
GO


CREATE TABLE [FairPlayShop].[StoreCustomerAddress] (
    [Id] int NOT NULL,
    CONSTRAINT [PK_StoreCustomerAddress] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [FairPlayDating].[TattooStatus] (
    [TattooStatusId] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_TattooStatus] PRIMARY KEY ([TattooStatusId])
);
GO


CREATE TABLE [ThemeConfiguration] (
    [ThemeConfigurationId] int NOT NULL IDENTITY,
    [Key] nvarchar(50) NOT NULL,
    [Value] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_ThemeConfiguration] PRIMARY KEY ([ThemeConfigurationId])
);
GO


CREATE TABLE [FairPlayTube].[VideoIndexerSupportedLanguage] (
    [VideoIndexerSupportedLanguageId] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    [LanguageCode] nvarchar(10) NOT NULL,
    CONSTRAINT [PK_VideoIndexerSupportedLanguage] PRIMARY KEY ([VideoIndexerSupportedLanguageId])
);
GO


CREATE TABLE [FairPlayTube].[VideoIndexingCost] (
    [VideoIndexingCostId] bigint NOT NULL IDENTITY,
    [CostPerMinute] money NOT NULL,
    [RowCreationDateTime] datetimeoffset NOT NULL,
    [RowCreationUser] nvarchar(256) NOT NULL,
    [SourceApplication] nvarchar(250) NOT NULL,
    [OriginatorIPAddress] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_VideoIndexingCost] PRIMARY KEY ([VideoIndexingCostId])
);
GO


CREATE TABLE [FairPlayTube].[VideoIndexingMargin] (
    [VideoIndexingMarginId] bigint NOT NULL IDENTITY,
    [Margin] decimal(5,4) NOT NULL,
    [RowCreationDateTime] datetimeoffset NOT NULL,
    [RowCreationUser] nvarchar(256) NOT NULL,
    [SourceApplication] nvarchar(250) NOT NULL,
    [OriginatorIPAddress] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_VideoIndexingMargin] PRIMARY KEY ([VideoIndexingMarginId])
);
GO


CREATE TABLE [FairPlayTube].[VideoIndexStatus] (
    [VideoIndexStatusId] int NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_VideoIndexStatus] PRIMARY KEY ([VideoIndexStatusId])
);
GO


CREATE TABLE [FairPlayTube].[VideoJobApplicationStatus] (
    [VideoJobApplicationStatusId] smallint NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [Description] nvarchar(2150) NOT NULL,
    CONSTRAINT [PK_VideoJobApplicationStatus] PRIMARY KEY ([VideoJobApplicationStatusId])
);
GO


CREATE TABLE [FairPlayTube].[VideoVisibility] (
    [VideoVisibilityId] smallint NOT NULL,
    [Name] nvarchar(10) NOT NULL,
    CONSTRAINT [PK_VideoVisibility] PRIMARY KEY ([VideoVisibilityId])
);
GO


CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [FairPlayDating].[ApplicationUserVouch] (
    [ApplicationUserVouchId] bigint NOT NULL IDENTITY,
    [FromApplicationUserId] nvarchar(450) NOT NULL,
    [ToApplicationUserId] nvarchar(450) NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [RowCreationDateTime] datetimeoffset NOT NULL,
    [RowCreationUser] nvarchar(256) NOT NULL,
    [SourceApplication] nvarchar(250) NOT NULL,
    [OriginatorIPAddress] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_ApplicationUserVouch] PRIMARY KEY ([ApplicationUserVouchId]),
    CONSTRAINT [FK_ApplicationUserVouch_FromUser] FOREIGN KEY ([FromApplicationUserId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_ApplicationUserVouch_ToUser] FOREIGN KEY ([ToApplicationUserId]) REFERENCES [AspNetUsers] ([Id])
);
GO


CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [FairPlaySocial].[Group] (
    [GroupId] bigint NOT NULL IDENTITY,
    [OwnerApplicationUserId] nvarchar(450) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [Description] nvarchar(250) NOT NULL,
    [TopicTag] nvarchar(100) NOT NULL,
    [RowCreationDateTime] datetimeoffset NOT NULL,
    [RowCreationUser] nvarchar(256) NOT NULL,
    [SourceApplication] nvarchar(250) NOT NULL,
    [OriginatorIPAddress] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Group] PRIMARY KEY ([GroupId]),
    CONSTRAINT [FK_Group_ApplicationUser] FOREIGN KEY ([OwnerApplicationUserId]) REFERENCES [AspNetUsers] ([Id])
);
GO


CREATE TABLE [FairPlayDating].[LikedUserProfile] (
    [LikedUserProfileId] bigint NOT NULL IDENTITY,
    [LikingApplicationUserId] nvarchar(450) NOT NULL,
    [LikedApplicationUserId] nvarchar(450) NOT NULL,
    [RowCreationDateTime] datetimeoffset NOT NULL,
    [RowCreationUser] nvarchar(256) NOT NULL,
    [SourceApplication] nvarchar(250) NOT NULL,
    [OriginatorIPAddress] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_LikedUserProfile] PRIMARY KEY ([LikedUserProfileId]),
    CONSTRAINT [FK_LikedUserProfile_LikedApplicactionUser] FOREIGN KEY ([LikedApplicationUserId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_LikedUserProfile_LikingApplicactionUser] FOREIGN KEY ([LikingApplicationUserId]) REFERENCES [AspNetUsers] ([Id])
);
GO


CREATE TABLE [FairPlayBudget].[MonthlyBudgetInfo] (
    [MonthlyBudgetInfoId] bigint NOT NULL IDENTITY,
    [Description] nvarchar(150) NOT NULL,
    [OwnerId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_MonthlyBudgetInfo] PRIMARY KEY ([MonthlyBudgetInfoId]),
    CONSTRAINT [FK_MonthlyBudgetInfo_AspNetUsers] FOREIGN KEY ([OwnerId]) REFERENCES [AspNetUsers] ([Id])
);
GO


CREATE TABLE [FairPlayDating].[NotLikedUserProfile] (
    [NotLikedUserProfileId] bigint NOT NULL IDENTITY,
    [NotLikingApplicationUserId] nvarchar(450) NOT NULL,
    [NotLikedApplicationUserId] nvarchar(450) NOT NULL,
    [RowCreationDateTime] datetimeoffset NOT NULL,
    [RowCreationUser] nvarchar(256) NOT NULL,
    [SourceApplication] nvarchar(250) NOT NULL,
    [OriginatorIPAddress] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_NotLikedUserProfile] PRIMARY KEY ([NotLikedUserProfileId]),
    CONSTRAINT [FK_NotLikedUserProfile_NotLikedApplicactionUser] FOREIGN KEY ([NotLikedApplicationUserId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_NotLikedUserProfile_NotLikingApplicactionUser] FOREIGN KEY ([NotLikingApplicationUserId]) REFERENCES [AspNetUsers] ([Id])
);
GO


CREATE TABLE [FairPlayShop].[Store] (
    [StoreId] bigint NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    [OwnerId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Store] PRIMARY KEY ([StoreId]),
    CONSTRAINT [FK_Store_AspNetUsers] FOREIGN KEY ([OwnerId]) REFERENCES [AspNetUsers] ([Id])
);
GO


CREATE TABLE [UserFunds] (
    [UserFunds] bigint NOT NULL IDENTITY,
    [ApplicationUserId] nvarchar(450) NOT NULL,
    [AvailableFunds] money NOT NULL,
    CONSTRAINT [PK_UserFunds] PRIMARY KEY ([UserFunds]),
    CONSTRAINT [FK_UserFunds_AspNetUsers] FOREIGN KEY ([ApplicationUserId]) REFERENCES [AspNetUsers] ([Id])
);
GO


CREATE TABLE [UserMessage] (
    [UserMessageId] bigint NOT NULL IDENTITY,
    [FromApplicationUserId] nvarchar(450) NOT NULL,
    [ToApplicationUserId] nvarchar(450) NOT NULL,
    [Message] nvarchar(max) NOT NULL,
    [SourceApplication] nvarchar(250) NOT NULL,
    [OriginatorIpaddress] nvarchar(100) NOT NULL,
    [RowCreationDateTime] datetimeoffset NOT NULL,
    [RowCreationUser] nvarchar(256) NOT NULL,
    [ReadByDestinatary] bit NOT NULL,
    CONSTRAINT [PK_UserMessage] PRIMARY KEY ([UserMessageId]),
    CONSTRAINT [FK_FromApplicationUserId_AspNetUsers] FOREIGN KEY ([FromApplicationUserId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_ToApplicationUserId_AspNetUsers] FOREIGN KEY ([ToApplicationUserId]) REFERENCES [AspNetUsers] ([Id])
);
GO


CREATE TABLE [FairPlayTube].[VideoPlan] (
    [VideoPlanId] bigint NOT NULL IDENTITY,
    [ApplicationUserId] nvarchar(450) NOT NULL,
    [VideoName] nvarchar(50) NOT NULL,
    [VideoDescription] nvarchar(500) NOT NULL,
    [VideoScript] nvarchar(3000) NOT NULL,
    CONSTRAINT [PK_VideoPlan] PRIMARY KEY ([VideoPlanId]),
    CONSTRAINT [FK_VideoPlan_ApplicationUser] FOREIGN KEY ([ApplicationUserId]) REFERENCES [AspNetUsers] ([Id])
);
GO


CREATE TABLE [StateOrProvince] (
    [StateOrProvinceId] int NOT NULL IDENTITY,
    [Name] nvarchar(500) NOT NULL,
    [CountryId] int NOT NULL,
    CONSTRAINT [PK_StateOrProvince] PRIMARY KEY ([StateOrProvinceId]),
    CONSTRAINT [FK_StateOrProvince_Country] FOREIGN KEY ([CountryId]) REFERENCES [Country] ([CountryId])
);
GO


CREATE TABLE [Resource] (
    [ResourceId] int NOT NULL IDENTITY,
    [Type] nvarchar(1500) NOT NULL,
    [Key] nvarchar(50) NOT NULL,
    [Value] text NOT NULL,
    [CultureId] int NOT NULL,
    CONSTRAINT [PK_Resource] PRIMARY KEY ([ResourceId]),
    CONSTRAINT [FK_Resource_Culture] FOREIGN KEY ([CultureId]) REFERENCES [Culture] ([CultureId])
);
GO


CREATE TABLE [FairPlayDating].[UserActivity] (
    [UserActivityId] bigint NOT NULL IDENTITY,
    [ApplicationUserId] nvarchar(450) NOT NULL,
    [ActivityId] int NOT NULL,
    [FrequencyId] int NOT NULL,
    CONSTRAINT [PK_UserActivity] PRIMARY KEY ([UserActivityId]),
    CONSTRAINT [FK_UserActivity_Activity] FOREIGN KEY ([ActivityId]) REFERENCES [FairPlayDating].[Activity] ([ActivityId]),
    CONSTRAINT [FK_UserActivity_ApplicationUser] FOREIGN KEY ([ApplicationUserId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_UserActivity_Frequency] FOREIGN KEY ([FrequencyId]) REFERENCES [FairPlayDating].[Frequency] ([FrequencyId])
);
GO


CREATE TABLE [PromptVariable] (
    [PromptVariableId] int NOT NULL IDENTITY,
    [PromptId] int NOT NULL,
    [VariableName] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_PromptVariable] PRIMARY KEY ([PromptVariableId]),
    CONSTRAINT [FK_PromptVariable_Prompt] FOREIGN KEY ([PromptId]) REFERENCES [Prompt] ([PromptId])
);
GO


CREATE TABLE [FairPlayDating].[UserProfile] (
    [UserProfileId] bigint NOT NULL IDENTITY,
    [ApplicationUserId] nvarchar(450) NOT NULL,
    [About] nvarchar(100) NOT NULL,
    [HairColorId] int NOT NULL,
    [PreferredHairColorId] int NOT NULL,
    [EyesColorId] int NOT NULL,
    [PreferredEyesColorId] int NOT NULL,
    [BiologicalGenderId] int NOT NULL,
    [CurrentDateObjectiveId] int NOT NULL,
    [ReligionId] int NOT NULL,
    [PreferredReligionId] int NOT NULL,
    [CurrentLatitude] float NOT NULL,
    [CurrentLongitude] float NOT NULL,
    [ProfilePhotoId] bigint NOT NULL,
    [KidStatusId] int NOT NULL,
    [PreferredKidStatusId] int NOT NULL,
    [TattooStatusId] int NOT NULL,
    [PreferredTattooStatusId] int NOT NULL,
    [BirthDate] datetimeoffset NOT NULL,
    [MainProfessionId] int NOT NULL,
    [CurrentGeoLocation] geography NOT NULL,
    CONSTRAINT [PK_UserProfile] PRIMARY KEY ([UserProfileId]),
    CONSTRAINT [FK_ApplicationUserId_UserProfile] FOREIGN KEY ([ApplicationUserId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_UserProfile_BiologicalGenderId] FOREIGN KEY ([BiologicalGenderId]) REFERENCES [FairPlayDating].[Gender] ([GenderId]),
    CONSTRAINT [FK_UserProfile_CurrentDateObjectiveId] FOREIGN KEY ([CurrentDateObjectiveId]) REFERENCES [FairPlayDating].[DateObjective] ([DateObjectiveId]),
    CONSTRAINT [FK_UserProfile_EyesColor] FOREIGN KEY ([EyesColorId]) REFERENCES [FairPlayDating].[EyesColor] ([EyesColorId]),
    CONSTRAINT [FK_UserProfile_HairColor] FOREIGN KEY ([HairColorId]) REFERENCES [FairPlayDating].[HairColor] ([HairColorId]),
    CONSTRAINT [FK_UserProfile_KidStatus] FOREIGN KEY ([KidStatusId]) REFERENCES [FairPlayDating].[KidStatus] ([KidStatusId]),
    CONSTRAINT [FK_UserProfile_Photo] FOREIGN KEY ([ProfilePhotoId]) REFERENCES [Photo] ([PhotoId]),
    CONSTRAINT [FK_UserProfile_PreferredEyesColor] FOREIGN KEY ([PreferredEyesColorId]) REFERENCES [FairPlayDating].[EyesColor] ([EyesColorId]),
    CONSTRAINT [FK_UserProfile_PreferredHairColor] FOREIGN KEY ([PreferredHairColorId]) REFERENCES [FairPlayDating].[HairColor] ([HairColorId]),
    CONSTRAINT [FK_UserProfile_PreferredKidStatus] FOREIGN KEY ([PreferredKidStatusId]) REFERENCES [FairPlayDating].[KidStatus] ([KidStatusId]),
    CONSTRAINT [FK_UserProfile_PreferredReligionId] FOREIGN KEY ([PreferredReligionId]) REFERENCES [FairPlayDating].[Religion] ([ReligionId]),
    CONSTRAINT [FK_UserProfile_PreferredTattooStatus] FOREIGN KEY ([PreferredTattooStatusId]) REFERENCES [FairPlayDating].[TattooStatus] ([TattooStatusId]),
    CONSTRAINT [FK_UserProfile_Profession] FOREIGN KEY ([MainProfessionId]) REFERENCES [FairPlayDating].[Profession] ([ProfessionId]),
    CONSTRAINT [FK_UserProfile_ReligionId] FOREIGN KEY ([ReligionId]) REFERENCES [FairPlayDating].[Religion] ([ReligionId]),
    CONSTRAINT [FK_UserProfile_TattooStatus] FOREIGN KEY ([TattooStatusId]) REFERENCES [FairPlayDating].[TattooStatus] ([TattooStatusId])
);
GO


CREATE TABLE [FairPlayTube].[VideoInfo] (
    [VideoInfoId] bigint NOT NULL IDENTITY,
    [AccountId] uniqueidentifier NOT NULL,
    [VideoId] nvarchar(50) NULL,
    [Location] nvarchar(50) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [Description] nvarchar(500) NULL,
    [FileName] nvarchar(50) NOT NULL,
    [VideoBloblUrl] nvarchar(500) NULL,
    [IndexedVideoUrl] nvarchar(500) NULL,
    [ApplicationUserId] nvarchar(450) NOT NULL,
    [VideoIndexStatusId] int NOT NULL,
    [VideoDurationInSeconds] float NOT NULL,
    [VideoIndexSourceClass] nvarchar(500) NULL,
    [Price] money NOT NULL,
    [ExternalVideoSourceUrl] nvarchar(500) NULL,
    [VideoLanguageCode] nvarchar(10) NULL,
    [VideoVisibilityId] smallint NOT NULL DEFAULT (1),
    [ThumbnailUrl] nvarchar(500) NULL,
    [RowCreationDateTime] datetimeoffset NOT NULL DEFAULT (GETUTCDATE()),
    [RowCreationUser] nvarchar(256) NOT NULL DEFAULT ('Unknown'),
    [SourceApplication] nvarchar(250) NOT NULL DEFAULT ('Unknown'),
    [OriginatorIPAddress] nvarchar(100) NOT NULL,
    [YouTubeVideoId] nvarchar(11) NULL,
    [VideoIndexJSON] nvarchar(max) NULL,
    [VideoThumbnailPhotoId] bigint NULL,
    [VideoIndexingProcessingPercentage] int NULL DEFAULT (0),
    [PublishedUrl] nvarchar(1000) NULL,
    CONSTRAINT [PK_VideoInfo] PRIMARY KEY ([VideoInfoId]),
    CONSTRAINT [FK_VideoInfo_ApplicationUser] FOREIGN KEY ([ApplicationUserId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_VideoInfo_Photo_Thumbnail] FOREIGN KEY ([VideoThumbnailPhotoId]) REFERENCES [Photo] ([PhotoId]),
    CONSTRAINT [FK_VideoInfo_VideoIndexStatus] FOREIGN KEY ([VideoIndexStatusId]) REFERENCES [FairPlayTube].[VideoIndexStatus] ([VideoIndexStatusId]),
    CONSTRAINT [FK_VideoInfo_VideoVisibility] FOREIGN KEY ([VideoVisibilityId]) REFERENCES [FairPlayTube].[VideoVisibility] ([VideoVisibilityId])
);
DECLARE @description AS sql_variant;
SET @description = N'Video Owner Id';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'FairPlayTube', 'TABLE', N'VideoInfo', 'COLUMN', N'ApplicationUserId';
GO


CREATE TABLE [FairPlaySocial].[Post] (
    [PostId] bigint NOT NULL IDENTITY,
    [PostVisibilityId] int NOT NULL DEFAULT (1),
    [PhotoId] bigint NULL,
    [PostTypeId] int NOT NULL DEFAULT (1),
    [ReplyToPostId] bigint NULL,
    [GroupId] bigint NULL,
    [Text] nvarchar(500) NOT NULL,
    [OwnerApplicationUserId] nvarchar(450) NOT NULL,
    [RowCreationDateTime] datetimeoffset NOT NULL,
    [RowCreationUser] nvarchar(256) NOT NULL,
    [SourceApplication] nvarchar(250) NOT NULL,
    [OriginatorIPAddress] nvarchar(100) NOT NULL,
    [CreatedFromPostId] bigint NULL,
    [CreatedAtLatitude] float NULL,
    [CreatedAtLongitude] float NULL,
    [CreatedAtGeoLocation] geography NULL,
    [RootPostId] bigint NULL,
    [ValidFrom] datetime2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
    [ValidTo] datetime2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
    CONSTRAINT [PK_Post] PRIMARY KEY ([PostId]),
    CONSTRAINT [FK_Post_ApplicationUser] FOREIGN KEY ([OwnerApplicationUserId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_Post_Group] FOREIGN KEY ([GroupId]) REFERENCES [FairPlaySocial].[Group] ([GroupId]),
    CONSTRAINT [FK_Post_Photo] FOREIGN KEY ([PhotoId]) REFERENCES [Photo] ([PhotoId]),
    CONSTRAINT [FK_Post_Post] FOREIGN KEY ([CreatedFromPostId]) REFERENCES [FairPlaySocial].[Post] ([PostId]),
    CONSTRAINT [FK_Post_PostType] FOREIGN KEY ([PostTypeId]) REFERENCES [FairPlaySocial].[PostType] ([PostTypeId]),
    CONSTRAINT [FK_Post_PostVisibility] FOREIGN KEY ([PostVisibilityId]) REFERENCES [FairPlaySocial].[PostVisibility] ([PostVisibilityId]),
    CONSTRAINT [FK_Post_Post_ReplyToPostId] FOREIGN KEY ([ReplyToPostId]) REFERENCES [FairPlaySocial].[Post] ([PostId]),
    CONSTRAINT [FK_Post_Post_RootPost] FOREIGN KEY ([RootPostId]) REFERENCES [FairPlaySocial].[Post] ([PostId]),
    PERIOD FOR SYSTEM_TIME([ValidFrom], [ValidTo])
) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [FairPlaySocial].[PostHistory]));
GO


CREATE TABLE [FairPlayBudget].[Expense] (
    [ExpenseId] bigint NOT NULL IDENTITY,
    [ExpenseDateTime] datetimeoffset NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [Amount] money NOT NULL,
    [OwnerId] nvarchar(450) NOT NULL,
    [MonthlyBudgetInfoId] bigint NOT NULL,
    [CurrencyId] int NOT NULL,
    CONSTRAINT [PK_Expense] PRIMARY KEY ([ExpenseId]),
    CONSTRAINT [FK_Expense_AspNetUsers] FOREIGN KEY ([OwnerId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_Expense_Currency] FOREIGN KEY ([CurrencyId]) REFERENCES [FairPlayBudget].[Currency] ([CurrencyId]),
    CONSTRAINT [FK_Expense_MonthlyBudgetInfo] FOREIGN KEY ([MonthlyBudgetInfoId]) REFERENCES [FairPlayBudget].[MonthlyBudgetInfo] ([MonthlyBudgetInfoId])
);
GO


CREATE TABLE [FairPlayBudget].[Income] (
    [IncomeId] bigint NOT NULL IDENTITY,
    [IncomeDateTime] datetimeoffset NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [Amount] money NOT NULL,
    [OwnerId] nvarchar(450) NOT NULL,
    [MonthlyBudgetInfoId] bigint NOT NULL,
    [CurrencyId] int NOT NULL,
    CONSTRAINT [PK_Income] PRIMARY KEY ([IncomeId]),
    CONSTRAINT [FK_Income_AspNetUsers] FOREIGN KEY ([OwnerId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_Income_Currency] FOREIGN KEY ([CurrencyId]) REFERENCES [FairPlayBudget].[Currency] ([CurrencyId]),
    CONSTRAINT [FK_Income_MonthlyBudgetInfo] FOREIGN KEY ([MonthlyBudgetInfoId]) REFERENCES [FairPlayBudget].[MonthlyBudgetInfo] ([MonthlyBudgetInfoId])
);
GO


CREATE TABLE [FairPlayShop].[Product] (
    [ProductId] bigint NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [QuantityInStock] int NOT NULL,
    [ProductStatusId] int NOT NULL,
    [OwnerId] nvarchar(450) NOT NULL,
    [SellingPrice] money NOT NULL,
    [AcquisitionCost] money NOT NULL,
    [SKU] nvarchar(50) NOT NULL,
    [Barcode] nvarchar(50) NULL,
    [ThumbnailPhotoId] bigint NOT NULL,
    [StoreId] bigint NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY ([ProductId]),
    CONSTRAINT [FK_Product_AspNetUsers] FOREIGN KEY ([OwnerId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_Product_Photo] FOREIGN KEY ([ThumbnailPhotoId]) REFERENCES [Photo] ([PhotoId]),
    CONSTRAINT [FK_Product_ProductStatus] FOREIGN KEY ([ProductStatusId]) REFERENCES [FairPlayShop].[ProductStatus] ([ProductStatusId]),
    CONSTRAINT [FK_Product_Store] FOREIGN KEY ([StoreId]) REFERENCES [FairPlayShop].[Store] ([StoreId])
);
GO


CREATE TABLE [FairPlayShop].[StoreCustomer] (
    [StoreCustomerId] bigint NOT NULL IDENTITY,
    [StoreId] bigint NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [FirstSurname] nvarchar(50) NOT NULL,
    [SecondSurname] nvarchar(50) NOT NULL,
    [EmailAddress] nvarchar(50) NOT NULL,
    [PhoneNumber] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_StoreCustomer] PRIMARY KEY ([StoreCustomerId]),
    CONSTRAINT [FK_StoreCustomer_Store] FOREIGN KEY ([StoreId]) REFERENCES [FairPlayShop].[Store] ([StoreId])
);
GO


CREATE TABLE [FairPlayTube].[VideoPlanThumbnail] (
    [VideoPlanThumbnailId] bigint NOT NULL IDENTITY,
    [VideoPlanId] bigint NOT NULL,
    [ImageBytes] varbinary(max) NOT NULL,
    CONSTRAINT [PK_VideoPlanThumbnail] PRIMARY KEY ([VideoPlanThumbnailId]),
    CONSTRAINT [FK_VideoPlanThumbnail_VideoPlan] FOREIGN KEY ([VideoPlanId]) REFERENCES [FairPlayTube].[VideoPlan] ([VideoPlanId])
);
GO


CREATE TABLE [City] (
    [CityId] int NOT NULL IDENTITY,
    [Name] nvarchar(500) NOT NULL,
    [StateOrProvinceId] int NOT NULL,
    CONSTRAINT [PK_City] PRIMARY KEY ([CityId]),
    CONSTRAINT [FK_City_StateOrProvince] FOREIGN KEY ([StateOrProvinceId]) REFERENCES [StateOrProvince] ([StateOrProvinceId])
);
GO


CREATE TABLE [FairPlayTube].[VideoCaptions] (
    [VideoCaptionsId] bigint NOT NULL IDENTITY,
    [VideoInfoId] bigint NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [Language] nvarchar(10) NOT NULL,
    CONSTRAINT [PK_VideoCaptions] PRIMARY KEY ([VideoCaptionsId]),
    CONSTRAINT [FK_VideoCaptions_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo] ([VideoInfoId])
);
GO


CREATE TABLE [FairPlayTube].[VideoComment] (
    [VideoCommentId] bigint NOT NULL IDENTITY,
    [VideoInfoId] bigint NOT NULL,
    [ApplicationUserId] nvarchar(450) NOT NULL,
    [Comment] nvarchar(500) NOT NULL,
    [RowCreationDateTime] datetimeoffset NOT NULL,
    [RowCreationUser] nvarchar(256) NULL,
    [SourceApplication] nvarchar(250) NULL,
    [OriginatorIPAddress] nvarchar(100) NULL,
    CONSTRAINT [PK_VideoComment] PRIMARY KEY ([VideoCommentId]),
    CONSTRAINT [FK_ApplicationUserId_VideoComment] FOREIGN KEY ([ApplicationUserId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_VideoInfo_VideoComment] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo] ([VideoInfoId])
);
GO


CREATE TABLE [FairPlayTube].[VideoDigitalMarketingDailyPosts] (
    [VideoDigitalMarketingDailyPostsId] bigint NOT NULL IDENTITY,
    [VideoInfoId] bigint NOT NULL,
    [SocialNetworkName] nvarchar(50) NOT NULL,
    [HtmlVideoDigitalMarketingDailyPostsIdeas] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_VideoDigitalMarketingDailyPosts] PRIMARY KEY ([VideoDigitalMarketingDailyPostsId]),
    CONSTRAINT [FK_VideoDigitalMarketingDailyPosts_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo] ([VideoInfoId])
);
GO


CREATE TABLE [FairPlayTube].[VideoDigitalMarketingPlan] (
    [VideoDigitalMarketingPlan] bigint NOT NULL IDENTITY,
    [VideoInfoId] bigint NOT NULL,
    [SocialNetworkName] nvarchar(50) NOT NULL,
    [HtmlDigitalMarketingPlan] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_VideoDigitalMarketingPlan] PRIMARY KEY ([VideoDigitalMarketingPlan]),
    CONSTRAINT [FK_VideoDigitalMarketingPlan_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo] ([VideoInfoId])
);
GO


CREATE TABLE [FairPlayTube].[VideoIndexingTransaction] (
    [VideoIndexingTransactionId] bigint NOT NULL IDENTITY,
    [VideoInfoId] bigint NOT NULL,
    [IndexingCost] money NOT NULL,
    [RowCreationDateTime] datetimeoffset NOT NULL,
    [RowCreationUser] nvarchar(256) NOT NULL,
    [SourceApplication] nvarchar(250) NOT NULL,
    [OriginatorIPAddress] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_VideoIndexingTransaction] PRIMARY KEY ([VideoIndexingTransactionId]),
    CONSTRAINT [FK_VideoIndexingTransaction_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo] ([VideoInfoId])
);
GO


CREATE TABLE [FairPlayTube].[VideoInfographic] (
    [VideoInfographicId] bigint NOT NULL,
    [VideoInfoId] bigint NOT NULL,
    [ImageBytes] varbinary(50) NOT NULL,
    CONSTRAINT [PK_VideoInfographic] PRIMARY KEY ([VideoInfographicId]),
    CONSTRAINT [FK_Video_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo] ([VideoInfoId])
);
GO


CREATE TABLE [FairPlayTube].[VideoJob] (
    [VideoJobId] bigint NOT NULL IDENTITY,
    [VideoInfoId] bigint NOT NULL,
    [Budget] money NOT NULL,
    [Title] nvarchar(50) NOT NULL,
    [Description] nvarchar(250) NOT NULL,
    [RowCreationDateTime] datetimeoffset NOT NULL,
    [RowCreationUser] nvarchar(256) NOT NULL,
    [SourceApplication] nvarchar(250) NOT NULL,
    [OriginatorIPAddress] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_VideoJob] PRIMARY KEY ([VideoJobId]),
    CONSTRAINT [FK_VideoJob_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo] ([VideoInfoId])
);
GO


CREATE TABLE [FairPlayTube].[VideoKeyword] (
    [VideoKeywordId] bigint NOT NULL IDENTITY,
    [VideoInfoId] bigint NOT NULL,
    [Keyword] nvarchar(500) NOT NULL,
    CONSTRAINT [PK_VideoKeyword] PRIMARY KEY ([VideoKeywordId]),
    CONSTRAINT [FK_VideoKeyword_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo] ([VideoInfoId])
);
GO


CREATE TABLE [FairPlayTube].[VideoThumbnail] (
    [VideoThumbnailId] bigint NOT NULL IDENTITY,
    [VideoInfoId] bigint NOT NULL,
    [PhotoId] bigint NOT NULL,
    CONSTRAINT [PK_VideoThumbnail] PRIMARY KEY ([VideoThumbnailId]),
    CONSTRAINT [FK_VideoThumbnail_Photo] FOREIGN KEY ([PhotoId]) REFERENCES [Photo] ([PhotoId]),
    CONSTRAINT [FK_VideoThumbnail_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo] ([VideoInfoId])
);
GO


CREATE TABLE [FairPlayTube].[VideoTopic] (
    [VideoTopicId] bigint NOT NULL IDENTITY,
    [VideoInfoId] bigint NOT NULL,
    [Topic] nvarchar(500) NOT NULL,
    [Confidence] float NOT NULL,
    CONSTRAINT [PK_VideoTopic] PRIMARY KEY ([VideoTopicId]),
    CONSTRAINT [FK_VideoTopic_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo] ([VideoInfoId])
);
GO


CREATE TABLE [FairPlayTube].[VideoWatchTime] (
    [VideoWatchTimeId] bigint NOT NULL IDENTITY,
    [VideoInfoId] bigint NOT NULL,
    [SessionId] uniqueidentifier NOT NULL,
    [SessionStartDatetime] datetimeoffset NOT NULL,
    [WatchTime] float NOT NULL,
    [WatchedByApplicationUserId] nvarchar(450) NULL,
    [LastUpdateDatetime] datetimeoffset NOT NULL,
    CONSTRAINT [PK_VideoWatchTime] PRIMARY KEY ([VideoWatchTimeId]),
    CONSTRAINT [FK_VideoWatchTime_AspNetUsers] FOREIGN KEY ([WatchedByApplicationUserId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_VideoWatchTime_VideoInfo] FOREIGN KEY ([VideoInfoId]) REFERENCES [FairPlayTube].[VideoInfo] ([VideoInfoId])
);
GO


CREATE TABLE [FairPlayShop].[StoreCustomerOrder] (
    [StoreCustomerOrderId] bigint NOT NULL IDENTITY,
    [StoreCustomerId] bigint NOT NULL,
    [OrderDateTime] datetimeoffset NOT NULL,
    [OrderSubTotal] money NOT NULL,
    [TaxTotal] money NOT NULL,
    [OrderTotal] money NOT NULL,
    CONSTRAINT [PK_StoreCustomerOrder] PRIMARY KEY ([StoreCustomerOrderId]),
    CONSTRAINT [FK_StoreCustomerOrder_StoreCustomer] FOREIGN KEY ([StoreCustomerId]) REFERENCES [FairPlayShop].[StoreCustomer] ([StoreCustomerId])
);
GO


CREATE TABLE [FairPlayShop].[StoreCustomerOrderDetail] (
    [StoreCustomerOrderDetail] bigint NOT NULL IDENTITY,
    [StoreCustomerOrderId] bigint NOT NULL,
    [ProductId] bigint NOT NULL,
    [UnityPrice] money NOT NULL,
    [Quantity] money NOT NULL,
    [LineTotal] money NOT NULL,
    CONSTRAINT [PK_StoreCustomerOrderDetail] PRIMARY KEY ([StoreCustomerOrderDetail]),
    CONSTRAINT [FK_StoreCustomerOrderDetail_Product] FOREIGN KEY ([ProductId]) REFERENCES [FairPlayShop].[Product] ([ProductId]),
    CONSTRAINT [FK_StoreCustomerOrderDetail_StoreCustomerOrder] FOREIGN KEY ([StoreCustomerOrderId]) REFERENCES [FairPlayShop].[StoreCustomerOrder] ([StoreCustomerOrderId])
);
GO


CREATE INDEX [IX_ApplicationUserVouch_ToApplicationUserId] ON [FairPlayDating].[ApplicationUserVouch] ([ToApplicationUserId]);
GO


CREATE UNIQUE INDEX [UI_ApplicationUserVouch_FromApplicationUserId_ToApplicationUserId] ON [FairPlayDating].[ApplicationUserVouch] ([FromApplicationUserId], [ToApplicationUserId]);
GO


CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO


CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE ([NormalizedName] IS NOT NULL);
GO


CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO


CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO


CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO


CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO


CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE ([NormalizedUserName] IS NOT NULL);
GO


CREATE UNIQUE INDEX [UI_City_Name] ON [City] ([StateOrProvinceId], [Name]);
GO


CREATE UNIQUE INDEX [UI_ConfigurationSecret_Name] ON [ConfigurationSecret] ([Name]);
GO


CREATE UNIQUE INDEX [UI_Country_Name] ON [Country] ([Name]);
GO


CREATE UNIQUE INDEX [UI_Culture_Name] ON [Culture] ([Name]);
GO


CREATE INDEX [IX_Expense_CurrencyId] ON [FairPlayBudget].[Expense] ([CurrencyId]);
GO


CREATE INDEX [IX_Expense_MonthlyBudgetInfoId] ON [FairPlayBudget].[Expense] ([MonthlyBudgetInfoId]);
GO


CREATE INDEX [IX_Expense_OwnerId] ON [FairPlayBudget].[Expense] ([OwnerId]);
GO


CREATE INDEX [IX_Group_OwnerApplicationUserId] ON [FairPlaySocial].[Group] ([OwnerApplicationUserId]);
GO


CREATE UNIQUE INDEX [UI_Group_Name] ON [FairPlaySocial].[Group] ([Name]);
GO


CREATE INDEX [IX_Income_CurrencyId] ON [FairPlayBudget].[Income] ([CurrencyId]);
GO


CREATE INDEX [IX_Income_MonthlyBudgetInfoId] ON [FairPlayBudget].[Income] ([MonthlyBudgetInfoId]);
GO


CREATE INDEX [IX_Income_OwnerId] ON [FairPlayBudget].[Income] ([OwnerId]);
GO


CREATE INDEX [IX_LikedUserProfile_LikedApplicationUserId] ON [FairPlayDating].[LikedUserProfile] ([LikedApplicationUserId]);
GO


CREATE UNIQUE INDEX [UI_LikedUserProfile_LikingApplicationUserId_LikedApplicationUserId] ON [FairPlayDating].[LikedUserProfile] ([LikingApplicationUserId], [LikedApplicationUserId]);
GO


CREATE INDEX [IX_MonthlyBudgetInfo_OwnerId] ON [FairPlayBudget].[MonthlyBudgetInfo] ([OwnerId]);
GO


CREATE UNIQUE INDEX [UI_MonthlyBudgetInfo_Description] ON [FairPlayBudget].[MonthlyBudgetInfo] ([Description]);
GO


CREATE INDEX [IX_NotLikedUserProfile_NotLikedApplicationUserId] ON [FairPlayDating].[NotLikedUserProfile] ([NotLikedApplicationUserId]);
GO


CREATE UNIQUE INDEX [UI_NotLikedUserProfile_NotLikingApplicationUserId_NotLikedApplicationUserId] ON [FairPlayDating].[NotLikedUserProfile] ([NotLikingApplicationUserId], [NotLikedApplicationUserId]);
GO


CREATE INDEX [IX_Post_CreatedFromPostId] ON [FairPlaySocial].[Post] ([CreatedFromPostId]);
GO


CREATE INDEX [IX_Post_GroupId] ON [FairPlaySocial].[Post] ([GroupId]);
GO


CREATE INDEX [IX_Post_OwnerApplicationUserId] ON [FairPlaySocial].[Post] ([OwnerApplicationUserId]);
GO


CREATE INDEX [IX_Post_PhotoId] ON [FairPlaySocial].[Post] ([PhotoId]);
GO


CREATE INDEX [IX_Post_PostTypeId] ON [FairPlaySocial].[Post] ([PostTypeId]);
GO


CREATE INDEX [IX_Post_PostVisibilityId] ON [FairPlaySocial].[Post] ([PostVisibilityId]);
GO


CREATE INDEX [IX_Post_ReplyToPostId] ON [FairPlaySocial].[Post] ([ReplyToPostId]);
GO


CREATE INDEX [IX_Post_RootPostId] ON [FairPlaySocial].[Post] ([RootPostId]);
GO


CREATE UNIQUE INDEX [UI_PostType_Name] ON [FairPlaySocial].[PostType] ([Name]);
GO


CREATE UNIQUE INDEX [UI_PostVisibility_Name] ON [FairPlaySocial].[PostVisibility] ([Name]);
GO


CREATE INDEX [IX_Product_OwnerId] ON [FairPlayShop].[Product] ([OwnerId]);
GO


CREATE INDEX [IX_Product_ProductStatusId] ON [FairPlayShop].[Product] ([ProductStatusId]);
GO


CREATE INDEX [IX_Product_StoreId] ON [FairPlayShop].[Product] ([StoreId]);
GO


CREATE INDEX [IX_Product_ThumbnailPhotoId] ON [FairPlayShop].[Product] ([ThumbnailPhotoId]);
GO


CREATE UNIQUE INDEX [UI_Product_Name] ON [FairPlayShop].[Product] ([Name], [OwnerId]);
GO


CREATE UNIQUE INDEX [UI_Profession_Name] ON [FairPlayDating].[Profession] ([Name]);
GO


CREATE UNIQUE INDEX [UI_Prompt_PromptName] ON [Prompt] ([PromptName]);
GO


CREATE UNIQUE INDEX [UI_PromptVariable_PromptId_VariableName] ON [PromptVariable] ([PromptId], [VariableName]);
GO


CREATE INDEX [IX_Resource_CultureId] ON [Resource] ([CultureId]);
GO


CREATE UNIQUE INDEX [UI_Resource_Type_Key_CultureId] ON [Resource] ([Type], [Key], [CultureId]);
GO


CREATE UNIQUE INDEX [UI_StateOrProvince_Name] ON [StateOrProvince] ([CountryId], [Name]);
GO


CREATE INDEX [IX_Store_OwnerId] ON [FairPlayShop].[Store] ([OwnerId]);
GO


CREATE INDEX [IX_StoreCustomer_StoreId] ON [FairPlayShop].[StoreCustomer] ([StoreId]);
GO


CREATE INDEX [IX_StoreCustomerOrder_StoreCustomerId] ON [FairPlayShop].[StoreCustomerOrder] ([StoreCustomerId]);
GO


CREATE INDEX [IX_StoreCustomerOrderDetail_ProductId] ON [FairPlayShop].[StoreCustomerOrderDetail] ([ProductId]);
GO


CREATE INDEX [IX_StoreCustomerOrderDetail_StoreCustomerOrderId] ON [FairPlayShop].[StoreCustomerOrderDetail] ([StoreCustomerOrderId]);
GO


CREATE UNIQUE INDEX [UI_TattooStatus_Name] ON [FairPlayDating].[TattooStatus] ([Name]);
GO


CREATE INDEX [IX_UserActivity_ActivityId] ON [FairPlayDating].[UserActivity] ([ActivityId]);
GO


CREATE INDEX [IX_UserActivity_FrequencyId] ON [FairPlayDating].[UserActivity] ([FrequencyId]);
GO


CREATE UNIQUE INDEX [UI_UserActivity_ApplicationUserId_ActivityId] ON [FairPlayDating].[UserActivity] ([ApplicationUserId], [ActivityId]);
GO


CREATE UNIQUE INDEX [UI_UserFunds_ApplicationUserId] ON [UserFunds] ([ApplicationUserId]);
GO


CREATE INDEX [IX_UserMessage_FromApplicationUserId] ON [UserMessage] ([FromApplicationUserId]);
GO


CREATE INDEX [IX_UserMessage_ToApplicationUserId] ON [UserMessage] ([ToApplicationUserId]);
GO


CREATE INDEX [IX_UserProfile_BiologicalGenderId] ON [FairPlayDating].[UserProfile] ([BiologicalGenderId]);
GO


CREATE INDEX [IX_UserProfile_CurrentDateObjectiveId] ON [FairPlayDating].[UserProfile] ([CurrentDateObjectiveId]);
GO


CREATE INDEX [IX_UserProfile_EyesColorId] ON [FairPlayDating].[UserProfile] ([EyesColorId]);
GO


CREATE INDEX [IX_UserProfile_HairColorId] ON [FairPlayDating].[UserProfile] ([HairColorId]);
GO


CREATE INDEX [IX_UserProfile_KidStatusId] ON [FairPlayDating].[UserProfile] ([KidStatusId]);
GO


CREATE INDEX [IX_UserProfile_MainProfessionId] ON [FairPlayDating].[UserProfile] ([MainProfessionId]);
GO


CREATE INDEX [IX_UserProfile_PreferredEyesColorId] ON [FairPlayDating].[UserProfile] ([PreferredEyesColorId]);
GO


CREATE INDEX [IX_UserProfile_PreferredHairColorId] ON [FairPlayDating].[UserProfile] ([PreferredHairColorId]);
GO


CREATE INDEX [IX_UserProfile_PreferredKidStatusId] ON [FairPlayDating].[UserProfile] ([PreferredKidStatusId]);
GO


CREATE INDEX [IX_UserProfile_PreferredReligionId] ON [FairPlayDating].[UserProfile] ([PreferredReligionId]);
GO


CREATE INDEX [IX_UserProfile_PreferredTattooStatusId] ON [FairPlayDating].[UserProfile] ([PreferredTattooStatusId]);
GO


CREATE INDEX [IX_UserProfile_ProfilePhotoId] ON [FairPlayDating].[UserProfile] ([ProfilePhotoId]);
GO


CREATE INDEX [IX_UserProfile_ReligionId] ON [FairPlayDating].[UserProfile] ([ReligionId]);
GO


CREATE INDEX [IX_UserProfile_TattooStatusId] ON [FairPlayDating].[UserProfile] ([TattooStatusId]);
GO


CREATE UNIQUE INDEX [UI_UserProfile_ApplicationUserId] ON [FairPlayDating].[UserProfile] ([ApplicationUserId]);
GO


CREATE INDEX [IX_VideoCaptions_VideoInfoId] ON [FairPlayTube].[VideoCaptions] ([VideoInfoId]);
GO


CREATE INDEX [IX_VideoComment_ApplicationUserId] ON [FairPlayTube].[VideoComment] ([ApplicationUserId]);
GO


CREATE INDEX [IX_VideoComment_VideoInfoId] ON [FairPlayTube].[VideoComment] ([VideoInfoId]);
GO


CREATE UNIQUE INDEX [IX_VideoDigitalMarketingDailyPosts_SocialNetworkPlan] ON [FairPlayTube].[VideoDigitalMarketingDailyPosts] ([VideoInfoId], [SocialNetworkName]);
GO


CREATE UNIQUE INDEX [IX_VideoDigitalMarketingPlan_SocialNetworkPlan] ON [FairPlayTube].[VideoDigitalMarketingPlan] ([VideoInfoId], [SocialNetworkName]);
GO


CREATE UNIQUE INDEX [UI_VideoIndexerSupportedLanguage_LanguageCode] ON [FairPlayTube].[VideoIndexerSupportedLanguage] ([LanguageCode]);
GO


CREATE UNIQUE INDEX [UI_VideoIndexerSupportedLanguage_Name] ON [FairPlayTube].[VideoIndexerSupportedLanguage] ([Name]);
GO


CREATE INDEX [IX_VideoIndexingTransaction_VideoInfoId] ON [FairPlayTube].[VideoIndexingTransaction] ([VideoInfoId]);
GO


CREATE INDEX [IX_VideoInfo_ApplicationUserId] ON [FairPlayTube].[VideoInfo] ([ApplicationUserId]);
GO


CREATE INDEX [IX_VideoInfo_VideoIndexStatusId] ON [FairPlayTube].[VideoInfo] ([VideoIndexStatusId]);
GO


CREATE INDEX [IX_VideoInfo_VideoThumbnailPhotoId] ON [FairPlayTube].[VideoInfo] ([VideoThumbnailPhotoId]);
GO


CREATE INDEX [IX_VideoInfo_VideoVisibilityId] ON [FairPlayTube].[VideoInfo] ([VideoVisibilityId]);
GO


CREATE UNIQUE INDEX [UI_VideoInfo_VideoId] ON [FairPlayTube].[VideoInfo] ([VideoId]) WHERE [VideoId] IS NOT NULL;
GO


CREATE UNIQUE INDEX [UI_VideoInfo_YouTubeVideoId] ON [FairPlayTube].[VideoInfo] ([YouTubeVideoId]) WHERE YouTubeVideoId IS NOT NULL;
GO


CREATE INDEX [IX_VideoInfographic_VideoInfoId] ON [FairPlayTube].[VideoInfographic] ([VideoInfoId]);
GO


CREATE INDEX [IX_VideoJob_VideoInfoId] ON [FairPlayTube].[VideoJob] ([VideoInfoId]);
GO


CREATE UNIQUE INDEX [UI_VideoJobApplicationStatus_Name] ON [FairPlayTube].[VideoJobApplicationStatus] ([Name]);
GO


CREATE UNIQUE INDEX [UI_VideoKeyword_VideoInfoId_Keyword] ON [FairPlayTube].[VideoKeyword] ([VideoInfoId], [Keyword]);
GO


CREATE INDEX [IX_VideoPlan_ApplicationUserId] ON [FairPlayTube].[VideoPlan] ([ApplicationUserId]);
GO


CREATE UNIQUE INDEX [IX_VideoPlan_VideoName] ON [FairPlayTube].[VideoPlan] ([VideoName], [ApplicationUserId]);
GO


CREATE INDEX [IX_VideoPlanThumbnail_VideoPlanId] ON [FairPlayTube].[VideoPlanThumbnail] ([VideoPlanId]);
GO


CREATE INDEX [IX_VideoThumbnail_PhotoId] ON [FairPlayTube].[VideoThumbnail] ([PhotoId]);
GO


CREATE UNIQUE INDEX [UI_VideoThumbnail_VideoPhoto] ON [FairPlayTube].[VideoThumbnail] ([VideoInfoId], [PhotoId]);
GO


CREATE UNIQUE INDEX [UI_VideoTopic_VideoInfoId_Topic] ON [FairPlayTube].[VideoTopic] ([VideoInfoId], [Topic]);
GO


CREATE INDEX [IX_VideoWatchTime_WatchedByApplicationUserId] ON [FairPlayTube].[VideoWatchTime] ([WatchedByApplicationUserId]);
GO


CREATE UNIQUE INDEX [UI_VideoWatchTime_SessionId] ON [FairPlayTube].[VideoWatchTime] ([SessionId]);
GO


CREATE UNIQUE INDEX [UI_VideoWatchTime_VideoInfoId_SessionId] ON [FairPlayTube].[VideoWatchTime] ([VideoInfoId], [SessionId]);
GO


BEGIN TRANSACTION
DECLARE @ADMIN_ROLE_NAME NVARCHAR(256) = 'SystemAdmin'
DECLARE @USER_ROLE_NAME NVARCHAR(256) = 'User'
--START OF DEFAULT ROLES
IF NOT EXISTS(SELECT * FROM AspNetRoles WHERE Name = @ADMIN_ROLE_NAME)
BEGIN
    INSERT INTO AspNetRoles(Id, [Name], NormalizedName) 
    VALUES (NEWID(), @ADMIN_ROLE_NAME, @ADMIN_ROLE_NAME)
END
IF NOT EXISTS(SELECT * FROM AspNetRoles WHERE Name = @USER_ROLE_NAME)
BEGIN
    INSERT INTO AspNetRoles(Id, [Name], NormalizedName) 
    VALUES (NEWID(), @USER_ROLE_NAME, @USER_ROLE_NAME)
END
--END OF DEFAULT ROLES
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