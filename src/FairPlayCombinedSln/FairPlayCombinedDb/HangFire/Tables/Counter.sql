CREATE TABLE [HangFire].[Counter] (
    [Key]      NVARCHAR (100) NOT NULL,
    [Value]    INT            NOT NULL,
    [ExpireAt] DATETIME       NULL,
    [Id]       BIGINT         IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_HangFire_Counter] PRIMARY KEY CLUSTERED ([Key] ASC, [Id] ASC)
);

