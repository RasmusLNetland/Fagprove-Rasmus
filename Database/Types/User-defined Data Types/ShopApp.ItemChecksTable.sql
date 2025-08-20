CREATE TYPE [ShopApp].[ItemChecksTable] AS TABLE
(
[Id] [int] NOT NULL,
[IsChecked] [bit] NOT NULL,
PRIMARY KEY CLUSTERED ([Id])
)
GO
GRANT EXECUTE ON TYPE:: [ShopApp].[ItemChecksTable] TO [DataManager]
GO
