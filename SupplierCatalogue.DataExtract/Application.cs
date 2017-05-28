// <copyright file="Application.cs" company="Hitched Ltd">
// Copyright (c) Hitched Ltd. All rights reserved.
// </copyright>

namespace SupplierCatalogue.DataExtract
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using SupplierCatalogue.DataExtract.Configuration;
    using SupplierCatalogue.DataExtract.Interfaces.Providers;

    /// <summary>
    /// The application class for the data extract.
    /// </summary>
    public class Application
    {
        private ILogger logger;
        private ApplicationOptions application;
        private ExtractOptions extract;
        private IHitchedProvider hitched;

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        /// <param name="logger">The system error logging object</param>
        /// <param name="application">The application configuration settings object</param>
        /// <param name="extract">The extract configuration settings object</param>
        /// <param name="hitched">The hitched data provider object</param>
        public Application(ILogger<Application> logger, IOptions<ApplicationOptions> application, IOptions<ExtractOptions> extract, IHitchedProvider hitched)
        {
            this.logger = logger;
            this.application = application.Value;
            this.extract = extract.Value;
            this.hitched = hitched;
        }

        /// <summary>
        /// Initate the main extract function of the application
        /// </summary>
        public void Run()
        {
            try
            {
                switch (this.extract.ExtractMode)
                {
                    case "json":
                    case "xml":
                        this.logger.LogInformation($"This is a console application for {this.application.Name} v{this.application.Version}");
                        this.logger.LogInformation("Generating Supplier Data");

                        string supplierData = this.hitched.FetchSupplierData();
                        if (supplierData.Length > 0)
                        {
                            string outputFile = string.Empty;
                            if (this.extract.SuppliersOutFilePath.Length > 0)
                            {
                                outputFile += this.extract.SuppliersOutFilePath;
                            }

                            outputFile += this.extract.SuppliersOutFileName + "." + this.extract.ExtractMode;
                            File.WriteAllText(outputFile, supplierData);
                            this.logger.LogInformation("Supplier Data Created in : " + outputFile);
                        }
                        else
                        {
                            this.logger.LogWarning("Failed To Process");
                        }

                        break;
                    default:
                        Task.Run(() => this.hitched.PerformSupplierObjectExtractAsync()).Wait();
                        break;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.ToString());
            }
        }
    }
}
