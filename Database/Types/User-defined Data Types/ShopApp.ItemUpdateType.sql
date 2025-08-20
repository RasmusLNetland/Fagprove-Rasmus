CREATE TYPE [ShopApp].[ItemUpdateType] AS TABLE
(
[Id] [int] NULL,
[Name] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Count] [int] NOT NULL
)
GO
GRANT EXECUTE ON TYPE:: [ShopApp].[ItemUpdateType] TO [DataManager]
GO
