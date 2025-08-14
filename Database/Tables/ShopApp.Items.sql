CREATE TABLE [ShopApp].[Items]
(
[id] [int] NOT NULL,
[list_id] [int] NOT NULL,
[name] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[created_on] [datetime] NOT NULL CONSTRAINT [DF_ShopApp_Items_CreatedOn] DEFAULT (getutcdate()),
[checked_on] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [ShopApp].[Items] ADD CONSTRAINT [PK_ShopApp_Items] PRIMARY KEY CLUSTERED ([id]) ON [PRIMARY]
GO
ALTER TABLE [ShopApp].[Items] ADD CONSTRAINT [FK_ShopApp_Items_List_Id] FOREIGN KEY ([list_id]) REFERENCES [ShopApp].[Lists] ([id]) ON DELETE CASCADE
GO
