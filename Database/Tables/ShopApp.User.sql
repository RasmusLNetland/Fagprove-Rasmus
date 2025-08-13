CREATE TABLE [ShopApp].[User]
(
[id] [int] NOT NULL IDENTITY(1, 1),
[email] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[full_name] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[created_on] [datetime] NOT NULL CONSTRAINT [DF_NAVTOR_USER_created_on] DEFAULT (getutcdate()),
[deleted_on] [datetime] NULL,
[passwd_md5] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[passwd_salt] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [ShopApp].[User] ADD CONSTRAINT [PK_SHOPAPP_USER] PRIMARY KEY CLUSTERED ([id]) ON [PRIMARY]
GO
