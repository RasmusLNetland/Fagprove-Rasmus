SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [ShopApp].[CreateUser] (@email nvarchar(255), @full_name nvarchar(255), @passwd_md5 nvarchar(255), @passwd_salt nvarchar(32)) AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

	BEGIN TRAN

	IF EXISTS(SELECT * FROM [ShopApp].[User] WHERE email = @email AND deleted_on IS NULL)
	BEGIN
		RAISERROR('User with email %s already exists', 16, 1, @email)
		ROLLBACK TRAN
		RETURN
	END

	DECLARE @user_id int

	INSERT INTO [ShopApp].[User](email, full_name, created_on, deleted_on, passwd_md5, passwd_salt )
	VALUES( @email, @full_name, SYSUTCDATETIME(), NULL, @passwd_md5, @passwd_salt )

	SET @user_id = SCOPE_IDENTITY()

	COMMIT TRAN

	SELECT id, email, full_name 
	FROM ShopApp.[User] u
	WHERE id = @user_id
END
GO
GRANT EXECUTE ON  [ShopApp].[CreateUser] TO [AuthManager]
GO
