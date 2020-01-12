using MCWrapper.Data.Models.Wallet;
using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Test.ServicesPipeline;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Test.Gets
{
    [TestFixture]
    public class RpcWalletGetTests
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
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetAccountAddressAsync(_chainName, UUID.NoHyphens, "some_account_name");

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetAccountAddressAsync("some_account_name");

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infActual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetAccountTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetAccountAsync(_chainName, UUID.NoHyphens, _address);

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetAccountAsync(_address);

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infActual);
        }

        [Test]
        public async Task GetAddressBalancesTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetAddressBalancesAsync(_chainName, UUID.NoHyphens, _address, 1, false);

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAddressBalancesResult[]>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetAddressBalancesAsync(_address, 1, false);

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAddressBalancesResult[]>>(infActual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetAddressesByAccountTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetAddressesByAccountAsync(_chainName, UUID.NoHyphens, "some_account_name");

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetAddressesByAccountAsync("some_account_name");

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infActual);
        }

        [Test]
        public async Task GetAddressesTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetAddressesAsync(_chainName, UUID.NoHyphens, true);

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAddressesResult[]>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetAddressesAsync(true);

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAddressesResult[]>>(infActual);
        }

        [Test]
        public async Task GetAddressTransactionTestAsync()
        {
            // Stage
            var transaction = await _wallet.IssueAsync(_address, new AssetEntity(), 100, 1, 0, default);

            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetAddressTransactionAsync(_chainName, UUID.NoHyphens, _address, transaction.Result, true);

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAddressTransactionResult>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetAddressTransactionAsync(_address, transaction.Result, true);

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAddressTransactionResult>>(infActual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetAssetBalancesTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetAssetBalancesAsync(_chainName, UUID.NoHyphens, "some_account_name", 2, true, true);

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetAssetBalancesAsync("some_account_name", 2, true, true);

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infActual);
        }

        [Test]
        public async Task GetAssetTransactionTestAsync()
        {
            // Stage
            var asset = await _wallet.IssueAsync(_address, new AssetEntity(), 100, 1, 0, default);
            await _wallet.SubscribeAsync(asset.Result, false, "");

            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetAssetTransactionAsync(_chainName, UUID.NoHyphens, asset.Result, asset.Result, true);

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAssetTransactionResult>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetAssetTransactionAsync(asset.Result, asset.Result, true);

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAssetTransactionResult>>(infActual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetBalanceTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetBalanceAsync(_chainName, UUID.NoHyphens, "", 1, false);

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetBalanceAsync("", 1, false);

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infActual);
        }

        [Test]
        public async Task GetMultiBalancesTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetMultiBalancesAsync(_chainName, UUID.NoHyphens, _address, null, 1, true, true);

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetMultiBalancesAsync(_address, null, 1, true, true);

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infActual);
        }

        [Test]
        public async Task GetNewAddressTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetNewAddressAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetNewAddressAsync();

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infActual);
        }

        [Test]
        public async Task GetRawChangeAddressTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetRawChangeAddressAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetRawChangeAddressAsync();

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infActual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetReceivedByAccountTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetReceivedByAccountAsync(_chainName, UUID.NoHyphens, "some_account_name", 2);

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetReceivedByAccountAsync("some_account_name", 2);

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infActual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetReceivedByAddressTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetReceivedByAddressAsync(_chainName, UUID.NoHyphens, _address, 2);

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetReceivedByAddressAsync(_address, 2);

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infActual);
        }

        [Test]
        public async Task GetStreamItemTestAsync()
        {
            // Stage
            var publish = await _wallet.PublishFromAsync(_address, "root", ChainEntity.GetUUID(), "Stream item data".ToHex(), "");

            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetStreamItemAsync(_chainName, UUID.NoHyphens, "root", publish.Result, true);

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<GetStreamItemResult>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetStreamItemAsync("root", publish.Result, true);

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<GetStreamItemResult>>(infActual);
        }

        [Test]
        public async Task GetStreamKeySummaryTestAsync()
        {
            // Stage
            var streamKey = ChainEntity.GetUUID();
            await _wallet.PublishFromAsync(_address, "root", streamKey, "Stream item data".ToHex(), "");

            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetStreamKeySummaryAsync(_chainName, UUID.NoHyphens, "root", streamKey, "jsonobjectmerge,ignore,recursive");

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetStreamKeySummaryAsync("root", streamKey, "jsonobjectmerge,ignore,recursive");

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infActual);
        }

        [Test]
        public async Task GetStreamPublisherSummaryTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetStreamPublisherSummaryAsync(_chainName, UUID.NoHyphens, "root", _address, "jsonobjectmerge,ignore,recursive");

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetStreamPublisherSummaryAsync("root", _address, "jsonobjectmerge,ignore,recursive");

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infActual);
        }

        [Test]
        public async Task GetTotalBalancesTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetTotalBalancesAsync(_chainName, UUID.NoHyphens, 1, true, false);

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<GetTotalBalancesResult[]>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetTotalBalancesAsync(1, true, false);

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<GetTotalBalancesResult[]>>(infActual);
        }

        [Test]
        public async Task GetTransactionTestAsync()
        {
            // Stage
            var txid = await _wallet.IssueFromAsync(_address, _address, new AssetEntity(), 100, 1, 0,
                new Dictionary<string, string> { { "description", "Some Description" } });

            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetTransactionAsync(_chainName, UUID.NoHyphens, txid.Result, true);

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<GetTransactionResult>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetTransactionAsync(txid.Result, true);

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<GetTransactionResult>>(infActual);
        }

        [Test]
        public async Task GetTxOutDataTestAsync()
        {
            // Stage
            var publish = await _wallet.PublishFromAsync(_address, "root", ChainEntity.GetUUID(), "Stream item data".ToHex(), "");
            var transaction = await _wallet.GetTransactionAsync(publish.Result, true);

            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetTxOutDataAsync(_chainName, UUID.NoHyphens, transaction.Result.Txid, 0, 10, 0);

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetTxOutDataAsync(transaction.Result.Txid, 0, 10, 0);

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infActual);
        }

        [Test]
        public async Task GetUnconfirmedBalanceTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetUnconfirmedBalanceAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetUnconfirmedBalanceAsync();

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infActual);
        }

        [Test]
        public async Task GetWalletInfoTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetWalletInfoAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<GetWalletInfoResult>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetWalletInfoAsync();

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<GetWalletInfoResult>>(infActual);
        }

        [Test]
        public async Task GetWalletTransactionTestAsync()
        {
            // Stage
            var publish = await _wallet.PublishFromAsync(_address, "root", ChainEntity.GetUUID(), "Stream item data".ToHex(), "");

            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.GetWalletTransactionAsync(_chainName, UUID.NoHyphens, publish.Result, true, true);

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<GetWalletTransactionResult>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.GetWalletTransactionAsync(publish.Result, true, true);

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
            Assert.IsInstanceOf<RpcResponse<GetWalletTransactionResult>>(infActual);
        }
    }
}
