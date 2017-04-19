USE [C27]
GO
/****** Object:  StoredProcedure [dbo].[QuoteRequestsMedia_SelectAllByQuoteRequestId]    Script Date: 4/18/2017 7:12:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER proc [dbo].[QuoteRequestsMedia_SelectAllByQuoteRequestId]
@QuoteRequestsId int

as

/*

Declare @QuoteRequestsId int = 117

Execute dbo.QuoteRequestsMedia_SelectAllByQuoteRequestId
@QuoteRequestsId 


*/


BEGIN


	
	SELECT 


       [QuoteRequestId]
      ,[MediaId]
		
	FROM [dbo].[QuoteRequestsMedia]

	WHERE QuoteRequestId = @QuoteRequestsId 



END