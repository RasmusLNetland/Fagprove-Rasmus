SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [ShopApp].[GetItemsForList]
    @list_id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- get items for list
    SELECT 
        i.id,
		i.list_id,
		i.name,
		i.created_on,
		i.checked_on,
		i.count
    FROM [ShopApp].[Items] i
    WHERE i.list_id = @list_id
    ORDER BY i.created_on DESC;
END
GO
GRANT EXECUTE ON  [ShopApp].[GetItemsForList] TO [DataManager]
GO
