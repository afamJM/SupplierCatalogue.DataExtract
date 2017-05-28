// <copyright file="IApiService.cs" company="Hitched Ltd">
// Copyright (c) Hitched Ltd. All rights reserved.
// </copyright>

namespace SupplierCatalogue.DataExtract.Interfaces.Services
{
    using System;
    using System.Threading.Tasks;
    using SupplierCatalogue.Models.Requests;

    /// <summary>
    /// Provides access to the Supplier Catalogue API
    /// </summary>
    public interface IApiService
    {
        /// <summary>
        /// Post an item to the Supplier Catalogue Api
        /// </summary>
        /// <param name="supplier">The generic supplier model to post to the API</param>
        /// <returns>Http response code</returns>
        Task<bool> CreateSupplierAsync(GenericSupplier supplier);
    }
}
