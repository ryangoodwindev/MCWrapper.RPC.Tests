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
    public class IMultiChainRpcUtilityTests
    {
        // private field
        private readonly IMultiChainRpcUtility _utility;
        private readonly IMultiChainRpcWallet _wallet;

        /// <summary>
        /// Create a new UtilityServiceTests instance
        /// </summary>
        public IMultiChainRpcUtilityTests()
        {
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch services from provider
            _utility = provider.GetService<IMultiChainRpcUtility>();
            _wallet = provider.GetService<IMultiChainRpcWallet>();
        }

        [Test]
        public async Task BinaryCacheTestAsync()
        {
            // Act - Create a binary cache
            var cache = await _utility.CreateBinaryCacheAsync(blockchainName: _utility.RpcOptions.ChainName, id: nameof(BinaryCacheTestAsync));

            // Assert
            Assert.IsNull(cache.Error);
            Assert.IsNotNull(cache.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(cache);

            // Act - Append to binary cache
            var append = await _utility.AppendBinaryCacheAsync(
                blockchainName: _utility.RpcOptions.ChainName,
                id: nameof(BinaryCacheTestAsync),
                identifier: cache.Result,
                data_hex: "Some string data we can use to generate dat hex content. An then a bit more at the end".ToHex());

            // Assert
            Assert.IsNull(append.Error);
            Assert.IsNotNull(append.Result);
            Assert.IsInstanceOf<RpcResponse<int>>(append);

            // Act - Delete binary cache
            var delete = await _utility.DeleteBinaryCacheAsync(
                blockchainName: _utility.RpcOptions.ChainName,
                id: nameof(BinaryCacheTestAsync),
                identifier: cache.Result);

            // Assert
            Assert.IsNull(delete.Error);
            Assert.IsNull(delete.Result);
            Assert.IsInstanceOf<RpcResponse>(delete);
        }

        [Test]
        public async Task CreateKeyPairsTestAsync()
        {
            // Act - Ask network for a new key pair
            RpcResponse<CreateKeyPairsResult[]> actual = await _utility.CreateKeyPairsAsync(
                blockchainName: _utility.RpcOptions.ChainName,
                id: nameof(CreateKeyPairsTestAsync),
                count: 3);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<CreateKeyPairsResult[]>>(actual);
        }

        [Test]
        public async Task CreateMultiSigTestAsync()
        {
            // Act - Create an address that requires the node's signature
            RpcResponse<CreateMultiSigResult> actual = await _utility.CreateMultiSigAsync(
                blockchainName: _utility.RpcOptions.ChainName,
                id: nameof(CreateMultiSigTestAsync),
                n_required: 1, 
                keys: new string[] 
                {
                    _utility.RpcOptions.ChainAdminAddress,
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
            var actual = await _utility.EstimateFeeAsync(
                blockchainName: _utility.RpcOptions.ChainName,
                id: nameof(EstimateFeeTestAsync),
                n_blocks: 78);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<long>>(actual);
        }

        [Test]
        public async Task EstimatePriorityTestAsync()
        {
            // Act - Estimate zero-fee transaction 
            var actual = await _utility.EstimatePriorityAsync(
                blockchainName: _utility.RpcOptions.ChainName,
                id: nameof(EstimatePriorityTestAsync),
                n_blocks: 78);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<float>>(actual);
        }

        [Test]
        public async Task ValidateAddressTestAsync()
        {
            // Act - Return information about the given address
            RpcResponse<ValidateAddressResult> actual = await _utility.ValidateAddressAsync(
                blockchainName: _utility.RpcOptions.ChainName,
                id: nameof(ValidateAddressTestAsync),
                address_or_pubkey_or_privkey: _utility.RpcOptions.ChainAdminAddress);

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
            var signature = await _wallet.SignMessageAsync(
                blockchainName: _wallet.RpcOptions.ChainName,
                id: nameof(VerifyMessageTestAsync),
                address_or_privkey: _wallet.RpcOptions.ChainAdminAddress,
                message: defaultMessage);

            // Assert
            Assert.IsNull(signature.Error);
            Assert.IsNotNull(signature.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(signature);

            // Act - Verify signed message
            var actual = await _utility.VerifyMessageAsync(
                blockchainName: _wallet.RpcOptions.ChainName,
                id: nameof(VerifyMessageTestAsync),
                address: _wallet.RpcOptions.ChainAdminAddress,
                signature: signature.Result,
                message: defaultMessage);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(actual);
        }

        // Inferred blockchainName tests //

        [Test]
        public async Task BinaryCacheInferredTestAsync()
        {
            // Act - Create a binary cache
            var cache = await _utility.CreateBinaryCacheAsync();

            // Assert
            Assert.IsNull(cache.Error);
            Assert.IsNotNull(cache.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(cache);

            // Act - Append to binary cache
            var append = await _utility.AppendBinaryCacheAsync(
                identifier: cache.Result,
                data_hex: "Some string data we can use to generate dat hex content. An then a bit more at the end".ToHex());

            // Assert
            Assert.IsNull(append.Error);
            Assert.IsNotNull(append.Result);
            Assert.IsInstanceOf<RpcResponse<int>>(append);

            // Act - Delete binary cache
            var delete = await _utility.DeleteBinaryCacheAsync(identifier: cache.Result);

            // Assert
            Assert.IsNull(delete.Error);
            Assert.IsNull(delete.Result);
            Assert.IsInstanceOf<RpcResponse>(delete);
        }

        [Test]
        public async Task CreateKeyPairsInferredTestAsync()
        {
            // Act - Ask network for a new key pair
            RpcResponse<CreateKeyPairsResult[]> actual = await _utility.CreateKeyPairsAsync(count: 3);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<CreateKeyPairsResult[]>>(actual);
        }

        [Test]
        public async Task CreateMultiSigInferredTestAsync()
        {
            // Act - Create an address that requires the node's signature
            RpcResponse<CreateMultiSigResult> actual = await _utility.CreateMultiSigAsync(
                n_required: 1,
                keys: new string[]
                {
                    _utility.RpcOptions.ChainAdminAddress,
                });

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<CreateMultiSigResult>>(actual);
        }

        [Test]
        public async Task EstimateFeeInferredTestAsync()
        {
            // Act - Estimate fee according to kB and number of blocks
            var actual = await _utility.EstimateFeeAsync(n_blocks: 78);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<long>>(actual);
        }

        [Test]
        public async Task EstimatePriorityInferredTestAsync()
        {
            // Act - Estimate zero-fee transaction 
            var actual = await _utility.EstimatePriorityAsync(n_blocks: 78);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<float>>(actual);
        }

        [Test]
        public async Task ValidateAddressInferredTestAsync()
        {
            // Act - Return information about the given address
            RpcResponse<ValidateAddressResult> actual = await _utility.ValidateAddressAsync(_utility.RpcOptions.ChainAdminAddress);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ValidateAddressResult>>(actual);
        }

        [Test]
        public async Task VerifyMessageInferredTestAsync()
        {
            // Stage - Default test message
            var defaultMessage = "Some Test Message for this distributed blockchain network.";

            // Act - Sign message
            var signature = await _wallet.SignMessageAsync(
                address_privkey: _wallet.RpcOptions.ChainAdminAddress,
                message: defaultMessage);

            // Assert
            Assert.IsNull(signature.Error);
            Assert.IsNotNull(signature.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(signature);

            // Act - Verify signed message
            var actual = await _utility.VerifyMessageAsync(
                address: _utility.RpcOptions.ChainAdminAddress,
                signature: signature.Result,
                message: defaultMessage);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(actual);
        }
    }
}