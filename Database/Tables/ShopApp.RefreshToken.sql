CREATE TABLE [ShopApp].[RefreshToken]
(
[token] [varchar] (900) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[user_id] [int] NOT NULL,
[issue_timestamp] [datetime2] (2) NOT NULL CONSTRAINT [DF_RefreshToken_IssueTimestamp] DEFAULT (sysutcdatetime()),
[expiration_time] [datetime2] (2) NOT NULL,
[renewed_on] [datetime2] (2) NULL,
[generation] [int] NULL CONSTRAINT [DF__RefreshTo__gener__48CFD27E] DEFAULT ((0))
) ON [PRIMARY]
GO
ALTER TABLE [ShopApp].[RefreshToken] ADD CONSTRAINT [PK_SHOPAPP_REFRESHTOKEN] PRIMARY KEY CLUSTERED ([token]) ON [PRIMARY]
GO
