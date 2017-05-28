// <copyright file="Program.cs" company="Hitched Ltd">
// Copyright (c) Hitched Ltd. All rights reserved.
// </copyright>

namespace SupplierCatalogue.DataExtract
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using SupplierCatalogue.DataExtract.Configuration;
    using SupplierCatalogue.DataExtract.Interfaces.Providers;
    using SupplierCatalogue.DataExtract.Interfaces.Services;
    using SupplierCatalogue.DataExtract.Providers;
    using SupplierCatalogue.DataExtract.Services;

    /// <summary>
    /// The main program controller class for the data extract console application
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entry point for the Hitched supplier data extract
        /// </summary>
        /// <param name="args">Command line arguments for the application</param>
        public static void Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            var app = serviceProvider.GetService<Application>();

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            ILoggerFactory loggerFactory = new LoggerFactory()
                .AddConsole()
                .AddDebug();

            services.AddSingleton(loggerFactory);
            services.AddLogging();
            services.AddSingleton<IQueryProvider, QueryProvider>();
            services.AddSingleton<IHitchedProvider, HitchedProvider>();
            services.AddSingleton<IApiService, ApiService>();

            IConfigurationRoot configuration = GetConfiguration();
            services.AddSingleton<IConfigurationRoot>(configuration);

            // Support typed Options
            services.AddOptions();
            services.Configure<ApplicationOptions>(configuration.GetSection("ApplicationOptions"));
            services.Configure<ExtractOptions>(configuration.GetSection("ExtractOptions"));
            services.Configure<DBOptions>(configuration.GetSection("DBOptions"));

            services.AddTransient<Application>();
        }

        private static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"AppSettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}