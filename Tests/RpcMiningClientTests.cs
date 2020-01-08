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
        // Inject services
        private readonly IMultiChainRpcMining _mining;
        private readonly string ChainName;

        // Create a new RpcMiningClientTests instance
        public RpcMiningClientTests()
        {
            // instantiate mock services container
            var services = new ParameterlessMockServices();

            // fetch service from service container
            _mining = services.GetRequiredService<IMultiChainRpcMining>();

            ChainName = _mining.RpcOptions.ChainName;
        }

        [Test, Ignore("Not supported by MultiChain v2.02")]
        public async Task GetBlockTemplateExplicitTestAsync()
        {
            // Act
            var expGet = await _mining.GetBlockTemplateAsync(ChainName, UUID.NoHyphens, "");

            // Assert
            Assert.IsNull(expGet.Result);
            Assert.IsNotNull(expGet.Error);
            Assert.IsInstanceOf<RpcResponse<object>>(expGet);

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
            // Act - Ask network for blockchain mining information
            var expGet = await _mining.GetMiningInfoAsync(ChainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expGet.Error);
            Assert.IsNotNull(expGet.Result);
            Assert.IsInstanceOf<RpcResponse<GetMiningInfoResult>>(expGet);

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
            // Act - Hash per sec statistic
            var expGet = await _mining.GetNetworkHashPsAsync(ChainName, UUID.NoHyphens, 10, 10);

            // Assert
            Assert.IsNull(expGet.Error);
            Assert.IsNotNull(expGet.Result);
            Assert.IsInstanceOf<RpcResponse<int>>(expGet);

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
            // Act - Prioritize a transaction on the network
            var expPrioritise = await _mining.PrioritiseTransactionAsync(ChainName, UUID.NoHyphens, "some_txid_when_this_feature_is_supported", 0.0, 1000);

            // Assert
            Assert.IsNull(expPrioritise.Result);
            Assert.IsNotNull(expPrioritise.Error);
            Assert.IsInstanceOf<RpcResponse<object>>(expPrioritise);

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
            // Act - Submit a block to the blockchain
            var expSubmit = await _mining.SubmitBlockAsync(ChainName, UUID.NoHyphens, string.Empty);

            // Assert
            Assert.IsNull(expSubmit.Result);
            Assert.IsNotNull(expSubmit.Error);
            Assert.IsInstanceOf<RpcResponse<object>>(expSubmit);

            // Act - Submit a block to the blockchain
            var infSubmit = await _mining.SubmitBlockAsync(string.Empty, "");

            // Assert
            Assert.IsNull(infSubmit.Result);
            Assert.IsNotNull(infSubmit.Error);
            Assert.IsInstanceOf<RpcResponse<object>>(infSubmit);
        }
    }
}
