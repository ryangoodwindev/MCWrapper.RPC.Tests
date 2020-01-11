using MCWrapper.Data.Models.Wallet;
using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServicesPipeline;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class RpcWalletGetTests
    {
        // Inject services
        private readonly IMultiChainRpcWallet _wallet;
        private readonly string _chainName;
        private readonly string _address;

        // Use mock startup service container
        private readonly ExplicitStartup _services = new ExplicitStartup();

        public RpcWalletGetTests()
        {
            _wallet = _services.GetRequiredService<IMultiChainRpcWallet>();
            _address = _wallet.RpcOptions.ChainAdminAddress;
            _chainName = _wallet.RpcOptions.ChainName;
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetAccountAddressTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetAccountAddressAsync("some_account_name");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetAccountAsync(_address);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetAddressBalancesTestAsync()
        {
            // Act
            RpcResponse<GetAddressBalancesResult[]> actual = await _wallet.GetAddressBalancesAsync(_address, 1, false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAddressBalancesResult[]>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetAddressesByAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetAddressesByAccountAsync("some_account_name");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetAddressesTestAsync()
        {
            // Act
            RpcResponse<GetAddressesResult[]> actual = await _wallet.GetAddressesAsync(true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAddressesResult[]>>(actual);
        }

        [Test]
        public async Task GetAddressTransactionTestAsync()
        {
            // Stage
            var transaction = await _wallet.IssueAsync(_address, new AssetEntity(), 100, 1, 0, default);

            // Act
            RpcResponse<GetAddressTransactionResult> actual = await _wallet.GetAddressTransactionAsync(_address, transaction.Result, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAddressTransactionResult>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetAssetBalancesTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetAssetBalancesAsync("some_account_name", 2, true, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetAssetTransactionTestAsync()
        {
            // Stage
            var asset = await _wallet.IssueAsync(_address, new AssetEntity(), 100, 1, 0, default);

            // Stage
            await _wallet.SubscribeAsync(asset.Result, false, "");

            // Act
            RpcResponse<GetAssetTransactionResult> actual = await _wallet.GetAssetTransactionAsync(asset.Result, asset.Result, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAssetTransactionResult>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetBalanceTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetBalanceAsync("", 1, false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetMultiBalancesTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetMultiBalancesAsync(_address, null, 1, true, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetNewAddressTestAsync()
        {
            // Act
            RpcResponse<string> actual = await _wallet.GetNewAddressAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GetRawChangeAddressTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetRawChangeAddressAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetReceivedByAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetReceivedByAccountAsync("some_account_name", 2);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetReceivedByAddressTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetReceivedByAddressAsync(_address, 2);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetStreamItemTestAsync()
        {
            // Stage
            var publish = await _wallet.PublishFromAsync(_address, "root", ChainEntity.GetUUID(), "Stream item data".ToHex(), "");

            // Act
            RpcResponse<GetStreamItemResult> actual = await _wallet.GetStreamItemAsync("root", publish.Result, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetStreamItemResult>>(actual);
        }

        [Test]
        public async Task GetStreamKeySummaryTestAsync()
        {
            // Stage
            var streamKey = ChainEntity.GetUUID();

            // Stage
            await _wallet.PublishFromAsync(_address, "root", streamKey, "Stream item data".ToHex(), "");

            // Act
            RpcResponse<object> actual = await _wallet.GetStreamKeySummaryAsync("root", streamKey, "jsonobjectmerge,ignore,recursive");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetStreamPublisherSummaryTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetStreamPublisherSummaryAsync("root", _address, "jsonobjectmerge,ignore,recursive");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetTotalBalancesTestAsync()
        {
            // Act
            RpcResponse<GetTotalBalancesResult[]> actual = await _wallet.GetTotalBalancesAsync(1, true, false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetTotalBalancesResult[]>>(actual);
        }

        [Test]
        public async Task GetTransactionTestAsync()
        {
            // Stage
            var txid = await _wallet.IssueFromAsync(_address, _address, new AssetEntity(), 100, 1, 0,
                new Dictionary<string, string> { { "description", "Some Description" } });

            // Act
            RpcResponse<GetTransactionResult> actual = await _wallet.GetTransactionAsync(txid.Result, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetTransactionResult>>(actual);
        }

        [Test]
        public async Task GetTxOutDataTestAsync()
        {
            // Stage
            var publish = await _wallet.PublishFromAsync(_address, "root", ChainEntity.GetUUID(), "Stream item data".ToHex(), "");

            // Stage
            var transaction = await _wallet.GetTransactionAsync(publish.Result, true);

            // Act
            RpcResponse<object> actual = await _wallet.GetTxOutDataAsync(transaction.Result.Txid, 0, 10, 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetUnconfirmedBalanceTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetUnconfirmedBalanceAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetWalletInfoTestAsync()
        {
            // Act
            RpcResponse<GetWalletInfoResult> actual = await _wallet.GetWalletInfoAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetWalletInfoResult>>(actual);
        }

        [Test]
        public async Task GetWalletTransactionTestAsync()
        {
            // Stage
            var publish = await _wallet.PublishFromAsync(_address, "root", ChainEntity.GetUUID(), "Stream item data".ToHex(), "");

            // Act
            RpcResponse<GetWalletTransactionResult> actual = await _wallet.GetWalletTransactionAsync(publish.Result, true, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetWalletTransactionResult>>(actual);
        }
    }
}
