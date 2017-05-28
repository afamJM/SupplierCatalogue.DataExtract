With Suppliers ( company_id, company_name, FirstName, LastName, facebook_username, twitter_username, pinterest_username, cover_image, areas_covered, imageURLs, FAQs ) As (
	Select
		company.company_id,
		company.company_name,
		company.FirstName,
		company.LastName,
		company.facebook_username,
		company.twitter_username,
		company.pinterest_username,
		company.CachedSearchImageUrl As cover_image,
		(
			Select	SS1.county_name  AS "name",
					SS1.county_name AS "county",
					0.0 AS "geocode.latitude",
					0.0 AS "geocode.longitude"
			From dbo.GetLiveListings SS1
			Where company.company_id = SS1.company_id
			ORDER BY SS1.county_name
			For JSON PATH
		) [areas_covered],
		(	SELECT	images.Title AS "name",
					images.ImagePath AS "path",
					images.Height AS "height",
					images.Width As "width"
			FROM MediaGallery.SupplierImages AS images
			WHERE images.CompanyID = company.company_id
			FOR JSON PATH
		) [imageURLs],
		(	Select	FAQs.Question As "question",
					FAQs.Answer As "answer"
			From Supplier.FAQs As FAQs
			Where FAQs.CompanyID = company.company_id
			Order By FAQs.SortOrder
			For JSON PATH
		) [faqs]
	From dbo.tbl_companymaster company
	Where Exists (Select 1
				From dbo.GetLiveListings SS2
				Where SS2.company_id = company.company_id
				And SS2.category_id In (4, 8, 9, 22, 31, 33, 42))
)
Select
	Suppliers.company_id AS "business.company_id",
	Suppliers.company_name AS "business.name",
	Suppliers.FirstName AS "business.contact.givenName",
	Suppliers.LastName AS "business.contact.familyName",
	Listings.email AS "business.contact.emailAddress",
	Listings.telephone AS "business.phoneNumber",
	Listings.address AS "business.address.street",
	Listings.street AS "business.address.town",
	Listings.state AS "business.address.county",
	Listings.post_code AS "business.address.postCode",
	Listings.email AS "business.emailAddress",
	Listings.county_name AS "location.name",
	Listings.county_name AS "location.county",
	Listings.country_name AS "location.country",
	0.0 AS "location.geocode.latitude",
	0.0 AS "location.geocode.longitude",
    CASE                       

        WHEN Listings.category_id = 4 THEN 'beauty'
		WHEN Listings.category_id = 8 THEN 'bridalwear'
		WHEN Listings.category_id = 9 THEN 'cake'
		WHEN Listings.category_id = 22 THEN 'florist'
		WHEN Listings.category_id = 31 THEN 'music'
		WHEN Listings.category_id = 33 THEN 'photographer'
		WHEN Listings.category_id = 42 THEN 'videographer'
		ELSE 'unknown'

    END AS "category",
	Listings.url AS "website",
	Listings.company_description AS "description",
	Suppliers.facebook_username AS "facebookIdentifier",
	Suppliers.twitter_username AS "twitterIdentifier",
	Suppliers.pinterest_username AS "pinterestIdentifier",
	Suppliers.areas_covered AS "locations",
	Suppliers.cover_image AS "coverImage.path",
	Suppliers.imageURLs AS "images",
	Suppliers.FAQs AS "FAQs"
From Suppliers
CROSS APPLY
    (
    SELECT  TOP 1 
		SS3.email, 
		SS3.telephone,
		SS3.address,
		SS3.street,
		SS3.city,
		SS3.state,
		SS3.post_code,
		SS3.county_name,
		SS3.country_name,
		SS3.category_id,
		SS3.category_name,
		SS3.company_description,
		SS3.url
    FROM    dbo.GetLiveListings As SS3
    WHERE   SS3.company_id = Suppliers.company_id
    ) Listings
For JSON PATH