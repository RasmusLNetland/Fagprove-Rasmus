SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [ShopApp].[MarkItemsAsChecked] (@items ShopApp.[ItemChecksTable] READONLY ) AS
BEGIN 
	    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    UPDATE i
    SET i.checked_on = CASE 
                           WHEN ic.IsChecked = 1 THEN GETUTCDATE()
                           ELSE NULL
                       END
    FROM ShopApp.Items i
    INNER JOIN @items ic ON i.id = ic.Id;
	
END
GO
GRANT EXECUTE ON  [ShopApp].[MarkItemsAsChecked] TO [DataManager]
GO
