CREATE TABLE [FairPlayTube].[VideoPlanThumbnail]
(
	[VideoPlanThumbnailId] BIGINT NOT NULL CONSTRAINT PK_VideoPlanThumbnail PRIMARY KEY IDENTITY, 
    [VideoPlanId] BIGINT NOT NULL, 
    [ImageBytes] VARBINARY(MAX) NOT NULL, 
    CONSTRAINT [FK_VideoPlanThumbnail_VideoPlan] FOREIGN KEY ([VideoPlanId]) REFERENCES [FairPlayTube].[VideoPlan]([VideoPlanId])
)
