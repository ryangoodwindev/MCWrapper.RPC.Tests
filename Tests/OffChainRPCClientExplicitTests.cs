using MCWrapper.RPC.Ledger.Clients.OffChain;
using MCWrapper.RPC.Tests.ServiceHelpers;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    class OffChainRPCClientExplicitTests
    {
        // private field
        private readonly OffChainRpcClient OffChain;

        /// <summary>
        /// Create new NetworkServiceTests instance
        /// </summary>
        public OffChainRPCClientExplicitTests()
        {
            // instantiate test services provider
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch service from provider
            OffChain = provider.GetService<OffChainRpcClient>();
        }

        [Test, Ignore("Ignored until I can test with enterprise edition")]
        public async Task PurgePublishedItemsAsyncTest()
        {
            var purge = await OffChain.PurgePublishedItemsAsync(OffChain.BlockchainOptions.ChainName, nameof(PurgePublishedItemsAsyncTest), "some_txid(s)");

            Assert.IsNotNull(purge);
        }

        [Test, Ignore("Ignored until I can test with enterprise edition")]
        public async Task PurgeStreamItemsAsyncTest()
        {
            var purge = await OffChain.PurgeStreamItemsAsync(OffChain.BlockchainOptions.ChainName, nameof(PurgeStreamItemsAsyncTest), "some_stream_identifier", "some_txid(s)");

            Assert.IsNotNull(purge);
        }

        [Test, Ignore("Ignored until I can test with enterprise edition")]
        public async Task RetrieveStreamItemsAsyncTest()
        {
            var retrieve = await OffChain.RetrieveStreamItemsAsync(OffChain.BlockchainOptions.ChainName, nameof(RetrieveStreamItemsAsyncTest), "some_stream_identifier", "some_txid(s)");

            Assert.IsNotNull(retrieve);
        }
    }
}
