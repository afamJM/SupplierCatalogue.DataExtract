// <copyright file="HitchedProvider.cs" company="Hitched Ltd">
// Copyright (c) Hitched Ltd. All rights reserved.
// </copyright>

namespace SupplierCatalogue.DataExtract.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using SupplierCatalogue.DataExtract.Configuration;
    using SupplierCatalogue.DataExtract.Database;
    using SupplierCatalogue.DataExtract.Interfaces.Providers;
    using SupplierCatalogue.DataExtract.Interfaces.Services;
    using SupplierCatalogue.Models;
    using SupplierCatalogue.Models.Requests;

    /// <summary>
    /// Hitched data provider class for the extract
    /// </summary>
    internal class HitchedProvider : BaseDataAccess, IHitchedProvider
    {
        private readonly IApiService apiService;
        private readonly IQueryProvider queryProvider;
        private readonly DBOptions database;
        private readonly ExtractOptions extract;

        /// <summary>
        /// Initializes a new instance of the <see cref="HitchedProvider"/> class.
        /// </summary>
        /// <param name="apiService">The Supplier API Service</param>
        /// <param name="queryProvider">The sql query provider object.</param>
        /// <param name="database">The database configuration settings object.</param>
        /// <param name="extract">The extract configuration settings object</param>
        public HitchedProvider(IApiService apiService, IQueryProvider queryProvider, IOptions<DBOptions> database, IOptions<ExtractOptions> extract)
        {
            this.apiService = apiService;
            this.queryProvider = queryProvider;
            this.database = database.Value;
            this.extract = extract.Value;
            this.ConnectionString = this.database.ConnectionString;
        }

        /// <summary>
        /// Fetch the supplier data in string form for output to a local file
        /// </summary>
        /// <returns>Supplier data formatted according to extract method.</returns>
        public string FetchSupplierData()
        {
            StringBuilder dataString = new StringBuilder();
            List<DbParameter> parameterList = new List<DbParameter>();

            string queryString = this.queryProvider.FetchSupplierQuery();
            parameterList.Add(this.GetParameter("RecordCount", this.extract.RecordCount));

            using (DbDataReader dataReader = this.GetDataReader(queryString, parameterList, CommandType.Text))
            {
                if (dataReader != null && dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        dataString.Append(dataReader[0]);
                        Console.Write(".");
                    }

                    Console.Write("\n");
                }
            }

            return dataString.ToString();
        }

        /// <summary>
        /// Perform an extract and submit of the supplier data as models to the API
        /// </summary>
        /// <returns>Boolean value to indicate success / failure</returns>
        public async Task<string> PerformSupplierObjectExtractAsync()
        {
            string someString = string.Empty;

            Console.Write("Beginning object extract....\n");

            string queryString = this.queryProvider.FetchSupplierMainQuery();
            List<DbParameter> parameterList = new List<DbParameter>
            {
                this.GetParameter("RecordCount", this.extract.RecordCount),
            };
            using (DbDataReader dataReader = this.GetDataReader(queryString, parameterList, CommandType.Text))
            {
                if (dataReader != null && dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        int supplierId = (int)dataReader["company_id"];
                        GenericSupplier generic_supplier = new GenericSupplier
                        {
                            Business = new Business
                            {
                                Name = dataReader["company_name"] == DBNull.Value ? string.Empty : (string)dataReader["company_name"],
                                EmailAddress = string.Empty,
                                Contact = new Person
                                {
                                    Title = string.Empty,
                                    FamilyName = dataReader["FirstName"] == DBNull.Value ? string.Empty : (string)dataReader["FirstName"],
                                    GivenName = dataReader["LastName"] == DBNull.Value ? string.Empty : (string)dataReader["LastName"],
                                    EmailAddress = string.Empty,
                                    JobTitle = string.Empty,
                                    ProfileImage = new Image(),
                                    SocialMediaPresence = new List<SocialMediaPresence>
                                    {
                                        new SocialMediaPresence
                                        {
                                            Network = "Facebook",
                                            Identifier = dataReader["facebook_username"] == DBNull.Value ? string.Empty : (string)dataReader["facebook_username"],
                                        },
                                        new SocialMediaPresence
                                        {
                                            Network = "Twitter",
                                            Identifier = dataReader["twitter_username"] == DBNull.Value ? string.Empty : (string)dataReader["twitter_username"],
                                        },
                                        new SocialMediaPresence
                                        {
                                            Network = "Pinterest",
                                            Identifier = dataReader["pinterest_username"] == DBNull.Value ? string.Empty : (string)dataReader["pinterest_username"],
                                        },
                                    },
                                },
                                Address = new Address
                                {
                                    Street = string.Empty,
                                    Town = string.Empty,
                                    County = string.Empty,
                                    Postcode = string.Empty,
                                },
                                PhoneNumber = string.Empty,
                            },
                            Category = string.Empty,
                            Website = null,
                            Location = new Location
                            { },
                            Locations = new List<Location>(),
                            StarRating = 0,
                            Reviews = new List<Review>(),
                            Description = string.Empty,
                            Faqs = new List<Faq>(),
                            CoverImage = new Image
                            {
                                Height = 0,
                                Width = 0,
                                Name = string.Empty,
                                Path = dataReader["cover_image"] == DBNull.Value || ((string)dataReader["cover_image"]).Equals(string.Empty) ? null : new UriBuilder((string)dataReader["cover_image"]).Uri,
                            },
                            ProfileImage = new Image
                            {
                                Height = 0,
                                Width = 0,
                                Name = string.Empty,
                                Path = dataReader["cover_image"] == DBNull.Value || ((string)dataReader["cover_image"]).Equals(string.Empty) ? null : new UriBuilder((string)dataReader["cover_image"]).Uri,
                            },
                            Images = new List<Image>(),
                            ListingType = string.Empty,
                            SocialMediaPresence = new List<SocialMediaPresence>
                            {
                                new SocialMediaPresence
                                {
                                    Network = "Facebook",
                                    Identifier = dataReader["facebook_username"] == DBNull.Value ? string.Empty : (string)dataReader["facebook_username"],
                                },
                                new SocialMediaPresence
                                {
                                    Network = "Twitter",
                                    Identifier = dataReader["twitter_username"] == DBNull.Value ? string.Empty : (string)dataReader["twitter_username"],
                                },
                                new SocialMediaPresence
                                {
                                    Network = "Pinterest",
                                    Identifier = dataReader["pinterest_username"] == DBNull.Value ? string.Empty : (string)dataReader["pinterest_username"],
                                },
                            },
                            SpecialOffers = new List<SpecialOffer>(),
                            Treatments = new List<string>(),
                            Designers = new List<Designer>(),
                            Styles = new List<string>(),
                        };

                        string listingQueryString = this.queryProvider.FetchSupplierListingQuery();
                        List<DbParameter> listingParameterList = new List<DbParameter>
                        {
                            this.GetParameter("SupplierId", supplierId),
                        };
                        using (DbDataReader listingDataReader = this.GetDataReader(listingQueryString, listingParameterList, CommandType.Text))
                        {
                            if (listingDataReader != null && listingDataReader.HasRows)
                            {
                                bool firstRow = true;
                                List<Location> supplierLocations = new List<Location>();
                                while (listingDataReader.Read())
                                {
                                    Location supplierLocation = new Location();
                                    if (firstRow)
                                    {
                                        generic_supplier.Description = listingDataReader["company_description"] == DBNull.Value ? string.Empty : (string)listingDataReader["company_description"];
                                        generic_supplier.Business.EmailAddress = listingDataReader["email"] == DBNull.Value ? string.Empty : (string)listingDataReader["email"];
                                        generic_supplier.Business.Address.Street = listingDataReader["address"] == DBNull.Value ? string.Empty : (string)listingDataReader["address"];
                                        generic_supplier.Business.Address.Town = listingDataReader["street"] == DBNull.Value ? string.Empty : (string)listingDataReader["street"];
                                        generic_supplier.Business.Address.County = listingDataReader["county_name"] == DBNull.Value ? string.Empty : (string)listingDataReader["county_name"];
                                        generic_supplier.Business.Address.Postcode = listingDataReader["post_code"] == DBNull.Value ? string.Empty : (string)listingDataReader["post_code"];
                                        generic_supplier.Business.PhoneNumber = listingDataReader["telephone"] == DBNull.Value ? string.Empty : (string)listingDataReader["telephone"];
                                        generic_supplier.Website = listingDataReader["url"] == DBNull.Value || ((string)listingDataReader["url"]).Equals(string.Empty) ? null : new UriBuilder((string)listingDataReader["url"]).Uri;

                                        int categoryId = listingDataReader["category_id"] == DBNull.Value ? 0 : (int)listingDataReader["category_id"];
                                        string categoryName;
                                        switch (categoryId)
                                        {
                                            case 4:
                                                categoryName = "beauty";
                                                break;
                                            case 8:
                                                categoryName = "bridalwear";
                                                break;
                                            case 9:
                                                categoryName = "cake";
                                                break;
                                            case 22:
                                                categoryName = "florist";
                                                break;
                                            case 31:
                                                categoryName = "music";
                                                break;
                                            case 33:
                                                categoryName = "photographer";
                                                break;
                                            case 42:
                                                categoryName = "videographer";
                                                break;
                                            default:
                                                categoryName = "Unknown";
                                                break;
                                        }

                                        generic_supplier.Category = categoryName;

                                        firstRow = false;
                                    }

                                    supplierLocation.Name = supplierLocation.County = listingDataReader["county_name"] == DBNull.Value ? string.Empty : (string)listingDataReader["county_name"];
                                    supplierLocation.GeoCode = new GeoCode
                                    {
                                        Latitude = 0,
                                        Longitude = 0,
                                    };
                                    supplierLocations.Add(supplierLocation);
                                }

                                generic_supplier.Locations = supplierLocations;
                            }
                        }

                        string imagesQueryString = this.queryProvider.FetchSupplierImagesQuery();
                        List<DbParameter> imageParameterList = new List<DbParameter>
                        {
                            this.GetParameter("SupplierId", supplierId),
                        };
                        using (DbDataReader imagesDataReader = this.GetDataReader(imagesQueryString, imageParameterList, CommandType.Text))
                        {
                            if (imagesDataReader != null && imagesDataReader.HasRows)
                            {
                                List<Image> supplierImages = new List<Image>();
                                while (imagesDataReader.Read())
                                {
                                    Image supplierImage = new Image
                                    {
                                        Name = imagesDataReader["name"] == DBNull.Value ? string.Empty : (string)imagesDataReader["name"],
                                        Path = imagesDataReader["path"] == DBNull.Value || ((string)imagesDataReader["path"]).Equals(string.Empty) ? null : new UriBuilder((string)imagesDataReader["path"]).Uri,
                                        Height = imagesDataReader["height"] == DBNull.Value ? 0 : (int)imagesDataReader["height"],
                                        Width = imagesDataReader["width"] == DBNull.Value ? 0 : (int)imagesDataReader["width"],
                                    };

                                    supplierImages.Add(supplierImage);
                                }

                                generic_supplier.Images = supplierImages;
                            }
                        }

                        string faqsQueryString = this.queryProvider.FetchSupplierFaqsQuery();
                        List<DbParameter> faqsParameterList = new List<DbParameter>
                        {
                            this.GetParameter("SupplierId", supplierId),
                        };
                        using (DbDataReader faqsDataReader = this.GetDataReader(faqsQueryString, faqsParameterList, CommandType.Text))
                        {
                            if (faqsDataReader != null && faqsDataReader.HasRows)
                            {
                                List<Faq> supplierFaqs = new List<Faq>();
                                while (faqsDataReader.Read())
                                {
                                    Faq supplierFaq = new Faq
                                    {
                                        Question = faqsDataReader["question"] == DBNull.Value ? string.Empty : (string)faqsDataReader["question"],
                                        Answer = faqsDataReader["answer"] == DBNull.Value ? string.Empty : (string)faqsDataReader["answer"],
                                    };

                                    supplierFaqs.Add(supplierFaq);
                                }

                                generic_supplier.Faqs = supplierFaqs;
                            }
                        }

                        Console.Write(".");

                        // Post the new supplier to the Api
                        await this.apiService.CreateSupplierAsync(generic_supplier);
                    }
                }
            }

            Console.Write("\n");

            return someString;
        }
    }
}
