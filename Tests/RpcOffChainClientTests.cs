using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServicesPipeline;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    class RpcOffChainClientTests
    {
        // Inject services
        private readonly IMultiChainRpcOffChain _offChain;
        private readonly string ChainName;

        // Create new NetworkServiceTests instance
        public RpcOffChainClientTests()
        {
            // instantiate mock services container
            var services = new ParameterlessMockServices();

            // fetch service from service container
            _offChain = services.GetRequiredService<IMultiChainRpcOffChain>();

            ChainName = _offChain.RpcOptions.ChainName;
        }

        [Test, Ignore("Ignored until I can test with enterprise edition")]
        public async Task PurgePublishedItemsAsyncTest()
        {
            var expPurge = await _offChain.PurgePublishedItemsAsync(ChainName, nameof(PurgePublishedItemsAsyncTest), "some_txid(s)");

            Assert.IsNotNull(expPurge);

            var infPurge = await _offChain.PurgePublishedItemsAsync("some_txid(s)");

            Assert.IsNotNull(infPurge);
        }

        [Test, Ignore("Ignored until I can test with enterprise edition")]
        public async Task PurgeStreamItemsAsyncTest()
        {
            var expPurge = await _offChain.PurgeStreamItemsAsync(ChainName, nameof(PurgeStreamItemsAsyncTest), "some_stream_identifier", "some_txid(s)");

            Assert.IsNotNull(expPurge);

            var infPurge = await _offChain.PurgeStreamItemsAsync("some_stream_identifier", "some_txid(s)");

            Assert.IsNotNull(infPurge);
        }

        [Test, Ignore("Ignored until I can test with enterprise edition")]
        public async Task RetrieveStreamItemsAsyncTest()
        {
            var expRetrieve = await _offChain.RetrieveStreamItemsAsync(ChainName, nameof(RetrieveStreamItemsAsyncTest), "some_stream_identifier", "some_txid(s)");

            Assert.IsNotNull(expRetrieve);

            var infRetrieve = await _offChain.RetrieveStreamItemsAsync("some_stream_identifier", "some_txid(s)");

            Assert.IsNotNull(infRetrieve);
        }
    }
}
