using MCWrapper.Data.Models.Utility;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServiceHelpers;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class UtilityRPCClientInferredTests
    {
        // private field
        private readonly WalletRpcClient Wallet;
        private readonly UtilityRpcClient Utility;

        /// <summary>
        /// Create a new UtilityServiceTests instance
        /// </summary>
        public UtilityRPCClientInferredTests()
        {
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch services from provider
            Wallet = provider.GetService<WalletRpcClient>();
            Utility = provider.GetService<UtilityRpcClient>();
        }

        [Test]
        public async Task BinaryCacheTestAsync()
        {
            // Act - Create a binary cache
            var cache = await Utility.CreateBinaryCacheAsync();

            // Assert
            Assert.IsNull(cache.Error);
            Assert.IsNotNull(cache.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(cache);

            // Act - Append to binary cache
            var append = await Utility.AppendBinaryCacheAsync(
                identifier: cache.Result,
                data_hex: "Some string data we can use to generate dat hex content. An then a bit more at the end".ToHex());

            // Assert
            Assert.IsNull(append.Error);
            Assert.IsNotNull(append.Result);
            Assert.IsInstanceOf<RpcResponse<int>>(append);

            // Act - Delete binary cache
            var delete = await Utility.DeleteBinaryCacheAsync(identifier: cache.Result);

            // Assert
            Assert.IsNull(delete.Error);
            Assert.IsNull(delete.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(delete);
        }

        [Test]
        public async Task CreateKeyPairsTestAsync()
        {
            // Act - Ask network for a new key pair
            RpcResponse<CreateKeyPairsResult[]> actual = await Utility.CreateKeyPairsAsync(count: 3);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<CreateKeyPairsResult[]>>(actual);
        }

        [Test]
        public async Task CreateMultiSigTestAsync()
        {
            // Act - Create an address that requires the node's signature
            RpcResponse<CreateMultiSigResult> actual = await Utility.CreateMultiSigAsync(
                n_required: 1,
                keys: new string[]
                {
                    Utility.BlockchainOptions.ChainAdminAddress,
                });

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<CreateMultiSigResult>>(actual);
        }

        [Test]
        public async Task EstimateFeeTestAsync()
        {
            // Act - Estimate fee according to kB and number of blocks
            var actual = await Utility.EstimateFeeAsync(n_blocks: 78);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task EstimatePriorityTestAsync()
        {
            // Act - Estimate zero-fee transaction 
            var actual = await Utility.EstimatePriorityAsync(n_blocks: 78);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task ValidateAddressTestAsync()
        {
            // Act - Return information about the given address
            RpcResponse<ValidateAddressResult> actual = await Utility.ValidateAddressAsync(Utility.BlockchainOptions.ChainAdminAddress);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ValidateAddressResult>>(actual);
        }

        [Test]
        public async Task VerifyMessageTestAsync()
        {
            // Stage - Default test message
            var defaultMessage = "Some Test Message for this distributed blockchain network.";

            // Act - Sign message
            var signature = await Wallet.SignMessageAsync(
                address_privkey: Wallet.BlockchainOptions.ChainAdminAddress,
                message: defaultMessage);

            // Assert
            Assert.IsNull(signature.Error);
            Assert.IsNotNull(signature.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(signature);

            // Act - Verify signed message
            var actual = await Utility.VerifyMessageAsync(
                address: Utility.BlockchainOptions.ChainAdminAddress,
                signature: signature.Result,
                message: defaultMessage);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(actual);
        }
    }
}
