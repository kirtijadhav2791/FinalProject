USE [EShoppingApplication]
GO

/****** Object:  StoredProcedure [dbo].[SpAdminSignUp]    Script Date: 6/21/2022 1:11:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SpAdminSignUp](
@UserName varchar(255), 
@PassWord varchar(255), 
@MasterPassWord varchar(255), 
@Role varchar(10)
)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			
			IF (@Role like 'admin')
			BEGIN

				IF EXISTS (SELECT * FROM UserDetail WHERE Role = 'master' AND PassWord=@MasterPassWord)
				BEGIN
					
					IF EXISTS (SELECT * FROM UserDetail WHERE Role = 'admin' AND UserName=@UserName)
					BEGIN
						select 2 as Result;
					END
					else
					BEGIN

							insert into UserDetail(UserName, PassWord, Role) values (@UserName,@PassWord,@Role);
							select 1 as Result;

					END

				END
				else
				BEGIN

					select 0 as Result;

				END
			END
			ELSE
			BEGIN
				
				IF EXISTS (SELECT * FROM UserDetail WHERE Role = 'customer' AND UserName=@UserName)
				BEGIN
					select 3 as Result;
				END
				else
				BEGIN

						insert into UserDetail(UserName, PassWord, Role) values (@UserName,@PassWord,@Role);
						select 1 as Result;

				END
			
			END

		COMMIT;
	END TRY
	BEGIN CATCH  

		ROLLBACK TRANSACTION
		SELECT ERROR_MESSAGE() AS ErrorMessage;  
	
	END CATCH;
END
GO


