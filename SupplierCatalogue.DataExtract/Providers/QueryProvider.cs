// <copyright file="QueryProvider.cs" company="Hitched Ltd">
// Copyright (c) Hitched Ltd. All rights reserved.
// </copyright>

namespace SupplierCatalogue.DataExtract.Providers
{
    using System.IO;
    using System.Reflection;
    using Microsoft.Extensions.Options;
    using SupplierCatalogue.DataExtract.Configuration;
    using SupplierCatalogue.DataExtract.Interfaces.Providers;

    /// <summary>
    /// Provides SQL strings for the main extract
    /// </summary>
    public class QueryProvider : IQueryProvider
    {
        private readonly ApplicationOptions application;
        private readonly ExtractOptions extract;
        private readonly Assembly assembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryProvider"/> class.
        /// </summary>
        /// <param name="application">The application configuration settings object</param>
        /// <param name="extract">The extract configuration options object</param>
        public QueryProvider(IOptions<ApplicationOptions> application, IOptions<ExtractOptions> extract)
        {
            this.extract = extract.Value;
            this.application = application.Value;
            this.assembly = this.application.Assembly;
        }

        /// <summary>
        /// Fecth the SQL string for xml and json extracts
        /// </summary>
        /// <returns>SQL string for the extract</returns>
        public string FetchSupplierQuery()
        {
            Stream queryStream;
            string fileType = this.extract.ExtractMode.Equals("xml") ? "xml" : "json";
            string version = this.extract.RecordCount > 0 ? "Suppliers" : "SuppliersNoLimit";

            queryStream = this.assembly.GetManifestResourceStream("SupplierCatalogue.DataExtract.Queries." + fileType + "." + version + ".sql");

            StreamReader queryStreamReader = new StreamReader(queryStream);

            return queryStreamReader.ReadToEnd();
        }

        /// <summary>
        /// Fecth the main SQL query for the Suppliers object extract
        /// </summary>
        /// <returns>SQL string for the main supplier object extract</returns>
        public string FetchSupplierMainQuery()
        {
            Stream queryStream;

            if (this.extract.RecordCount > 0)
            {
                queryStream = this.assembly.GetManifestResourceStream("SupplierCatalogue.DataExtract.Queries.SuppliersMain.sql");
            }
            else
            {
                queryStream = this.assembly.GetManifestResourceStream("SupplierCatalogue.DataExtract.Queries.SuppliersMainNoLimit.sql");
            }

            StreamReader queryStreamReader = new StreamReader(queryStream);

            return queryStreamReader.ReadToEnd();
        }

        /// <summary>
        /// Fecth the SQL query for the Supplier listings extract
        /// </summary>
        /// <returns>SQL string for the main supplier listings extract</returns>
        public string FetchSupplierListingQuery()
        {
            Stream queryStream = this.assembly.GetManifestResourceStream("SupplierCatalogue.DataExtract.Queries.SupplierListings.sql");

            StreamReader queryStreamReader = new StreamReader(queryStream);

            return queryStreamReader.ReadToEnd();
        }

        /// <summary>
        /// Fecth the SQL query for the Supplier images extract
        /// </summary>
        /// <returns>SQL string for the main supplier images extract</returns>
        public string FetchSupplierImagesQuery()
        {
            Stream queryStream = this.assembly.GetManifestResourceStream("SupplierCatalogue.DataExtract.Queries.SupplierImages.sql");

            StreamReader queryStreamReader = new StreamReader(queryStream);

            return queryStreamReader.ReadToEnd();
        }

        /// <summary>
        /// Fecth the SQL query for the Supplier FAQs extract
        /// </summary>
        /// <returns>SQL string for the main supplier FAQs extract</returns>
        public string FetchSupplierFaqsQuery()
        {
            Stream queryStream = this.assembly.GetManifestResourceStream("SupplierCatalogue.DataExtract.Queries.SupplierFaqs.sql");

            StreamReader queryStreamReader = new StreamReader(queryStream);

            return queryStreamReader.ReadToEnd();
        }
    }
}
