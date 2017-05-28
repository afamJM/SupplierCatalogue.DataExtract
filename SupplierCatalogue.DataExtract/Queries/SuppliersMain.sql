Select Top (@RecordCount)
	company.company_id,
	company.company_name,
	company.FirstName,
	company.LastName,
	company.facebook_username,
	company.twitter_username,
	company.pinterest_username,
	company.CachedSearchImageUrl As cover_image
From dbo.tbl_companymaster company
Where Exists (Select 1
			From dbo.GetLiveListings listing
			Where listing.company_id = company.company_id
			And listing.category_id In (4, 8, 9, 22, 31, 33, 42))