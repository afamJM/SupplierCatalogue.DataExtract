Select	images.Title		As "name",
		images.ImagePath	As "path",
		images.Height		As "height",
		images.Width		As "width"
From MediaGallery.SupplierImages As images
Where images.CompanyID = (@SupplierId)