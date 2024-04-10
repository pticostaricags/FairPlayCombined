CREATE TABLE [FairPlayTube].[VideoPlan]
(
	[VideoPlanId] BIGINT NOT NULL CONSTRAINT PK_VideoPlan PRIMARY KEY IDENTITY, 
    [ApplicationUserId] NVARCHAR(450) NOT NULL, 
    [VideoName] NVARCHAR(50) NOT NULL, 
    [VideoDescription] NVARCHAR(500) NOT NULL 
   
    CONSTRAINT [FK_VideoPlan_ApplicationUser] FOREIGN KEY ([ApplicationUserId]) REFERENCES [dbo].[AspNetUsers]([Id]), 
    [VideoScript] NVARCHAR(3000) NOT NULL
)

GO

CREATE UNIQUE INDEX [IX_VideoPlan_VideoName] ON [FairPlayTube].[VideoPlan] ([VideoName],[ApplicationUserId])
