using MCWrapper.Data.Models.Wallet;
using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Test.ServicesPipeline;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Test.Lists
{
    [TestFixture]
    public class RpcWalletListTests
    {
        // Inject services
        private readonly IMultiChainRpcGeneral _blockchain;
        private readonly IMultiChainRpcUtility _utility;
        private readonly IMultiChainRpcWallet _wallet;
        private readonly string _chainName;
        private readonly string _address;

        // Use mock startup service container
        private readonly ExplicitStartup _services = new ExplicitStartup();

        public RpcWalletListTests()
        {
            _blockchain = _services.GetRequiredService<IMultiChainRpcGeneral>();
            _utility = _services.GetRequiredService<IMultiChainRpcUtility>();
            _wallet = _services.GetRequiredService<IMultiChainRpcWallet>();
            _address = _wallet.RpcOptions.ChainAdminAddress;
            _chainName = _wallet.RpcOptions.ChainName;
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need listaccounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListAccountsTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ListAccountsAsync(2, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task ListAddressesTestAsync()
        {
            // Act
            RpcResponse<ListAddressesResult[]> actual = await _wallet.ListAddressesAsync("*", true, 1, 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListAddressesResult[]>>(actual);
        }

        [Test]
        public async Task ListAddressGroupingsTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ListAddressGroupingsAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task ListAddressTransactionsTestAsync()
        {
            // Act
            RpcResponse<ListAddressTransactionsResult[]> actual = await _wallet.ListAddressTransactionsAsync(_address, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListAddressTransactionsResult[]>>(actual);
        }

        [Test]
        public async Task ListAssetTransactionsTestAsync()
        {
            // Stage
            var issue = await _wallet.IssueAsync(_address, new AssetEntity(), 100, 1, 0, default);

            // Stage
            await _wallet.SubscribeAsync(issue.Result, false, "");

            // Act
            RpcResponse<ListAssetTransactionsResult[]> actual = await _wallet.ListAssetTransactionsAsync(issue.Result, true, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListAssetTransactionsResult[]>>(actual);
        }

        [Test]
        public async Task ListLockUnspentTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ListLockUnspentAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need listaccounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListReceivedByAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ListReceivedByAccountAsync(2, true, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need listaccounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListReceivedByAddressTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ListReceivedByAddressAsync(2, true, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task ListSinceBlockTestAsync()
        {
            // Stage
            var lastBlock = await _blockchain.GetLastBlockInfoAsync(0);

            // Act
            RpcResponse<object> actual = await _wallet.ListSinceBlockAsync(lastBlock.Result.Hash, 1, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task ListStreamBlockItemsTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ListStreamBlockItemsAsync("root", "60, 61-65", true, 10, 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task ListStreamItemsTestAsync()
        {
            // Act
            RpcResponse<ListStreamItemsResult[]> actual = await _wallet.ListStreamItemsAsync("root", true, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamItemsResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamKeyItemsTestAsync()
        {
            // Act
            RpcResponse<ListStreamKeyItemsResult[]> actual = await _wallet.ListStreamKeyItemsAsync("root", "some_key", true, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamKeyItemsResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamKeysTestAsync()
        {
            // Act
            RpcResponse<ListStreamKeysResult[]> actual = await _wallet.ListStreamKeysAsync("root", "*", true, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamKeysResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamPublisherItemsTestAsync()
        {
            // Act
            RpcResponse<ListStreamPublisherItemsResult[]> actual = await _wallet.ListStreamPublisherItemsAsync("root", _address, true, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamPublisherItemsResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamPublishersTestAsync()
        {
            // Act
            RpcResponse<ListStreamPublishersResult[]> actual = await _wallet.ListStreamPublishersAsync("root", "*", true, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamPublishersResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamQueryItemsTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ListStreamQueryItemsAsync("root", new { publisher = _address }, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task ListStreamTxItemsTestAsync()
        {
            // Stage
            var txid = await _wallet.PublishAsync("root", ChainEntity.GetUUID(), "Some Stream Item Data".ToHex(), "");

            // Act
            RpcResponse<object> actual = await _wallet.ListStreamTxItemsAsync("root", txid.Result, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Not supported with scalable wallet - if you need listtransactions, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListTransactionsTestAsync()
        {
            // Act
            RpcResponse<ListTransactionsResult[]> actual = await _wallet.ListTransactionsAsync("some_account", 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListTransactionsResult[]>>(actual);
        }

        [Test]
        public async Task ListUnspentTestAsync()
        {
            // Act
            RpcResponse<ListUnspentResult[]> actual = await _wallet.ListUnspentAsync(2, 100, new[] { _address });

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListUnspentResult[]>>(actual);
        }

        [Test]
        public async Task ListWalletTransactionsTestAsync()
        {
            // Act
            RpcResponse<ListWalletTransactionsResult[]> actual = await _wallet.ListWalletTransactionsAsync(10, 0, true, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListWalletTransactionsResult[]>>(actual);
        }
    }
}
