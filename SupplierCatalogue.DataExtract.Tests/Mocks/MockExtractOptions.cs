namespace SupplierCatalogue.DataExtract.Tests.Mocks
{
    using Microsoft.Extensions.Options;
    using SupplierCatalogue.DataExtract.Configuration;

    /// <summary>
    /// The mock testing object for extract configuration settings
    /// </summary>
    public class MockExtractOptions : IOptions<ExtractOptions>
    {
        /// <summary>
        /// Gets or sets the file output path for the extract
        /// </summary>
        public string SuppliersOutFilePath { get; set; }

        /// <summary>
        /// Gets or sets the file output name for the extract
        /// </summary>
        public string SuppliersOutFileName { get; set; }

        /// <summary>
        /// Gets or sets the record count for the extract
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// Gets the value for the extract configuration settings
        /// </summary>
        public ExtractOptions Value
        {
            get
            {
                ExtractOptions mockOptions = new ExtractOptions()
                {
                    RecordCount = this.RecordCount,
                    SuppliersOutFileName = this.SuppliersOutFileName,
                    SuppliersOutFilePath = this.SuppliersOutFilePath,
                };
                return mockOptions;
            }
        }
    }
}
