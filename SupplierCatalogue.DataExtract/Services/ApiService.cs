// <copyright file="IApiService.cs" company="Hitched Ltd">
// Copyright (c) Hitched Ltd. All rights reserved.
// </copyright>

namespace SupplierCatalogue.DataExtract.Services
{
    using System;
    using System.Net.Http;
    using System.Runtime.Serialization.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using SupplierCatalogue.DataExtract.Configuration;
    using SupplierCatalogue.DataExtract.Interfaces.Services;
    using SupplierCatalogue.Models.Requests;

    /// <summary>
    /// Provides access to the Supplier Catalogue API
    /// </summary>
    public class ApiService : IApiService
    {
        private static HttpClient client;
        private readonly ExtractOptions extract;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiService"/> class.
        /// The Http service provider for the Supplier Catalogue Data Extract
        /// </summary>
        /// <param name="extract">The extract configuration settings object</param>
        public ApiService(IOptions<ExtractOptions> extract)
        {
            this.extract = extract.Value;

            // Create a handler to ensure redirects are not followed
            var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
            };

            // Create the HttpClient with the new handler
            client = new HttpClient(handler)
            {
                BaseAddress = new UriBuilder(this.extract.ApiDestination).Uri,
            };
        }

        /// <summary>
        /// Post an item to the Supplier Catalogue Api
        /// </summary>
        /// <param name="supplier">The generic supplier model to post to the API</param>
        /// <returns>Http response code</returns>
        public async Task<bool> CreateSupplierAsync(GenericSupplier supplier)
        {
            // Generate the JSON string from the generic supplier object
            var serializer = new DataContractJsonSerializer(typeof(GenericSupplier));
            var artifact = new System.IO.MemoryStream();
            serializer.WriteObject(artifact, supplier);
            var content = new StringContent(System.Text.Encoding.UTF8.GetString(artifact.ToArray()), System.Text.Encoding.UTF8, "application/json");

            // Post to the Api
            HttpResponseMessage response = await client.PostAsync("suppliers", content);

            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }
    }
}