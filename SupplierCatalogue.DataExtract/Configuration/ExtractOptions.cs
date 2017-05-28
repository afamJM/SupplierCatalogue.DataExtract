// <copyright file="ExtractOptions.cs" company="Hitched Ltd">
// Copyright (c) Hitched Ltd. All rights reserved.
// </copyright>

namespace SupplierCatalogue.DataExtract.Configuration
{
    /// <summary>
    /// The extract configuration options interface
    /// </summary>
    public class ExtractOptions
    {
        /// <summary>
        /// Gets or sets the extract mode for the application
        /// </summary>
        public string ExtractMode { get; set; }

        /// <summary>
        /// Gets or sets the output file path for the application
        /// </summary>
        public string SuppliersOutFilePath { get; set; }

        /// <summary>
        /// Gets or sets the output file name for the application
        /// </summary>
        public string SuppliersOutFileName { get; set; }

        /// <summary>
        /// Gets or sets the ectract record count for the application
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// Gets or sets the api destination for the object extract and send
        /// </summary>
        public string ApiDestination { get; set; }
    }
}
