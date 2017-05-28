namespace SupplierCatalogue.DataExtract.Tests
{
    using SupplierCatalogue.DataExtract.Interfaces.Providers;
    using SupplierCatalogue.DataExtract.Providers;
    using SupplierCatalogue.DataExtract.Tests.Mocks;
    using Xunit;

    /// <summary>
    /// Tests for the Hitched Query Provider class
    /// </summary>
    public class QueryProviderTest
    {
        private readonly MockApplicationOptions application;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryProviderTest"/> class.
        /// </summary>
        public QueryProviderTest()
        {
            this.application = new MockApplicationOptions();
        }

        /// <summary>
        /// Test that the query provider return valid SQL
        /// </summary>
        [Fact]
        public void ShouldReturnSQL()
        {
            MockExtractOptions testOptions = new MockExtractOptions()
            {
                RecordCount = 10,
            };
            IQueryProvider queryProvider = new QueryProvider(this.application, testOptions);
            string testSQL = queryProvider.FetchSupplierQuery();
            Assert.NotEqual(0, testSQL.Length);
        }

        /// <summary>
        /// Test that the query provider limits the result set when indicated in the settings file
        /// </summary>
        [Fact]
        public void ShouldIncludeRecordLimit()
        {
            MockExtractOptions testOptions = new MockExtractOptions()
            {
                RecordCount = 10,
            };
            IQueryProvider queryProvider = new QueryProvider(this.application, testOptions);
            string testSQL = queryProvider.FetchSupplierQuery();
            Assert.NotEqual(0, testSQL.Length);
            Assert.Contains("Top (@RecordCount)", testSQL);
        }

        /// <summary>
        /// Test that the query provider does not limit the result set when indicated in the settings file
        /// </summary>
        [Fact]
        public void ShouldNotIncludeRecordLimit()
        {
            MockExtractOptions testOptions = new MockExtractOptions()
            {
                RecordCount = 0,
            };
            IQueryProvider queryProvider = new QueryProvider(this.application, testOptions);
            string testSQL = queryProvider.FetchSupplierQuery();
            Assert.NotEqual(0, testSQL.Length);
            Assert.DoesNotContain("Top (@RecordCount)", testSQL);
        }
    }
}
