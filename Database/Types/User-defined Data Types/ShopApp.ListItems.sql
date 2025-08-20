CREATE TYPE [ShopApp].[ListItems] AS TABLE
(
[list_id] [int] NOT NULL,
[name] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[created_on] [datetime] NOT NULL,
[checked_on] [datetime] NULL,
[count] [int] NULL
)
GO
GRANT EXECUTE ON TYPE:: [ShopApp].[ListItems] TO [DataManager]
GO
