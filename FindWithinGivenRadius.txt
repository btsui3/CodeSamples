
ALTER proc [dbo].[Products_SelectByCategoryId]
@searchQuery nvarchar(150) = '',
@CategoryId int,
@latpoint decimal(9, 6),
@lngpoint decimal(9, 6),
@radius float(2)

AS



BEGIN

SELECT
       [Id]
      ,[CompanyId]     
      ,[Name]
      ,[Category]
      ,[Description]
      ,[Cost]
      ,[Quantity]
      ,[Threshold]
      ,[MinPurchase]
      ,[mediaId]
      ,[CategoryId]
      ,[Latitude]
      ,[Longitude]	 
      ,[Address1]
      ,[City]	  
      ,[State]
      ,[ZipCode]
      ,distance_in_mi
	  

FROM (

SELECT
       Products.[Id]
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

	(Products.Description LIKE '%'+ @searchQuery +'%')

	AND

	(CategoryId = @CategoryId)


) AS d

WHERE distance_in_mi <= @radius

ORDER BY distance_in_mi

END




/* 
%%%%%%%%%%  TEST CODE  %%%%%%%%%%%%%%%%


Declare 
@searchQuery nvarchar(150) = '1',
@CategoryId int = 30000,
@latpoint decimal(9, 6) = 33.6116239,
@lngpoint decimal(9, 6) = -117.8766065,
@radius float(2) = 100;

Execute dbo.Products_SelectByCategoryId
@searchQuery
,@CategoryId 
,@latpoint
,@lngpoint 
,@radius


*/