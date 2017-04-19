USE [C27]
GO
/****** Object:  StoredProcedure [dbo].[Suppliers_GetByCategoryIdAndRadiusAndDesignation]    Script Date: 4/18/2017 5:49:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER proc [dbo].[Suppliers_GetByCategoryIdAndRadiusAndDesignation]

@CategoryId int,
@latpoint decimal(9, 6),
@lngpoint decimal(9, 6),
@radius float(2),
@designations int

AS

/* 

Declare 
@CategoryId int = 30000,
@latpoint decimal(9, 6) = 33.6116239,
@lngpoint decimal(9, 6) = -117.8766065,
@radius float(2) = 100,
@designations int = 0

Execute dbo.Suppliers_GetByCategoryIdAndRadiusAndDesignation
@CategoryId
,@latpoint
,@lngpoint 
,@radius
,@designations


*/


BEGIN

SELECT 
			 [companyName]
			,[designations]
			,[url]
			,[email]
			,[mediaUrl]
			,[phone]
			,[CompanyId]     
      ,[name] as productName
      ,[Category]
      ,[Description] as productDescription
      ,[Cost]
      ,[Quantity]
      ,[Threshold]
      ,[MinPurchase]
      ,[mediaId] as productMediaId
      ,[CategoryId]
			,[Latitude]
			,[Longitude]	 
			,[Address1]
			,[City]	  
			,[State]
			,[ZipCode]
			,[distance_in_mi]


FROM (
SELECT
			 c.[name] AS companyName
			,c.[designations] AS designations
			,c.[url] AS url
			,c.[email] as email
			,(SELECT url FROM [Media] WHERE c.mediaId = [Media].id) AS mediaUrl
			,c.phoneNumber as phone
			,Products.[CompanyId]
      ,Products.[Name]
      ,Products.[Category]
      ,Products.[Description]
      ,Products.[Cost]
      ,Products.[Quantity]
      ,Products.[Threshold]
      ,Products.[MinPurchase]
      ,Products.[mediaId]
      ,Products.[CategoryId]
			,a.[Latitude]
			,a.[Longitude]
			,a.[Address1]
			,a.[City]
			,a.[ZipCode]
			,a.[State]
			,p.distance_unit
	 
                 * DEGREES(ACOS(COS(RADIANS(p.latpoint))
                 * COS(RADIANS([Latitude]))
                 * COS(RADIANS(p.longpoint) - RADIANS(a.[Longitude]))
                 + SIN(RADIANS(p.latpoint))
                 * SIN(RADIANS(a.[Latitude])))) AS distance_in_mi


  FROM [dbo].[Products] LEFT JOIN [dbo].[Addresses] AS a

  ON Products.AddressId = a.Id 

  LEFT JOIN [Company] AS c




	ON c.id = [Products].companyId

	JOIN (
					SELECT  @latpoint  AS latpoint,  @lngpoint AS longpoint,
									@radius AS radius,       69.0 AS distance_unit

				) AS p ON 1=1


	WHERE 
 
	(a.[Latitude] BETWEEN p.latpoint  - (p.radius / p.distance_unit)

		AND p.latpoint  + (p.radius / p.distance_unit)
		AND a.[Longitude] BETWEEN p.longpoint - (p.radius / (p.distance_unit * COS(RADIANS(p.latpoint))))
		AND p.longpoint + (p.radius / (p.distance_unit * COS(RADIANS(p.latpoint))))
	)

	AND

	(CategoryId = @CategoryId)

) AS d

WHERE distance_in_mi <= @radius


AND 
    --(designations & IsNull(@designations, 1)) > 0

    (
        (@designations = 0)
        OR 
        (@designations != 0 AND ((designations & @designations) > 0))
    )

ORDER BY distance_in_mi

END