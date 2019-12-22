using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServiceHelpers;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class IMultiChainRpcMiningTests
    {
        // private field
        private readonly IMultiChainRpcMining _mining;

        /// <summary>
        /// Create a new MiningServiceTests instance
        /// </summary>
        public IMultiChainRpcMiningTests()
        {
            // instantiate test serviecs provider
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch service from provider
            _mining = provider.GetService<IMultiChainRpcMining>();
        }

        [Test, Ignore("Not supported by MultiChain v2.02")]
        public async Task GetBlockTemplateExplicitTestAsync()
        {
            // Act - Fetch a blockchain block template
            var actual = await _mining.GetBlockTemplateAsync(blockchainName: _mining.RpcOptions.ChainName, id: UUID.NoHyphens, "");

            // Assert
            Assert.IsNull(actual.Result);
            Assert.IsNotNull(actual.Error);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetMiningInfoExplicitTestAsync()
        {
            // Act - Ask network for blockchain mining information
            var actual = await _mining.GetMiningInfoAsync(blockchainName: _mining.RpcOptions.ChainName, id: UUID.NoHyphens);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetNetworkHashPsExplicitTestAsync()
        {
            // Act - Hash per sec statistic
            var actual = await _mining.GetNetworkHashPsAsync(
                blockchainName: _mining.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                blocks: 10,
                height: 10);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Not supported by MultiChain v2.02")]
        public async Task PrioritiseTransactionExplicitTestAsync()
        {
            // Act - Prioritize a transaction on the network
            var actual = await _mining.PrioritiseTransactionAsync(
                blockchainName: _mining.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                txid: "some_txid_when_this_feature_is_supported",
                priority_delta: 0.0,
                fee_delta: 1000);

            // Assert
            Assert.IsNull(actual.Result);
            Assert.IsNotNull(actual.Error);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("SubmitBlock is ignored because I don't understand how to use it yet")]
        public async Task SubmitBlockExplicitTestAsync()
        {
            // Act - Submit a block to the blockchain
            var actual = await _mining.SubmitBlockAsync(
                blockchainName: _mining.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                hex_data: string.Empty);

            // Assert
            Assert.IsNull(actual.Result);
            Assert.IsNotNull(actual.Error);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        // Inferred blockchainName tests //

        [Test, Ignore("Not supported by MultiChain v2.02")]
        public async Task GetBlockTemplateInferredTestAsync()
        {
            // Act - Fetch a blockchain block template
            var actual = await _mining.GetBlockTemplateAsync("");

            // Assert
            Assert.IsNull(actual.Result);
            Assert.IsNotNull(actual.Error);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetMiningInfoInferredTestAsync()
        {
            // Act - Ask network for blockchain mining information
            var actual = await _mining.GetMiningInfoAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetNetworkHashPsInferredTestAsync()
        {
            // Act - Hash per sec statistic
            var actual = await _mining.GetNetworkHashPsAsync(blocks: 10, height: 10);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Not supported by MultiChain v2.02")]
        public async Task PrioritiseTransactionInferredTestAsync()
        {
            // Act - Prioritize a transaction on the network
            var actual = await _mining.PrioritiseTransactionAsync(
                txid: "some_txid_when_this_feature_is_supported",
                priority_delta: 0.0,
                fee_delta: 1000);

            // Assert
            Assert.IsNull(actual.Result);
            Assert.IsNotNull(actual.Error);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("SubmitBlock is ignored because I don't understand how to use it yet")]
        public async Task SubmitBlockInferredTestAsync()
        {
            // Act - Submit a block to the blockchain
            var actual = await _mining.SubmitBlockAsync(hex_data: string.Empty, json_parameters_object: "");

            // Assert
            Assert.IsNull(actual.Result);
            Assert.IsNotNull(actual.Error);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }
    }
}
