// <copyright file="IHitchedProvider.cs" company="Hitched Ltd">
// Copyright (c) Hitched Ltd. All rights reserved.
// </copyright>

namespace SupplierCatalogue.DataExtract.Interfaces.Providers
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for the Hitched data provider component
    /// </summary>
    public interface IHitchedProvider
    {
        /// <summary>
        /// Fetch the supplier data in string form for output to a local file
        /// </summary>
        /// <returns>Supplier data formatted according to extract method.</returns>
        string FetchSupplierData();

        /// <summary>
        /// Perform an extract and submit of the supplier data as models to the API
        /// </summary>
        /// <returns>Boolean value to indicate success / failure</returns>
        Task<string> PerformSupplierObjectExtractAsync();
    }
}
