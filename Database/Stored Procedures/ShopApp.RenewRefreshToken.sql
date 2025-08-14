SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [ShopApp].[RenewRefreshToken] (@token varchar(900)) AS
BEGIN 
	SET NOCOUNT ON
	SET XACT_ABORT ON

	BEGIN TRAN

	DECLARE @user_id int
	SELECT @user_id = [user_id]
	FROM ShopApp.RefreshToken
	WHERE token = @token AND SYSUTCDATETIME() <= expiration_time AND renewed_on IS NULL

	IF @user_id IS NULL OR NOT EXISTS 
		(SELECT * 
		FROM ShopApp.[User]
		WHERE id = @user_id AND deleted_on IS NULL)
	BEGIN
		ROLLBACK TRANSACTION
        RETURN
	END

	DECLARE @generation int

	UPDATE ShopApp.RefreshToken
	SET renewed_on = SYSUTCDATETIME(), @generation = generation
	WHERE token = @token AND SYSUTCDATETIME() <= expiration_time AND renewed_on IS NULL

	IF @@ROWCOUNT < 1
	BEGIN
		ROLLBACK TRANSACTION
        RETURN 0
	END

	DECLARE @tokenByteLength int = 384
	DECLARE @tokenBinaryData varbinary(384) = CRYPT_GEN_RANDOM (@tokenByteLength) 
	DECLARE @newToken varchar(512)= CAST('' as xml).value('xs:base64Binary(sql:variable("@tokenBinaryData"))', 'varchar(512)')
	
	-- maybe we should check if it's unique

	INSERT INTO ShopApp.RefreshToken (token, [user_id], expiration_time, generation, renewed_on)
	VALUES(@newToken, @user_id, DATEADD(DAY, 7, SYSUTCDATETIME()), @generation + 1, NULL)

	SELECT id, full_name, email
	FROM ShopApp.[User]
	WHERE id = @user_id

	SELECT @newToken AS token

	COMMIT TRAN
END
GO
GRANT EXECUTE ON  [ShopApp].[RenewRefreshToken] TO [AuthManager]
GO
