SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [ShopApp].[GetTemplateLists]
AS
BEGIN
    SET NOCOUNT ON;

    -- get lists for user
    SELECT 
        l.id,
        l.name
    FROM [ShopApp].[ListTemplates] l
	ORDER BY id ASC;

    -- get items for lists
    SELECT 
        i.id,
        i.list_id,
        i.name,
		i.count
    FROM [ShopApp].[ItemTemplates] i
    INNER JOIN [ShopApp].[ListTemplates] l ON i.list_id = l.id;
END
GO
GRANT EXECUTE ON  [ShopApp].[GetTemplateLists] TO [DataManager]
GO
