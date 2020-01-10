using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServicesPipeline;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    class RpcOffChainClientTests
    {
        /*
         
            Please note: 

            There are two types of methods demonstrated below for each test.

            Explicit method => Requires that the target blockchain's name must be passed as an argument to the
                               associated method.

            Inferred method => The target blockchain's name is not required to be passed as an agrument directly,
                               however, these methods do require that the RpcOptions have been configured properly
                               during application startup.
            
            All variables beginning with the 'exp' prefix are the result of an explicit method.
            All variables beginning with the 'inf' prefix are the result of an inferred method.
             
        */

        // Inject services
        private readonly IMultiChainRpcOffChain _offChain;
        private readonly string _chainName;

        // Use mock startup service container
        private readonly ExplicitStartup _services = new ExplicitStartup();

        // Create new RpcOffChainClientTests instance
        public RpcOffChainClientTests()
        {
            _offChain = _services.GetRequiredService<IMultiChainRpcOffChain>();
            _chainName = _offChain.RpcOptions.ChainName;
        }

        [Test, Ignore("Ignored until I can test with enterprise edition")]
        public async Task PurgePublishedItemsAsyncTest()
        {
            /*
              Explicit blockchain name test
           */

            var expPurge = await _offChain.PurgePublishedItemsAsync(_chainName, nameof(PurgePublishedItemsAsyncTest), "some_txid(s)");

            Assert.IsNotNull(expPurge);

            /*
               Inferred blockchain name test
            */

            var infPurge = await _offChain.PurgePublishedItemsAsync("some_txid(s)");

            Assert.IsNotNull(infPurge);
        }

        [Test, Ignore("Ignored until I can test with enterprise edition")]
        public async Task PurgeStreamItemsAsyncTest()
        {
            /*
              Explicit blockchain name test
           */

            var expPurge = await _offChain.PurgeStreamItemsAsync(_chainName, nameof(PurgeStreamItemsAsyncTest), "some_stream_identifier", "some_txid(s)");

            Assert.IsNotNull(expPurge);

            /*
               Inferred blockchain name test
            */

            var infPurge = await _offChain.PurgeStreamItemsAsync("some_stream_identifier", "some_txid(s)");

            Assert.IsNotNull(infPurge);
        }

        [Test, Ignore("Ignored until I can test with enterprise edition")]
        public async Task RetrieveStreamItemsAsyncTest()
        {
            /*
              Explicit blockchain name test
           */

            var expRetrieve = await _offChain.RetrieveStreamItemsAsync(_chainName, nameof(RetrieveStreamItemsAsyncTest), "some_stream_identifier", "some_txid(s)");

            Assert.IsNotNull(expRetrieve);

            /*
               Inferred blockchain name test
            */

            var infRetrieve = await _offChain.RetrieveStreamItemsAsync("some_stream_identifier", "some_txid(s)");

            Assert.IsNotNull(infRetrieve);
        }
    }
}
