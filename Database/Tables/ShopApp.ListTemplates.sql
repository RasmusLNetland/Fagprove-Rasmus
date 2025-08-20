CREATE TABLE [ShopApp].[ListTemplates]
(
[id] [int] NOT NULL IDENTITY(1, 1),
[name] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [ShopApp].[ListTemplates] ADD CONSTRAINT [PK_ShopApp_ListTemplates] PRIMARY KEY CLUSTERED ([id]) ON [PRIMARY]
GO
