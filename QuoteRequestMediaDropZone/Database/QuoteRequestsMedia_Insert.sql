USE [C27]
GO
/****** Object:  StoredProcedure [dbo].[QuoteRequestsMedia_Insert]    Script Date: 4/18/2017 7:12:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER proc [dbo].[QuoteRequestsMedia_Insert] 

			@QuoteRequestId int 
			, @MediaId int 
			

/*



Declare  
		   @QuoteRequestId int = 139
		 , @MediaId int = 196


Execute dbo.QuoteRequestsMedia_Insert

			  @QuoteRequestId 
			, @MediaId  
			

			Select @QuoteRequestId
			Select *
			From dbo.QuoteRequestsMedia
			Where QuoteRequestId = @QuoteRequestId

*/


as


BEGIN





	

INSERT INTO [dbo].[QuoteRequestsMedia]
           ([QuoteRequestId]
           ,[MediaId])
		   
    
	 VALUES
           (@QuoteRequestId 
           ,@MediaId)






END

