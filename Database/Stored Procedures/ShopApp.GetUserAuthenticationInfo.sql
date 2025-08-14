SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [ShopApp].[GetUserAuthenticationInfo](@email nvarchar(255))
AS
BEGIN
    SET NOCOUNT ON

	DECLARE @database_user_id int
	SELECT @database_user_id = u.id
	FROM [ShopApp].[User] u
	WHERE u.email = @email AND u.deleted_on IS NULL

	IF @database_user_id IS NULL
	BEGIN
		RAISERROR('Failed to find user %s or user is deleted', 16, 1, @email)
		SELECT 0
	END

	SELECT id, full_name, passwd_md5, passwd_salt
	FROM [ShopApp].[User]
	WHERE id = @database_user_id

END
GO
GRANT EXECUTE ON  [ShopApp].[GetUserAuthenticationInfo] TO [AuthManager]
GO
