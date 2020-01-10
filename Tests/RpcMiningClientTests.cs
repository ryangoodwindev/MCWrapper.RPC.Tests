using MCWrapper.Data.Models.Mining;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServicesPipeline;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class RpcMiningClientTests
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
        private readonly IMultiChainRpcMining _mining;
        private readonly string _chainName;

        // Use mock startup service container
        private readonly ExplicitStartup _services = new ExplicitStartup();

        // Create a new RpcMiningClientTests instance
        public RpcMiningClientTests()
        {
            _mining = _services.GetRequiredService<IMultiChainRpcMining>();
            _chainName = _mining.RpcOptions.ChainName;
        }

        [Test, Ignore("Not supported by MultiChain v2.02")]
        public async Task GetBlockTemplateExplicitTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expGet = await _mining.GetBlockTemplateAsync(_chainName, UUID.NoHyphens, "");

            // Assert
            Assert.IsNull(expGet.Result);
            Assert.IsNotNull(expGet.Error);
            Assert.IsInstanceOf<RpcResponse<object>>(expGet);

            /*
              Inferred blockchain name test
           */

            // Act
            var infGet = await _mining.GetBlockTemplateAsync("");

            // Assert
            Assert.IsNull(infGet.Result);
            Assert.IsNotNull(infGet.Error);
            Assert.IsInstanceOf<RpcResponse<object>>(infGet);
        }

        [Test]
        public async Task GetMiningInfoExplicitTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Ask network for blockchain mining information
            var expGet = await _mining.GetMiningInfoAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expGet.Error);
            Assert.IsNotNull(expGet.Result);
            Assert.IsInstanceOf<RpcResponse<GetMiningInfoResult>>(expGet);

            /*
              Inferred blockchain name test
           */

            // Act - Ask network for blockchain mining information
            var infGet = await _mining.GetMiningInfoAsync();

            // Assert
            Assert.IsNull(infGet.Error);
            Assert.IsNotNull(infGet.Result);
            Assert.IsInstanceOf<RpcResponse<GetMiningInfoResult>>(infGet);
        }

        [Test]
        public async Task GetNetworkHashPsExplicitTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Hash per sec statistic
            var expGet = await _mining.GetNetworkHashPsAsync(_chainName, UUID.NoHyphens, 10, 10);

            // Assert
            Assert.IsNull(expGet.Error);
            Assert.IsNotNull(expGet.Result);
            Assert.IsInstanceOf<RpcResponse<int>>(expGet);

            /*
              Inferred blockchain name test
           */

            // Act - Hash per sec statistic
            var infGet = await _mining.GetNetworkHashPsAsync(10, 10);

            // Assert
            Assert.IsNull(infGet.Error);
            Assert.IsNotNull(infGet.Result);
            Assert.IsInstanceOf<RpcResponse<int>>(infGet);
        }

        [Test, Ignore("Not supported by MultiChain v2.02")]
        public async Task PrioritiseTransactionExplicitTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Prioritize a transaction on the network
            var expPrioritise = await _mining.PrioritiseTransactionAsync(_chainName, UUID.NoHyphens, "some_txid_when_this_feature_is_supported", 0.0, 1000);

            // Assert
            Assert.IsNull(expPrioritise.Result);
            Assert.IsNotNull(expPrioritise.Error);
            Assert.IsInstanceOf<RpcResponse<object>>(expPrioritise);

            /*
              Inferred blockchain name test
           */

            // Act - Prioritize a transaction on the network
            var infPrioritise = await _mining.PrioritiseTransactionAsync("some_txid_when_this_feature_is_supported", 0.0, 1000);

            // Assert
            Assert.IsNull(infPrioritise.Result);
            Assert.IsNotNull(infPrioritise.Error);
            Assert.IsInstanceOf<RpcResponse<object>>(infPrioritise);
        }

        [Test, Ignore("SubmitBlock is ignored because I don't understand how to use it yet")]
        public async Task SubmitBlockExplicitTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Submit a block to the blockchain
            var expSubmit = await _mining.SubmitBlockAsync(_chainName, UUID.NoHyphens, string.Empty);

            // Assert
            Assert.IsNull(expSubmit.Result);
            Assert.IsNotNull(expSubmit.Error);
            Assert.IsInstanceOf<RpcResponse<object>>(expSubmit);

            /*
              Inferred blockchain name test
           */

            // Act - Submit a block to the blockchain
            var infSubmit = await _mining.SubmitBlockAsync(string.Empty, "");

            // Assert
            Assert.IsNull(infSubmit.Result);
            Assert.IsNotNull(infSubmit.Error);
            Assert.IsInstanceOf<RpcResponse<object>>(infSubmit);
        }
    }
}
