CREATE TABLE [ShopApp].[Lists]
(
[name] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[created_on] [datetime] NOT NULL CONSTRAINT [DF_ShopApp_Lists_CreatedOn] DEFAULT (getutcdate()),
[completed_on] [datetime] NULL,
[user_id] [int] NOT NULL,
[id] [int] NOT NULL IDENTITY(1, 1)
) ON [PRIMARY]
GO
ALTER TABLE [ShopApp].[Lists] ADD CONSTRAINT [PK_ShopApp_Lists] PRIMARY KEY CLUSTERED ([id]) ON [PRIMARY]
GO
ALTER TABLE [ShopApp].[Lists] ADD CONSTRAINT [FK_Lists_User_Id] FOREIGN KEY ([user_id]) REFERENCES [ShopApp].[User] ([id])
GO
