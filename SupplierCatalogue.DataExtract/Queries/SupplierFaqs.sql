Select	FAQs.Question As "question",
		FAQs.Answer As "answer"
From Supplier.FAQs As FAQs
Where FAQs.CompanyID = (@SupplierId)
Order By FAQs.SortOrder