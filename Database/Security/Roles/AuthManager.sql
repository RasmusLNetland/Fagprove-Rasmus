CREATE ROLE [AuthManager]
AUTHORIZATION [dbo]
GO
ALTER ROLE [AuthManager] ADD MEMBER [shop_app_user]
GO
