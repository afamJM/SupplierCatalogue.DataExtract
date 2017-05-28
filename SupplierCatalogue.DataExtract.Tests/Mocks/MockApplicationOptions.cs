namespace SupplierCatalogue.DataExtract.Tests.Mocks
{
    using System.Reflection;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestPlatform.TestHost;
    using SupplierCatalogue.DataExtract.Configuration;

    /// <summary>
    /// The mock testing object for application configuration settings
    /// </summary>
    public class MockApplicationOptions : IOptions<ApplicationOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockApplicationOptions"/> class.
        /// </summary>
        public MockApplicationOptions()
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
        /// Gets the value for the application configuration settings
        /// </summary>
        public ApplicationOptions Value
        {
            get
            {
                ApplicationOptions mockOptions = new ApplicationOptions()
                {
                    Name = this.Name,
                    Version = this.Version,
                };
                return mockOptions;
            }
        }

        /// <summary>
        /// Gets or sets the assembly for the application
        /// </summary>
        public Assembly Assembly { get; set; }
    }
}
