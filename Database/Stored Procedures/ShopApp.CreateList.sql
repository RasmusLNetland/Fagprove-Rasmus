SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [ShopApp].[CreateList] 
    @name       VARCHAR(MAX), 
    @user_id    INT, 
    @list_items [ShopApp].[ListItems] READONLY
AS
BEGIN 
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @list_id INT;

    INSERT INTO ShopApp.Lists (name, created_on, completed_on, user_id)
    VALUES (@name, SYSUTCDATETIME(), NULL, @user_id);

    SET @list_id = SCOPE_IDENTITY();

    IF EXISTS (SELECT 1 FROM @list_items)
    BEGIN
        INSERT INTO ShopApp.Items (list_id, name, created_on, checked_on, [count])
        SELECT 
            @list_id,       
            name,
            created_on,
            checked_on,
			[count]
        FROM @list_items;
    END

    SELECT id, name, created_on, completed_on, user_id
    FROM ShopApp.Lists
    WHERE id = @list_id;

    SELECT id, list_id, name, created_on, checked_on, [count]
    FROM ShopApp.Items
    WHERE list_id = @list_id;
END
GO
GRANT EXECUTE ON  [ShopApp].[CreateList] TO [DataManager]
GO
