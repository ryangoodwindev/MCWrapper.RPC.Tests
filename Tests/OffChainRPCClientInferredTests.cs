using MCWrapper.RPC.Ledger.Clients.OffChain;
using MCWrapper.RPC.Tests.ServiceHelpers;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class OffChainRPCClientInferredTests
    {
        // private field
        private readonly OffChainRpcClient OffChain;

        /// <summary>
        /// Create new NetworkServiceTests instance
        /// </summary>
        public OffChainRPCClientInferredTests()
        {
            // instantiate test services provider
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch service from provider
            OffChain = provider.GetService<OffChainRpcClient>();
        }

        [Test, Ignore("Ignored until I can test with enterprise edition")]
        public async Task PurgePublishedItemsAsyncTest()
        {
            var purge = await OffChain.PurgePublishedItemsAsync("some_txid(s)");

            Assert.IsNotNull(purge);
        }

        [Test, Ignore("Ignored until I can test with enterprise edition")]
        public async Task PurgeStreamItemsAsyncTest()
        {
            var purge = await OffChain.PurgeStreamItemsAsync("some_stream_identifier", "some_txid(s)");

            Assert.IsNotNull(purge);
        }

        [Test, Ignore("Ignored until I can test with enterprise edition")]
        public async Task RetrieveStreamItemsAsyncTest()
        {
            var retrieve = await OffChain.RetrieveStreamItemsAsync("some_stream_identifier", "some_txid(s)");

            Assert.IsNotNull(retrieve);
        }
    }
}
