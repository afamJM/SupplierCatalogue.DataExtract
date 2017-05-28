Select
	listing.email, 
	listing.telephone,
	listing.address,
	listing.street,
	listing.city,
	listing.state,
	listing.post_code,
	listing.county_name,
	listing.country_name,
	listing.category_id,
	listing.category_name,
	listing.company_description,
	listing.url
From dbo.GetLiveListings listing
Where listing.company_id = (@SupplierId)