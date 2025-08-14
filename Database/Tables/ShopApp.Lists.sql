CREATE TABLE [ShopApp].[Lists]
(
[id] [int] NOT NULL,
[name] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[created_on] [datetime] NOT NULL CONSTRAINT [DF_ShopApp_Lists_CreatedOn] DEFAULT (getutcdate()),
[completed_on] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [ShopApp].[Lists] ADD CONSTRAINT [PK_ShopApp_Lists] PRIMARY KEY CLUSTERED ([id]) ON [PRIMARY]
GO
