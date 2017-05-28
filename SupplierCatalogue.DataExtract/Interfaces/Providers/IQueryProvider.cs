// <copyright file="IQueryProvider.cs" company="Hitched Ltd">
// Copyright (c) Hitched Ltd. All rights reserved.
// </copyright>

namespace SupplierCatalogue.DataExtract.Interfaces.Providers
{
    /// <summary>
    /// Interface for the SQL query provider component
    /// </summary>
    public interface IQueryProvider
    {
        /// <summary>
        /// Fecth the SQL string for xml and json extracts
        /// </summary>
        /// <returns>SQL string for the extract</returns>
        string FetchSupplierQuery();

        /// <summary>
        /// Fecth the main SQL query for the Suppliers object extract
        /// </summary>
        /// <returns>SQL string for the main supplier object extract</returns>
        string FetchSupplierMainQuery();

        /// <summary>
        /// Fecth the SQL query for the Supplier listings extract
        /// </summary>
        /// <returns>SQL string for the main supplier listings extract</returns>
        string FetchSupplierListingQuery();

        /// <summary>
        /// Fecth the SQL query for the Supplier images extract
        /// </summary>
        /// <returns>SQL string for the main supplier images extract</returns>
        string FetchSupplierImagesQuery();

        /// <summary>
        /// Fecth the SQL query for the Supplier FAQs extract
        /// </summary>
        /// <returns>SQL string for the main supplier FAQs extract</returns>
        string FetchSupplierFaqsQuery();
    }
}
