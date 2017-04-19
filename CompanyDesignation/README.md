# CompanyDesignation
Use bitmasking to designate whether a company is a small business, veteran-owned, minority-owned, women-owned, or none of the above. 

If a user were to click on any of these options, the "checked" option would be assigned a value of "1" and and "un-checked" value would be assigned a value of "0". 

	None = 0,          --> 00000
	SmallBusiness = 1, --> 00001
	VeteranOwned = 2,  --> 00010
	MinorityOwned = 4, --> 00100
	WomenOwned = 8 		 --> 01000

For example, if a company was designated as "SmallBusiness (1)", "VeteranOwned (2)", MinorityOwned (4)", and "WomenOwned (8)", that would give a numerical value of "15". 15 in binary is "01111".

		00001
		00010
		00100
	+	01000   
----------------
	=	01111	 --> 15
 
 
 Bitmasking allows us to use fewer permutations of company data in our database, which allows easier access and manipulation of company data. 
