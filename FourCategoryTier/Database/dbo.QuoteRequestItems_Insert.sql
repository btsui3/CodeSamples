ALTER proc [dbo].[QuoteRequestItems_Insert]
		  @QuoteRequestItemId int
		, @QuoteRequestId int
		, @Name nvarchar(256) 
		, @Estimation int
		, @Quantity int
		, @Unit nvarchar(256)
		, @Id int OUTPUT

AS


BEGIN


INSERT INTO [dbo].[QuoteRequestItems]
		   (
		     [QuoteRequestItemId]
		   , [QuoteRequestId]
		   , [Name]
		   , [Estimation]
		   , [Quantity]
		   , [Unit]
		
           )

     VALUES
           (
		     @QuoteRequestItemId
		   , @QuoteRequestId
		   , @Name
		   , @Estimation
		   , @Quantity
		   , @Unit
		   )

		   
SET @Id = SCOPE_IDENTITY()

END


/* %%%%%%%%%%%%%%%%%%%   TEST CODE       %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

Declare 

			 @Id int = 0
		   , @QuoteRequestItemId int = 666
		   , @QuoteRequestId int = 777
		   , @Name nvarchar(256) = 'Tooled Concrete Finishing'
		   , @Estimation int = 3
		   , @Quantity int= 12
		   , @Unit nvarchar(256) = 'lbs'
		  

Execute dbo.QuoteRequestItems_Insert

		     @QuoteRequestItemId
		   , @QuoteRequestId
		   , @Name
		   , @Estimation
		   , @Quantity
		   , @Unit
		   , @Id OUTPUT

SELECT
		     [QuoteRequestItemId]
		   , [QuoteRequestId]
		   , [Name]
		   , [Estimation]
		   , [Quantity]
		   , [Unit]
		   , [Id]
		

From [dbo].[QuoteRequestItems]

WHERE Id = @Id

*/