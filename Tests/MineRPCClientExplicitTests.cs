using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServiceHelpers;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class MineRPCClientExplicitTests
    {
        // private field
        private readonly MiningRpcClient Mining;

        /// <summary>
        /// Create a new MiningServiceTests instance
        /// </summary>
        public MineRPCClientExplicitTests()
        {
            // instantiate test serviecs provider
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch service from provider
            Mining = provider.GetService<MiningRpcClient>();
        }

        [Test, Ignore("Not supported by MultiChain v2.02")]
        public async Task GetBlockTemplateTestAsync()
        {
            // Act - Fetch a blockchain block template
            var actual = await Mining.GetBlockTemplateAsync(blockchainName: Mining.BlockchainOptions.ChainName, id: nameof(GetBlockTemplateTestAsync), "");

            // Assert
            Assert.IsNull(actual.Result);
            Assert.IsNotNull(actual.Error);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetMiningInfoTestAsync()
        {
            // Act - Ask network for blockchain mining information
            var actual = await Mining.GetMiningInfoAsync(blockchainName: Mining.BlockchainOptions.ChainName, id: nameof(GetMiningInfoTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetNetworkHashPsTestAsync()
        {
            // Act - Hash per sec statistic
            var actual = await Mining.GetNetworkHashPsAsync(
                blockchainName: Mining.BlockchainOptions.ChainName,
                id: nameof(GetNetworkHashPsTestAsync),
                blocks: 10,
                height: 10);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Not supported by MultiChain v2.02")]
        public async Task PrioritiseTransactionTestAsync()
        {
            // Act - Prioritize a transaction on the network
            var actual = await Mining.PrioritiseTransactionAsync(
                blockchainName: Mining.BlockchainOptions.ChainName,
                id: nameof(PrioritiseTransactionTestAsync),
                txid: "some_txid_when_this_feature_is_supported",
                priority_delta: 0.0,
                fee_delta: 1000);

            // Assert
            Assert.IsNull(actual.Result);
            Assert.IsNotNull(actual.Error);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("SubmitBlock is ignored because I don't understand how to use it yet")]
        public async Task SubmitBlockTestAsync()
        {
            // Act - Submit a block to the blockchain
            var actual = await Mining.SubmitBlockAsync(
                blockchainName: Mining.BlockchainOptions.ChainName,
                id: nameof(SubmitBlockTestAsync), 
                hex_data: string.Empty);

            // Assert
            Assert.IsNull(actual.Result);
            Assert.IsNotNull(actual.Error);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }
    }
}
