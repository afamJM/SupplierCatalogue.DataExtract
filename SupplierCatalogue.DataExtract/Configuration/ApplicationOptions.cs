// <copyright file="ApplicationOptions.cs" company="Hitched Ltd">
// Copyright (c) Hitched Ltd. All rights reserved.
// </copyright>

namespace SupplierCatalogue.DataExtract.Configuration
{
    using System.Reflection;

    /// <summary>
    /// The application configuration options interface
    /// </summary>
    public class ApplicationOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationOptions"/> class.
        /// </summary>
        public ApplicationOptions()
        {
            this.Assembly = typeof(Program).GetTypeInfo().Assembly;
        }

        /// <summary>
        /// Gets or sets the application name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the application version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the application assembly
        /// </summary>
        public Assembly Assembly { get; set; }
    }
}
