SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [ShopApp].[CreateRefreshToken] (@email nvarchar(255)) AS
BEGIN 
	SET NOCOUNT ON
	SET XACT_ABORT ON

	DECLARE @user_id int
	SELECT @user_id = u.id
	FROM [ShopApp].[User] u
	WHERE u.email = @email AND u.deleted_on IS NULL

	IF @user_id IS NULL 
		RETURN 0

	DECLARE @tokenByteLength int = 384
	DECLARE @tokenBinaryData varbinary(384) = CRYPT_GEN_RANDOM (@tokenByteLength) 
	DECLARE @newToken varchar(512)= CAST('' as xml).value('xs:base64Binary(sql:variable("@tokenBinaryData"))', 'varchar(512)')
	
	-- maybe we should check if it's unique

	INSERT INTO ShopApp.RefreshToken (token, user_id, expiration_time, renewed_on)
	VALUES(@newToken, @user_id, DATEADD(DAY, 7, SYSUTCDATETIME()), NULL)

	SELECT @newToken AS token

END
GO
GRANT EXECUTE ON  [ShopApp].[CreateRefreshToken] TO [AuthManager]
GO
