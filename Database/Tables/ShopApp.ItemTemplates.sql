CREATE TABLE [ShopApp].[ItemTemplates]
(
[id] [int] NOT NULL IDENTITY(1, 1),
[list_id] [int] NOT NULL,
[name] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Count] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [ShopApp].[ItemTemplates] ADD CONSTRAINT [PK_ShopApp_ItemsTemplates] PRIMARY KEY CLUSTERED ([id]) ON [PRIMARY]
GO
ALTER TABLE [ShopApp].[ItemTemplates] ADD CONSTRAINT [FK_ShopApp_ItemTemplates_List_Id] FOREIGN KEY ([list_id]) REFERENCES [ShopApp].[ListTemplates] ([id]) ON DELETE CASCADE
GO
