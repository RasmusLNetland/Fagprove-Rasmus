SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [ShopApp].[GetListsByUser]
    @user_id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- get lists for user
    SELECT 
        l.id,
        l.name,
        l.created_on,
        l.completed_on,
        l.user_id
    FROM [ShopApp].[Lists] l
    WHERE l.user_id = @user_id
    ORDER BY l.created_on DESC;

    -- get items for lists
    SELECT 
        i.id,
        i.list_id,
        i.name,
        i.created_on,
        i.checked_on
    FROM [ShopApp].[Items] i
    INNER JOIN [ShopApp].[Lists] l ON i.list_id = l.id
    WHERE l.user_id = @user_id
    ORDER BY i.created_on ASC;
END
GO
GRANT EXECUTE ON  [ShopApp].[GetListsByUser] TO [DataManager]
GO
