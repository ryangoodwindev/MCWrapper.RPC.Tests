using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServiceHelpers;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    class OffChainRPCClientExplicitTests
    {
        // private field
        private readonly IMultiChainRpcOffChain _offChain;

        /// <summary>
        /// Create new NetworkServiceTests instance
        /// </summary>
        public OffChainRPCClientExplicitTests()
        {
            // instantiate test services provider
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch service from provider
            _offChain = provider.GetService<IMultiChainRpcOffChain>();
        }

        [Test, Ignore("Ignored until I can test with enterprise edition")]
        public async Task PurgePublishedItemsAsyncTest()
        {
            var purge = await _offChain.PurgePublishedItemsAsync(_offChain.RpcOptions.ChainName, nameof(PurgePublishedItemsAsyncTest), "some_txid(s)");

            Assert.IsNotNull(purge);
        }

        [Test, Ignore("Ignored until I can test with enterprise edition")]
        public async Task PurgeStreamItemsAsyncTest()
        {
            var purge = await _offChain.PurgeStreamItemsAsync(_offChain.RpcOptions.ChainName, nameof(PurgeStreamItemsAsyncTest), "some_stream_identifier", "some_txid(s)");

            Assert.IsNotNull(purge);
        }

        [Test, Ignore("Ignored until I can test with enterprise edition")]
        public async Task RetrieveStreamItemsAsyncTest()
        {
            var retrieve = await _offChain.RetrieveStreamItemsAsync(_offChain.RpcOptions.ChainName, nameof(RetrieveStreamItemsAsyncTest), "some_stream_identifier", "some_txid(s)");

            Assert.IsNotNull(retrieve);
        }
    }
}
