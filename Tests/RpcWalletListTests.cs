using MCWrapper.Data.Models.Wallet;
using MCWrapper.Data.Models.Wallet.CustomModels;
using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Test.ServicesPipeline;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Test.Lists
{
    [TestFixture]
    public class RpcWalletListTests
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
        private readonly IMultiChainRpcGeneral _blockchain;
        private readonly IMultiChainRpcWallet _wallet;
        private readonly string _chainName;
        private readonly string _address;

        // Use mock startup service container
        private readonly ExplicitStartup _services = new ExplicitStartup();

        public RpcWalletListTests()
        {
            _blockchain = _services.GetRequiredService<IMultiChainRpcGeneral>();
            _wallet = _services.GetRequiredService<IMultiChainRpcWallet>();
            _address = _wallet.RpcOptions.ChainAdminAddress;
            _chainName = _wallet.RpcOptions.ChainName;
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need listaccounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListAccountsTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListAccountsAsync(_chainName, UUID.NoHyphens, 2, true);

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListAccountsAsync(2, true);

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(infActual);
        }

        [Test]
        public async Task ListAddressesTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListAddressesAsync(_chainName, UUID.NoHyphens, "*", true, 1, 0);

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.NotZero(expActual.Result.Count);
            Assert.IsInstanceOf<RpcResponse<IList<ListAddressesResult>>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListAddressesAsync("*", true, 1, 0);

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.NotZero(infActual.Result.Count);
            Assert.IsInstanceOf<RpcResponse<IList<ListAddressesResult>>>(infActual);
        }

        [Test]
        public async Task ListAddressGroupingsTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListAddressGroupingsAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<object>>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListAddressGroupingsAsync();

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<object>>>(infActual);
        }

        [Test]
        public async Task ListAddressTransactionsTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListAddressTransactionsAsync(_chainName, UUID.NoHyphens, _address, 10, 0, true);

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListAddressTransactionsResult>>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListAddressTransactionsAsync(_address, 10, 0, true);

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListAddressTransactionsResult>>>(infActual);
        }

        [Test]
        public async Task ListAssetTransactionsTestAsync()
        {
            // Stage
            var issue = await _wallet.IssueAsync(_address, new AssetEntity(), 100, 1, 0, default);
            await _wallet.SubscribeAsync(issue.Result, false, "");

            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListAssetTransactionsAsync(_chainName, UUID.NoHyphens, issue.Result, true, 10, 0, true);

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListAssetTransactionsResult>>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListAssetTransactionsAsync(issue.Result, true, 10, 0, true);

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListAssetTransactionsResult>>>(infActual);
        }

        [Test]
        public async Task ListLockUnspentTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListLockUnspentAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<Transaction>>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListLockUnspentAsync();

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<Transaction>>>(infActual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need listaccounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListReceivedByAccountTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListReceivedByAccountAsync(_chainName, UUID.NoHyphens, 2, true, true);

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<object>>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListReceivedByAccountAsync(2, true, true);

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<object>>>(infActual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need listaccounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListReceivedByAddressTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListReceivedByAddressAsync(_chainName, UUID.NoHyphens, 2, true, true);

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<object>>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListReceivedByAddressAsync(2, true, true);

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<object>>>(infActual);
        }

        [Test]
        public async Task ListSinceBlockTestAsync()
        {
            // Stage
            var lastBlock = await _blockchain.GetLastBlockInfoAsync(0);

            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListSinceBlockAsync(_chainName, UUID.NoHyphens, lastBlock.Result.Hash, 1, true);

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListSinceBlockAsync(lastBlock.Result.Hash, 1, true);

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse>(infActual);
        }

        [Test]
        public async Task ListStreamBlockItemsTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListStreamBlockItemsAsync(_chainName, UUID.NoHyphens, "root", "60, 61-65", true, 10, 0);

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<object>>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListStreamBlockItemsAsync("root", "60, 61-65", true, 10, 0);

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<object>>>(infActual);
        }

        [Test]
        public async Task ListStreamItemsTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListStreamItemsAsync(_chainName, UUID.NoHyphens, "root", true, 10, 0, true);

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListStreamItemsResult>>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListStreamItemsAsync("root", true, 10, 0, true);

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListStreamItemsResult>>>(infActual);
        }

        [Test]
        public async Task ListStreamKeyItemsTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListStreamKeyItemsAsync(_chainName, UUID.NoHyphens, "root", "some_key", true, 10, 0, true);

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListStreamKeyItemsResult>>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListStreamKeyItemsAsync("root", "some_key", true, 10, 0, true);

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListStreamKeyItemsResult>>>(infActual);
        }

        [Test]
        public async Task ListStreamKeysTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListStreamKeysAsync(_chainName, UUID.NoHyphens, "root", "*", true, 10, 0, true);

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListStreamKeysResult>>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListStreamKeysAsync("root", "*", true, 10, 0, true);

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListStreamKeysResult>>>(infActual);
        }

        [Test]
        public async Task ListStreamPublisherItemsTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListStreamPublisherItemsAsync(_chainName, UUID.NoHyphens, "root", _address, true, 10, 0, true);

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListStreamPublisherItemsResult>>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListStreamPublisherItemsAsync("root", _address, true, 10, 0, true);

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListStreamPublisherItemsResult>>>(infActual);
        }

        [Test]
        public async Task ListStreamPublishersTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListStreamPublishersAsync(_chainName, UUID.NoHyphens, "root", "*", true, 10, 0, true);

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListStreamPublishersResult>>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListStreamPublishersAsync("root", "*", true, 10, 0, true);

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListStreamPublishersResult>>>(infActual);
        }

        [Test]
        public async Task ListStreamQueryItemsTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListStreamQueryItemsAsync(_chainName, UUID.NoHyphens, "root", new { publisher = _address }, true);

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<object>>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListStreamQueryItemsAsync("root", new { publisher = _address }, true);

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<object>>>(infActual);
        }

        [Test]
        public async Task ListStreamTxItemsTestAsync()
        {
            // Stage
            var txid = await _wallet.PublishAsync("root", ChainEntity.GetUUID(), "Some Stream Item Data".ToHex(), "");

            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListStreamTxItemsAsync(_chainName, UUID.NoHyphens, "root", txid.Result, true);

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<object>>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListStreamTxItemsAsync("root", txid.Result, true);

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<object>>>(infActual);
        }

        [Test, Ignore("Not supported with scalable wallet - if you need listtransactions, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListTransactionsTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListTransactionsAsync(_chainName, UUID.NoHyphens, "some_account", 10, 0, true);

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListTransactionsResult>>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListTransactionsAsync("some_account", 10, 0, true);

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListTransactionsResult>>>(infActual);
        }

        [Test]
        public async Task ListUnspentTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListUnspentAsync(_chainName, UUID.NoHyphens, 2, 100, new[] { _address });

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListUnspentResult>>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListUnspentAsync(2, 100, new[] { _address });

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListUnspentResult>>>(infActual);
        }

        [Test]
        public async Task ListWalletTransactionsTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.ListWalletTransactionsAsync(_chainName, UUID.NoHyphens, 10, 0, true, true);

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListWalletTransactionsResult>>>(expActual);

            /*
              Inferred blockchain name test
           */

            // Act
            var infActual = await _wallet.ListWalletTransactionsAsync(10, 0, true, true);

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<ListWalletTransactionsResult>>>(infActual);
        }
    }
}