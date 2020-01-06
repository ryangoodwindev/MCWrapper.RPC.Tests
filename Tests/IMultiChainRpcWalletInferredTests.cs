using MCWrapper.Data.Models.Wallet;
using MCWrapper.Data.Models.Wallet.CustomModels;
using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServiceHelpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class IMultiChainRpcWalletInferredTests
    {
        // private fields
        private readonly IMultiChainRpcUtility _utility;
        private readonly IMultiChainRpcWallet _wallet;
        private readonly IMultiChainRpcGeneral _blockchain;

        /// <summary>
        /// Create a new WalletServiceTests instance
        /// </summary>
        public IMultiChainRpcWalletInferredTests()
        {
            // instantiate test services provider
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch service from provider
            _utility = provider.GetService<IMultiChainRpcUtility>();
            _wallet = provider.GetService<IMultiChainRpcWallet>();
            _blockchain = provider.GetService<IMultiChainRpcGeneral>();
        }

        [Test]
        public async Task AddMultiSigAddressTestAsync()
        {
            // Act
            var actual = await _wallet.AddMultiSigAddressAsync(
                n_required: 1,
                keys: new[] { _wallet.RpcOptions.ChainAdminAddress }, "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task AppendRawExchangeTestAsync()
        {
            // Stage - Issue a new asset to the blockchain node 
            var asset = await _wallet.IssueAsync(
                toAddress: _wallet.RpcOptions.ChainAdminAddress,
                assetParams: new AssetEntity(),
                quantity: 100,
                smallestUnit: 1, nativeCurrencyAmount: 0, null);

            // Act
            var prepareLockUnspent = await _wallet.PrepareLockUnspentFromAsync(
                from_address: _wallet.RpcOptions.ChainAdminAddress,
                asset_quantities: new Dictionary<string, decimal> { { "", 0 }, { asset.Result, 10 } },
                _lock: true);

            // Assert
            Assert.IsNull(prepareLockUnspent.Error);
            Assert.IsNotNull(prepareLockUnspent.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentFromResult>>(prepareLockUnspent);

            // Act
            var rawExchange = await _wallet.CreateRawExchangeAsync(
                txid: prepareLockUnspent.Result.Txid,
                vout: prepareLockUnspent.Result.Vout,
                ask_assets: new Dictionary<string, int> { { "", 0 } });

            // Assert
            Assert.IsNull(rawExchange.Error);
            Assert.IsNotNull(rawExchange.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(rawExchange);

            // Act
            var appendRaw = await _wallet.AppendRawExchangeAsync(
                hex: rawExchange.Result,
                txid: prepareLockUnspent.Result.Txid,
                vout: prepareLockUnspent.Result.Vout,
                ask_assets: new Dictionary<string, int> { { "", 0 } });

            // Assert
            Assert.IsNull(appendRaw.Error);
            Assert.IsNotNull(appendRaw.Result);
            Assert.IsInstanceOf<RpcResponse<AppendRawExchangeResult>>(appendRaw);

            // Act
            var disable = await _wallet.DisableRawTransactionAsync(tx_hex: appendRaw.Result.Hex);

            // Assert
            Assert.IsNull(disable.Error);
            Assert.IsNotNull(disable.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(disable);
        }

        [Test]
        public async Task ApproveFromTestAsync()
        {
            // Stage
            var jsCode = "function filtertransaction() { var tx=getfiltertransaction(); if (tx.vout.length < 1) return 'One output required'; }";

            // Stage
            var filter = await _wallet.CreateAsync(Entity.TxFilter, StreamFilterEntity.GetUUID(), new { }, jsCode);

            // Act
            RpcResponse<object> actual = await _wallet.ApproveFromAsync(_wallet.RpcOptions.ChainAdminAddress, filter.Result, true); // we are going to expect this to fail since there are no upgrades available

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("BackupWallet test ignored since it halts the blockchain network")]
        public async Task BackupWalletTestAsync()
        {
            // Act
            var actual = await _wallet.BackupWalletAsync("backup.dat");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task CombineUnspentTestAsync()
        {
            // Act
            var actual = await _wallet.CombineUnspentAsync(_wallet.RpcOptions.ChainAdminAddress, 1, 100, 2, 1000, 15);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task CompleteRawExchangeTestAsync()
        {
            // Act
            var prepareLockUnspent = await _wallet.PrepareLockUnspentAsync(new Dictionary<string, int> { { "", 0 } }, true);

            // Assert
            Assert.IsNull(prepareLockUnspent.Error);
            Assert.IsNotNull(prepareLockUnspent.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentResult>>(prepareLockUnspent);

            // Act
            var rawExchange = await _wallet.CreateRawExchangeAsync(prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Assert
            Assert.IsNull(rawExchange.Error);
            Assert.IsNotNull(rawExchange.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(rawExchange);

            // Act
            var appendRaw = await _wallet.AppendRawExchangeAsync(rawExchange.Result, prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Assert
            Assert.IsNull(appendRaw.Error);
            Assert.IsNotNull(appendRaw.Result);
            Assert.IsInstanceOf<RpcResponse<AppendRawExchangeResult>>(appendRaw);

            // Act
            var complete = await _wallet.CompleteRawExchangeAsync(appendRaw.Result.Hex, prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } }, "test".ToHex());

            // Assert
            Assert.IsNull(complete.Error);
            Assert.IsNotNull(complete.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(complete);
        }

        [Test]
        public async Task CreateFromTestAsync()
        {
            // Act
            var actual = await _wallet.CreateFromAsync(_wallet.RpcOptions.ChainAdminAddress, Entity.Stream, StreamEntity.GetUUID(), true, new { });

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task CreateRawExchangeTestAsync()
        {
            // Act
            var prepareLockUnspent = await _wallet.PrepareLockUnspentAsync(new Dictionary<string, int> { { "", 0 } }, true);

            // Assert
            Assert.IsNull(prepareLockUnspent.Error);
            Assert.IsNotNull(prepareLockUnspent.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentResult>>(prepareLockUnspent);

            // Act
            var rawExchange = await _wallet.CreateRawExchangeAsync(prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Assert
            Assert.IsNull(rawExchange.Error);
            Assert.IsNotNull(rawExchange.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(rawExchange);

            // Act
            var disable = await _wallet.DisableRawTransactionAsync(rawExchange.Result);

            // Assert
            Assert.IsNull(disable.Error);
            Assert.IsNotNull(disable.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(disable);
        }

        [Test]
        public async Task CreateRawSendFromTestAsync()
        {
            // Act
            var actual = await _wallet.CreateRawSendFromAsync(_wallet.RpcOptions.ChainAdminAddress, new Dictionary<string, double> { { _wallet.RpcOptions.ChainAdminAddress, 0 } }, Array.Empty<object>(), "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task CreateTestAsync()
        {
            // Act
            var actual = await _wallet.CreateAsync(Entity.Stream, StreamEntity.GetUUID(), true, new { });

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task DecodeRawExchangeTestAsync()
        {
            // Act
            var prepareLockUnspent = await _wallet.PrepareLockUnspentAsync(new Dictionary<string, int> { { "", 0 } }, true);

            // Assert
            Assert.IsNull(prepareLockUnspent.Error);
            Assert.IsNotNull(prepareLockUnspent.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentResult>>(prepareLockUnspent);

            // Act
            var rawExchange = await _wallet.CreateRawExchangeAsync(prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Assert
            Assert.IsNull(rawExchange.Error);
            Assert.IsNotNull(rawExchange.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(rawExchange);

            // Act
            var decode = await _wallet.DecodeRawExchangeAsync(rawExchange.Result, true);

            // Assert
            Assert.IsNull(decode.Error);
            Assert.IsNotNull(decode.Result);
            Assert.IsInstanceOf<RpcResponse<DecodeRawExchangeResult>>(decode);

            // Act
            var disable = await _wallet.DisableRawTransactionAsync(rawExchange.Result);

            // Assert
            Assert.IsNull(disable.Error);
            Assert.IsNotNull(disable.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(disable);
        }

        [Test]
        public async Task DisableRawTransactionTestAsync()
        {
            // Act
            var prepareLockUnspent = await _wallet.PrepareLockUnspentAsync(new Dictionary<string, int> { { "", 0 } }, true);

            // Assert
            Assert.IsNull(prepareLockUnspent.Error);
            Assert.IsNotNull(prepareLockUnspent.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentResult>>(prepareLockUnspent);

            // Act
            var rawExchange = await _wallet.CreateRawExchangeAsync(prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Assert
            Assert.IsNull(rawExchange.Error);
            Assert.IsNotNull(rawExchange.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(rawExchange);

            // Act
            var disable = await _wallet.DisableRawTransactionAsync(rawExchange.Result);

            // Assert
            Assert.IsNull(disable.Error);
            Assert.IsNotNull(disable.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(disable);
        }

        [Test]
        public async Task DumpPrivKeyTestAsync()
        {
            // Act
            var actual = await _wallet.DumpPrivKeyAsync(_wallet.RpcOptions.ChainAdminAddress);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Dumping the wallet seems to slow down the network. Test is passing and ignored.")]
        public async Task DumpWalletTestAync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.DumpWalletAsync("test_async");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Test is implemented and ignored since I don't want to encrypt my wallet in staging")]
        public async Task EncryptWalletTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.EncryptWalletAsync("some_password");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
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
            RpcResponse<object> actual = await _wallet.GetAccountAsync(_wallet.RpcOptions.ChainAdminAddress);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetAddressBalancesTestAsync()
        {
            // Act
            RpcResponse<GetAddressBalancesResult[]> actual = await _wallet.GetAddressBalancesAsync(_wallet.RpcOptions.ChainAdminAddress, 1, false);

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
            var transaction = await _wallet.IssueAsync(_wallet.RpcOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, default);

            // Act
            RpcResponse<GetAddressTransactionResult> actual = await _wallet.GetAddressTransactionAsync(_wallet.RpcOptions.ChainAdminAddress, transaction.Result, true);

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
            var asset = await _wallet.IssueAsync(_wallet.RpcOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, default);

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
            RpcResponse<object> actual = await _wallet.GetMultiBalancesAsync(_wallet.RpcOptions.ChainAdminAddress, null, 1, true, true);

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
            RpcResponse<object> actual = await _wallet.GetReceivedByAddressAsync(_wallet.RpcOptions.ChainAdminAddress, 2);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetStreamItemTestAsync()
        {
            // Stage
            var publish = await _wallet.PublishFromAsync(_wallet.RpcOptions.ChainAdminAddress, "root", ChainEntity.GetUUID(), "Stream item data".ToHex(), "");

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
            await _wallet.PublishFromAsync(_wallet.RpcOptions.ChainAdminAddress, "root", streamKey, "Stream item data".ToHex(), "");

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
            RpcResponse<object> actual = await _wallet.GetStreamPublisherSummaryAsync("root", _wallet.RpcOptions.ChainAdminAddress, "jsonobjectmerge,ignore,recursive");

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
            var txid = await _wallet.IssueFromAsync(_wallet.RpcOptions.ChainAdminAddress, _wallet.RpcOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, 
                new Dictionary<string,string> { { "description", "Some Description" } });

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
            var publish = await _wallet.PublishFromAsync(_wallet.RpcOptions.ChainAdminAddress, "root", ChainEntity.GetUUID(), "Stream item data".ToHex(), "");

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
            var publish = await _wallet.PublishFromAsync(_wallet.RpcOptions.ChainAdminAddress, "root", ChainEntity.GetUUID(), "Stream item data".ToHex(), "");

            // Act
            RpcResponse<GetWalletTransactionResult> actual = await _wallet.GetWalletTransactionAsync(publish.Result, true, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetWalletTransactionResult>>(actual);
        }

        [Test]
        public async Task GrantFromTestAsync()
        {
            // Stage
            var newAddress = await _wallet.GetNewAddressAsync();

            // Act
            RpcResponse<string> actual = await _wallet.GrantFromAsync(_wallet.RpcOptions.ChainAdminAddress, newAddress.Result, Permission.Receive, 0, 1, 1000, "", "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GrantTestAsync()
        {
            // Stage
            var newAddress = await _wallet.GetNewAddressAsync();

            // Act
            RpcResponse<string> actual = await _wallet.GrantAsync(newAddress.Result, Permission.Receive, 0, 1, 1000, "", "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GrantWithDataFromTestAsync()
        {
            // Stage
            var newAddress = await _wallet.GetNewAddressAsync();

            // Act
            RpcResponse<string> actual = await _wallet.GrantWithDataFromAsync(_wallet.RpcOptions.ChainAdminAddress, newAddress.Result, Permission.Receive, "some_data".ToHex(), 0, 1, 1000);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GrantWithDataTestAsync()
        {
            // Stage
            var newAddress = await _wallet.GetNewAddressAsync();

            // Act
            RpcResponse<string> actual = await _wallet.GrantWithDataAsync(newAddress.Result, Permission.Receive, "some_data".ToHex(), 0, 1, 1000);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test, Ignore("I don't want to import any addresses during unit testing")]
        public async Task ImportAddressTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ImportAddressAsync("some_external_address", "some_label", false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("I don't want to import any private keys during unit testing")]
        public async Task ImportPrivKeyTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ImportPrivKeyAsync("some_external_private_key", "some_label", false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Tests impacting the current wallet are ignore while general tests are running")]
        public async Task ImportWalletTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ImportWalletAsync("test", false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        public async Task IssueFromTestStronglyTypedAsync()
        {
            // Act
            RpcResponse<string> act_1 = await _wallet.IssueFromAsync(_wallet.RpcOptions.ChainAdminAddress,
                                                                     _wallet.RpcOptions.ChainAdminAddress,
                                                                     new AssetEntity(),
                                                                     100,
                                                                     1,
                                                                     0.1m,
                                                                     new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_1);

            // Act
            RpcResponse<string> act_2 = await _wallet.IssueFromAsync(_wallet.RpcOptions.ChainAdminAddress,
                                                                     _wallet.RpcOptions.ChainAdminAddress,
                                                                     new AssetEntity(),
                                                                     100,
                                                                     1,
                                                                     0.1m,
                                                                     new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_2);
        }

        public async Task IssueFromTestGenericallyTypedAsync()
        {
            // Act
            RpcResponse<string> act_1 = await _wallet.IssueFromAsync(_wallet.RpcOptions.ChainAdminAddress,
                                                                     _wallet.RpcOptions.ChainAdminAddress,
                                                                     new { name = UUID.NoHyphens, isOpen = true },
                                                                     100,
                                                                     1,
                                                                     0.1m,
                                                                     new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_1);

            // Act
            RpcResponse<string> act_2 = await _wallet.IssueFromAsync(_wallet.RpcOptions.ChainAdminAddress,
                                                                     _wallet.RpcOptions.ChainAdminAddress,
                                                                     new { name = UUID.NoHyphens, isOpen = true },
                                                                     100,
                                                                     1,
                                                                     0.1m,
                                                                     new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_2);
        }

        public async Task IssueFromTestStringNameAsync()
        {
            // Act
            RpcResponse<string> act_1 = await _wallet.IssueFromAsync(_wallet.RpcOptions.ChainAdminAddress,
                                                                     _wallet.RpcOptions.ChainAdminAddress,
                                                                     UUID.NoHyphens,
                                                                     100,
                                                                     1,
                                                                     0.1m,
                                                                     new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_1);

            // Act
            RpcResponse<string> act_2 = await _wallet.IssueFromAsync(_wallet.RpcOptions.ChainAdminAddress,
                                                                     _wallet.RpcOptions.ChainAdminAddress,
                                                                     UUID.NoHyphens,
                                                                     100,
                                                                     1,
                                                                     0.1m,
                                                                     new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_2);
        }

        [Test]
        public async Task IssueMoreFromTestAsync()
        {
            // Stage
            var issue = await _wallet.IssueFromAsync(_wallet.RpcOptions.ChainAdminAddress,
                                                     _wallet.RpcOptions.ChainAdminAddress,
                                                     new AssetEntity(),
                                                     100,
                                                     1,
                                                     0,
                                                     default);

            // Act
            RpcResponse<object> act_1 = await _wallet.IssueMoreFromAsync(_wallet.RpcOptions.ChainAdminAddress,
                                                                         _wallet.RpcOptions.ChainAdminAddress,
                                                                         issue.Result.ToString(),
                                                                         100,
                                                                         0,
                                                                         new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(act_1);


            // Act
            RpcResponse<object> act_2 = await _wallet.IssueMoreFromAsync(_wallet.RpcOptions.ChainAdminAddress,
                                                                         _wallet.RpcOptions.ChainAdminAddress,
                                                                         issue.Result.ToString(),
                                                                         100,
                                                                         0,
                                                                         new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(act_2);
        }

        [Test]
        public async Task IssueMoreTestAsync()
        {
            // Stage
            var issue = await _wallet.IssueAsync(_wallet.RpcOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, default);

            // Act
            RpcResponse<object> act_1 = await _wallet.IssueMoreAsync(_wallet.RpcOptions.ChainAdminAddress,
                                                                     issue.Result.ToString(),
                                                                     100,
                                                                     0,
                                                                     new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(act_1);

            // Act
            RpcResponse<object> act_2 = await _wallet.IssueMoreAsync(_wallet.RpcOptions.ChainAdminAddress,
                                                                     issue.Result.ToString(),
                                                                     100,
                                                                     0,
                                                                     new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(act_2);
        }

        [Test]
        public async Task IssueTestStronlyTypedAsync()
        {
            // Act - ask target network for a new blockchain address
            var newAddress = await _wallet.GetNewAddressAsync();

            // Assert - proof new address response
            Assert.IsNull(newAddress.Error);
            Assert.IsNotNull(newAddress.Result);
            Assert.IsInstanceOf<string>(newAddress.Result);
            Assert.GreaterOrEqual(newAddress.Result.Length, 35);


            // Act - grant the new address receive,send permissions
            var grant = await _wallet.GrantAsync(newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000);

            // Assert - proof grant response
            Assert.IsNull(grant.Error);
            Assert.IsNotNull(grant.Result);
            Assert.IsInstanceOf<string>(grant.Result);
            Assert.GreaterOrEqual(grant.Result.Length, 64);


            // Act - issue a new asset using a AssetEntity class object
            RpcResponse<string> act_1 = await _wallet.IssueAsync(
                toAddress: newAddress.Result,
                assetParams: new AssetEntity(name: UUID.NoHyphens, isOpen: true, restrictions: "send,receive"),
                quantity: 100,
                smallestUnit: 1,
                nativeCurrencyAmount: 0,
                customFields: new Dictionary<string, string>()
                {
                    { "text", "Dictionary test".ToHex() },
                    { "description", "Even more text data".ToHex() }
                });

            // Assert - proof issue response
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<string>(act_1.Result);
            Assert.GreaterOrEqual(act_1.Result.Length, 64);


            // Act - issue a new asset using a generic object
            RpcResponse<string> act_2 = await _wallet.IssueAsync(
                toAddress: newAddress.Result,
                assetParams: new AssetEntity(name: UUID.NoHyphens, isOpen: true, restrictions: "send,receive"),
                quantity: 100,
                smallestUnit: 1,
                nativeCurrencyAmount: 0,
                customFields: new 
                {
                    text = "Dictionary test".ToHex(),
                    description = "Even more text data".ToHex() 
                });

            // Assert - proof issue response
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<string>(act_2.Result);
            Assert.GreaterOrEqual(act_2.Result.Length, 64);
        }

        [Test]
        public async Task IssueTestGenericallyTypedAsync()
        {
            // Act - ask target network for a new blockchain address
            var newAddress = await _wallet.GetNewAddressAsync();

            // Assert - proof new address response
            Assert.IsNull(newAddress.Error);
            Assert.IsNotNull(newAddress.Result);
            Assert.IsInstanceOf<string>(newAddress.Result);
            Assert.GreaterOrEqual(newAddress.Result.Length, 35);


            // Act - grant the new address receive,send permissions
            var grant = await _wallet.GrantAsync(newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000);

            // Assert - proof grant response
            Assert.IsNull(grant.Error);
            Assert.IsNotNull(grant.Result);
            Assert.IsInstanceOf<string>(grant.Result);
            Assert.GreaterOrEqual(grant.Result.Length, 64);


            // Act - issue a new asset using a strongly typed AssetEntity class object
            RpcResponse<string> act_1 = await _wallet.IssueAsync(
                toAddress: newAddress.Result,
                assetParams: new { name = UUID.NoHyphens, isOpen = true, restrictions = "send,receive" },
                quantity: 100,
                smallestUnit: 1,
                nativeCurrencyAmount: 0,
                customFields: new Dictionary<string, string>()
                {
                    { "text", "Dictionary test".ToHex() },
                    { "description", "Even more text data".ToHex() }
                });

            // Assert - proof issue response
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<string>(act_1.Result);
            Assert.GreaterOrEqual(act_1.Result.Length, 64);


            // Act - issue a new asset using a strongly typed AssetEntity class object
            RpcResponse<string> act_2 = await _wallet.IssueAsync(
                toAddress: newAddress.Result,
                assetParams: new { name = UUID.NoHyphens, isOpen = true, restrictions = "send,receive" },
                quantity: 100,
                smallestUnit: 1,
                nativeCurrencyAmount: 0,
                customFields: new
                {
                    text = "Dictionary test".ToHex(),
                    description = "Even more text data".ToHex()
                });

            // Assert - proof issue response
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<string>(act_2.Result);
            Assert.GreaterOrEqual(act_2.Result.Length, 64);
        }

        [Test]
        public async Task IssueTestStringNameAsync()
        {
            // Act - ask target network for a new blockchain address
            var newAddress = await _wallet.GetNewAddressAsync();

            // Assert - proof new address response
            Assert.IsNull(newAddress.Error);
            Assert.IsNotNull(newAddress.Result);
            Assert.IsInstanceOf<string>(newAddress.Result);
            Assert.GreaterOrEqual(newAddress.Result.Length, 35);


            // Act - grant the new address receive,send permissions
            var grant = await _wallet.GrantAsync(newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000);

            // Assert - proof grant response
            Assert.IsNull(grant.Error);
            Assert.IsNotNull(grant.Result);
            Assert.IsInstanceOf<string>(grant.Result);
            Assert.GreaterOrEqual(grant.Result.Length, 64);


            // Act - issue a new asset using a strongly typed AssetEntity class object
            RpcResponse<string> act_1 = await _wallet.IssueAsync(
                toAddress: newAddress.Result,
                assetName: UUID.NoHyphens,
                quantity: 100,
                smallestUnit: 1,
                nativeCurrencyAmount: 0,
                customFields: new Dictionary<string, string>()
                {
                    { "text", "Dictionary test".ToHex() },
                    { "description", "Even more text data".ToHex() }
                });

            // Assert - proof issue response
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<string>(act_1.Result);
            Assert.GreaterOrEqual(act_1.Result.Length, 64);


            // Act - issue a new asset using a strongly typed AssetEntity class object
            RpcResponse<string> act_2 = await _wallet.IssueAsync(
                toAddress: newAddress.Result,
                assetName: UUID.NoHyphens,
                quantity: 100,
                smallestUnit: 1,
                nativeCurrencyAmount: 0,
                customFields: new
                {
                    text = "Dictionary test".ToHex(),
                    description = "Even more text data".ToHex()
                });

            // Assert - proof issue response
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<string>(act_2.Result);
            Assert.GreaterOrEqual(act_2.Result.Length, 64);
        }

        [Test]
        public async Task KeyPoolRefillTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.KeyPoolRefillAsync(200);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
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
            RpcResponse<ListAddressTransactionsResult[]> actual = await _wallet.ListAddressTransactionsAsync(_wallet.RpcOptions.ChainAdminAddress, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListAddressTransactionsResult[]>>(actual);
        }

        [Test]
        public async Task ListAssetTransactionsTestAsync()
        {
            // Stage
            var issue = await _wallet.IssueAsync(_wallet.RpcOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, default);

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
            RpcResponse<ListStreamPublisherItemsResult[]> actual = await _wallet.ListStreamPublisherItemsAsync("root", _wallet.RpcOptions.ChainAdminAddress, true, 10, 0, true);

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
            RpcResponse<object> actual = await _wallet.ListStreamQueryItemsAsync("root", new { publisher = _wallet.RpcOptions.ChainAdminAddress }, true);

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
            RpcResponse<ListUnspentResult[]> actual = await _wallet.ListUnspentAsync(2, 100, new[] { _wallet.RpcOptions.ChainAdminAddress });

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

        [Test]
        public async Task LockUnspentTestAsync()
        {
            // Stage
            var unspent = await _wallet.PrepareLockUnspentAsync(new Dictionary<string, int> { { "", 0 } }, false);

            // Act
            RpcResponse<object> actual = await _wallet.LockUnspentAsync(false, new Transaction[] { new Transaction { Txid = unspent.Result.Txid, Vout = unspent.Result.Vout } });

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need move, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task MoveTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.MoveAsync("from_account", "to_account", 0.01, 6, "Testing the Move function");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task PrepareLockUnspentTestAsync()
        {
            // Act
            RpcResponse<PrepareLockUnspentResult> actual = await _wallet.PrepareLockUnspentAsync(new Dictionary<string, double> { { "", 0 } }, false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentResult>>(actual);
        }

        [Test]
        public async Task PrepareLockUnspentFromTestAsync()
        {
            // Act
            RpcResponse<PrepareLockUnspentFromResult> actual = await _wallet.PrepareLockUnspentFromAsync(_wallet.RpcOptions.ChainAdminAddress, new Dictionary<string, double> { { "", 0 } }, false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentFromResult>>(actual);
        }

        [Test]
        public async Task PublishTestAsync()
        {
            // Act
            RpcResponse<string> actual = await _wallet.PublishAsync("root", "test_key", "some_data".ToHex(), "offchain");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task PublishFromTestAsync()
        {
            // Act
            RpcResponse<string> actual = await _wallet.PublishFromAsync(_wallet.RpcOptions.ChainAdminAddress, "root", "test_key", "some_data".ToHex(), "offchain");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task PublishMultiTestAsync()
        {
            // Act
            RpcResponse<string> actual = await _wallet.PublishMultiAsync("root", new object[] { new { key = "some_key", data = "some_data".ToHex() } }, "offchain");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task PublishMultiFromTestAsync()
        {
            // Act
            RpcResponse<string> actual = await _wallet.PublishMultiFromAsync(_wallet.RpcOptions.ChainAdminAddress, "root", new object[] { new { key = "some_key", data = "some_data".ToHex() } }, "offchain");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test, Ignore("ResendWalletTransaction test is deffered from normal unit testing")]
        public async Task ResendWalletTransactionsTestAsync()
        {
            // Act - ttempt to resend the current wallet's transaction
            RpcResponse<object> actual = await _wallet.ResendWalletTransactionsAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task RevokeTestAsync()
        {
            // Stage - Ask the blockchain network for a new address
            var newAddress = await _wallet.GetNewAddressAsync();

            // Stage - Grant new address receive permissions
            await _wallet.GrantAsync(newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 0, 10000, "", "");

            // Act - Revoke send permission
            RpcResponse<object> actual = await _wallet.RevokeAsync(newAddress.Result, "send", 0, "Permissions", "Permissions set");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task RevokeFromTestAsync()
        {
            // Stage - Ask the blockchain network for a new address
            var newAddress = await _wallet.GetNewAddressAsync();

            // Stage - Grant new address receive permissions
            await _wallet.GrantAsync(newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 0, 10000, "", "");

            // Act - Revoke send permission
            RpcResponse<object> actual = await _wallet.RevokeFromAsync(_wallet.RpcOptions.ChainAdminAddress, newAddress.Result, "send", 0, "", "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SendTestAsync()
        {
            // Act
            RpcResponse<string> actual = await _wallet.SendAsync(_wallet.RpcOptions.ChainAdminAddress, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task SendAssetTestAsync()
        {
            // Stage
            var asset = await _wallet.IssueAsync(_wallet.RpcOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, default);

            // Act
            RpcResponse<object> actual = await _wallet.SendAssetAsync(_wallet.RpcOptions.ChainAdminAddress, asset.Result, 1, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SendAssetFromTestAsync()
        {
            // Stage
            var newAddress = await _wallet.GetNewAddressAsync();

            // Stage
            await _wallet.GrantAsync(newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000, "", "");

            // Stage
            var asset = await _wallet.IssueAsync(_wallet.RpcOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0,
                new Dictionary<string, string> { { "text", "text to hex".ToHex() } });

            // Act
            RpcResponse<object> actual = await _wallet.SendAssetFromAsync(_wallet.RpcOptions.ChainAdminAddress, newAddress.Result, asset.Result, 1, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SendFromTestAsync()
        {
            // Stage
            var newAddress = await _wallet.GetNewAddressAsync();

            // Stage
            await _wallet.GrantAsync(newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000, "", "");

            // Act
            RpcResponse<object> actual = await _wallet.SendFromAsync(_wallet.RpcOptions.ChainAdminAddress, newAddress.Result, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need sendfrom, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task SendFromAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.SendFromAccountAsync(_wallet.RpcOptions.ChainAdminAddress, _wallet.RpcOptions.ChainAdminAddress, .001, 2, "Comment Text", "Comment_To text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need sendmany, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task SendManyTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.SendManyAsync("", new object[] { new Dictionary<string, double> { { _wallet.RpcOptions.ChainAdminAddress, 1 } } }, 2, "Comment text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SendWithDataTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.SendWithDataAsync(_wallet.RpcOptions.ChainAdminAddress, 0, "some data".ToHex());

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SendWithDataFromTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.SendWithDataFromAsync(_wallet.RpcOptions.ChainAdminAddress, _wallet.RpcOptions.ChainAdminAddress, 0, "some data".ToHex());

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need move, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task SetAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.SetAccountAsync(_wallet.RpcOptions.ChainAdminAddress, "master_account");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Ignored since I do not want to change the TxFee while other transactions are runningh")]
        public async Task SetTxFeeTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.SetTxFeeAsync(0.0001);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SignMessageTestAsync()
        {
            // Act
            RpcResponse<string> actual = await _wallet.SignMessageAsync(_wallet.RpcOptions.ChainAdminAddress, "Testing the SignMessage function");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task SubscribeTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.SubscribeAsync("root", false, "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task TxOutToBinaryCacheTestAsync()
        {
            // Stage
            var binaryCache = await _utility.CreateBinaryCacheAsync();

            // Stage
            var publish = await _wallet.PublishFromAsync(_wallet.RpcOptions.ChainAdminAddress, "root", ChainEntity.GetUUID(), "A bunch of text data that will be transcribed to this this publish event and this one is async brotato chip".ToHex(), "");

            // Stage
            var transaction = await _wallet.GetAddressTransactionAsync(_wallet.RpcOptions.ChainAdminAddress, publish.Result, true);

            // Act
            RpcResponse<double> actual = await _wallet.TxOutToBinaryCacheAsync(binaryCache.Result, transaction.Result.Txid, transaction.Result.Vout[0].N, 100000, 0);

            // Act
            await _utility.DeleteBinaryCacheAsync(binaryCache.Result);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<double>>(actual);
        }

        [Test]
        public async Task UnsubscribeTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.UnsubscribeAsync("root", false);

            // Act
            await _wallet.SubscribeAsync("root", false, "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Wallet related tests are ignored while general tests are running")]
        public async Task WalletLockTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.WalletLockAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Wallet related tests are ignored while general tests are running")]
        public async Task WalletPassphraseTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.WalletPassphraseAsync("wallet_passphrase", 10);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Wallet related tests are ignored while general tests are running")]
        public async Task WalletPassphraseChangeTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.WalletPassphraseChangeAsync("old_passphrase", "new_passphrase");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }
    }
}
