SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE   PROCEDURE [ShopApp].[UpdateList]
    @list_id INT,
    @name NVARCHAR(max),
    @items [ShopApp].[ItemUpdateType] READONLY
AS
BEGIN
    SET NOCOUNT ON;

    -- first we update list name
    UPDATE [ShopApp].[Lists]
    SET Name = @Name
    WHERE Id = @list_id;

    -- then we update existing items (items within parameter list, which have new values)
    UPDATE i
    SET 
        i.Name = src.Name,
        i.Count = src.Count
    FROM [ShopApp].[Items] i
    INNER JOIN @items src ON i.Id = src.Id
    WHERE i.list_id = @list_id;

    -- then we insert new items (items within parameter lsit, which DO NOT exist in database)
    INSERT INTO [ShopApp].[Items] (list_id, Name, Count)
    SELECT @list_id, src.Name, src.Count
    FROM @items src
    WHERE src.Id IS NULL;

    -- then we delete items (items which are within database but NOT within the parameter list)
    DELETE i
    FROM [ShopApp].[Items] i
    WHERE i.list_id = @list_id
      AND i.Id NOT IN (SELECT Id FROM @items WHERE Id IS NOT NULL);
END
GO
GRANT EXECUTE ON  [ShopApp].[UpdateList] TO [DataManager]
GO
