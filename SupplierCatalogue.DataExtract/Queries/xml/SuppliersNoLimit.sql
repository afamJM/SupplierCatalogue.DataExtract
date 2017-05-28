WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/SupplierCatalogue.API.Models.Requests' AS xsd),
Suppliers ( company_id, company_name, FirstName, LastName, facebook_username, twitter_username, pinterest_username, cover_image, areas_covered, imageURLs, faqs ) As (
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
			(Select	SS1.county_name  AS "Name",
					SS1.county_name AS "County",
					0.0 AS "xsd:Geocode/Latitude",
					0.0 AS "xsd:Geocode/Longitude"
			From dbo.GetLiveListings SS1
			Where company.company_id = SS1.company_id
			ORDER BY SS1.county_name
			For XML PATH('xsd:Location'), TYPE )
		) [areas_covered],
		(	
			(SELECT	images.Title AS "Name",
					images.ImagePath AS "Path",
					images.Height AS "Height",
					images.Width As "Width"
			FROM MediaGallery.SupplierImages AS images
			WHERE images.CompanyID = company.company_id
			For XML PATH('xsd:Image'), TYPE )
		) [imageURLs],
		(	
			(Select	FAQs.Question As "question",
					FAQs.Answer As "answer"
			From Supplier.FAQs As FAQs
			Where FAQs.CompanyID = company.company_id
			Order By FAQs.SortOrder
			For XML PATH('xsd:Faq'), TYPE )
		) [faqs]
	From dbo.tbl_companymaster company
	Where Exists (Select 1
				From dbo.GetLiveListings SS2
				Where SS2.company_id = company.company_id
				And SS2.category_id In (4, 8, 9, 22, 31, 33, 42))
)
Select
	Suppliers.company_id AS "xsd:Business/Company_id",
	Suppliers.company_name AS "xsd:Business/Name",
	Suppliers.FirstName AS "xsd:Business/xsd:Contact/GivenName",
	Suppliers.LastName AS "xsd:Business/xsd:Contact/FamilyName",
	Listings.email AS "xsd:Business/xsd:Contact/EmailAddress",
	Listings.telephone AS "xsd:Business/PhoneNumber",
	Listings.address AS "xsd:Business/xsd:Address/Street",
	Listings.street AS "xsd:Business/xsd:Address/Town",
	Listings.state AS "xsd:Business/xsd:Address/County",
	Listings.post_code AS "xsd:Business/xsd:Address/PostCode",
	Listings.email AS "xsd:Business/EmailAddress",
	Listings.county_name AS "xsd:Location/Name",
	Listings.county_name AS "xsd:Location/County",
	Listings.country_name AS "xsd:Location/Country",
	0.0 AS "xsd:Location/xsd:Geocode/Latitude",
	0.0 AS "xsd:Location/xsd:Geocode/Longitude",
    CASE                       

        WHEN Listings.category_id = 4 THEN 'beauty'
		WHEN Listings.category_id = 8 THEN 'bridalwear'
		WHEN Listings.category_id = 9 THEN 'cake'
		WHEN Listings.category_id = 22 THEN 'florist'
		WHEN Listings.category_id = 31 THEN 'music'
		WHEN Listings.category_id = 33 THEN 'photographer'
		WHEN Listings.category_id = 42 THEN 'videographer'
		ELSE 'unknown'

    END AS "Category",
	Listings.url AS "Website",
	Listings.company_description AS "Description",
	Suppliers.facebook_username AS "FacebookIdentifier",
	Suppliers.twitter_username AS "TwitterIdentifier",
	Suppliers.pinterest_username AS "PinterestIdentifier",
	Suppliers.areas_covered AS "Locations",
	Suppliers.cover_image AS "xsd:CoverImage/Path",
	Suppliers.imageURLs AS "Images",
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
For XML PATH ('xsd:GenericSupplier')