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
    public class IMultiChainRpcWalletExplicitTests
    {
        // private fields
        private readonly IMultiChainRpcUtility _utility;
        private readonly IMultiChainRpcWallet _wallet;
        private readonly IMultiChainRpcGeneral _blockchain;

        /// <summary>
        /// Create a new WalletServiceTests instance
        /// </summary>
        public IMultiChainRpcWalletExplicitTests()
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
            RpcResponse<object> actual = await _wallet.AddMultiSigAddressAsync(
                blockchainName: _wallet.RpcOptions.ChainName,
                id: nameof(AddMultiSigAddressTestAsync),
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
            RpcResponse<string> asset = await _wallet.IssueAsync(
                blockchainName: _wallet.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                toAddress: _wallet.RpcOptions.ChainAdminAddress,
                assetParams: new AssetEntity(),
                quantity: 100,
                smallestUnit: 1, nativeCurrencyAmount: 0, null);

            // Act
            RpcResponse<PrepareLockUnspentFromResult> prepareLockUnspent = await _wallet.PrepareLockUnspentFromAsync(
                blockchainName: _wallet.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                from_address: _wallet.RpcOptions.ChainAdminAddress,
                asset_quantities: new Dictionary<string, decimal> { { asset.Result, 10 } },
                _lock: true);

            // Act
            RpcResponse<string> rawExchange = await _wallet.CreateRawExchangeAsync(
                blockchainName: _wallet.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                txid: prepareLockUnspent.Result.Txid,
                vout: prepareLockUnspent.Result.Vout,
                ask_assets: new Dictionary<string, int> { { "", 0 } });

            // Act
            RpcResponse<AppendRawExchangeResult> appendRaw = await _wallet.AppendRawExchangeAsync(
                blockchainName: _wallet.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                hex: rawExchange.Result,
                txid: prepareLockUnspent.Result.Txid,
                vout: prepareLockUnspent.Result.Vout,
                ask_assets: new Dictionary<string, int> { { "", 0 } });

            // Act
            RpcResponse<object> disable = await _wallet.DisableRawTransactionAsync(
                blockchainName: _wallet.RpcOptions.ChainName,
                id: nameof(AppendRawExchangeTestAsync),
                tx_hex: appendRaw.Result.Hex);

            // Assert
            Assert.IsNull(prepareLockUnspent.Error);
            Assert.IsNotNull(prepareLockUnspent.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentFromResult>>(prepareLockUnspent);

            // Assert
            Assert.IsNull(rawExchange.Error);
            Assert.IsNotNull(rawExchange.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(rawExchange);

            // Assert
            Assert.IsNull(appendRaw.Error);
            Assert.IsNotNull(appendRaw.Result);
            Assert.IsInstanceOf<RpcResponse<AppendRawExchangeResult>>(appendRaw);

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
            var filter = await _wallet.CreateAsync(_wallet.RpcOptions.ChainName, nameof(ApproveFromTestAsync), Entity.TxFilter, StreamFilterEntity.GetUUID(), new { }, jsCode);

            // Act
            RpcResponse<object> actual = await _wallet.ApproveFromAsync(_wallet.RpcOptions.ChainName, nameof(ApproveFromTestAsync), _wallet.RpcOptions.ChainAdminAddress, filter.Result, true); // we are going to expect this to fail since there are no upgrades available

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("BackupWallet test ignored since it halts the blockchain network")]
        public async Task BackupWalletTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.BackupWalletAsync(_wallet.RpcOptions.ChainName, nameof(BackupWalletTestAsync), "backup.dat");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task CombineUnspentTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.CombineUnspentAsync(_wallet.RpcOptions.ChainName, nameof(CombineUnspentTestAsync), _wallet.RpcOptions.ChainAdminAddress, 1, 100, 2, 1000, 15);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task CompleteRawExchangeTestAsync()
        {
            // Act
            RpcResponse<PrepareLockUnspentResult> prepareLockUnspent = await _wallet.PrepareLockUnspentAsync(_wallet.RpcOptions.ChainName, nameof(CompleteRawExchangeTestAsync), new Dictionary<string, int> { { "", 0 } }, true);

            // Act
            RpcResponse<string> rawExchange = await _wallet.CreateRawExchangeAsync(_wallet.RpcOptions.ChainName, nameof(CompleteRawExchangeTestAsync), prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            RpcResponse<AppendRawExchangeResult> appendRaw = await _wallet.AppendRawExchangeAsync(_wallet.RpcOptions.ChainName, nameof(CompleteRawExchangeTestAsync), rawExchange.Result, prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            RpcResponse<object> complete = await _wallet.CompleteRawExchangeAsync(_wallet.RpcOptions.ChainName, nameof(CompleteRawExchangeTestAsync), appendRaw.Result.Hex, prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } }, "test".ToHex());

            // Assert
            Assert.IsNull(prepareLockUnspent.Error);
            Assert.IsNotNull(prepareLockUnspent.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentResult>>(prepareLockUnspent);

            // Assert
            Assert.IsNull(rawExchange.Error);
            Assert.IsNotNull(rawExchange.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(rawExchange);

            // Assert
            Assert.IsNull(appendRaw.Error);
            Assert.IsNotNull(appendRaw.Result);
            Assert.IsInstanceOf<RpcResponse<AppendRawExchangeResult>>(appendRaw);

            // Assert
            Assert.IsNull(complete.Error);
            Assert.IsNotNull(complete.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(complete);
        }

        [Test]
        public async Task CreateFromTestAsync()
        {
            // Act
            RpcResponse<string> actual = await _wallet.CreateFromAsync(_wallet.RpcOptions.ChainName, nameof(CreateFromTestAsync), _wallet.RpcOptions.ChainAdminAddress, Entity.Stream, StreamEntity.GetUUID(), true, new { });

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task CreateRawExchangeTestAsync()
        {
            // Act
            var asset = new AssetEntity();

            var issue = await _wallet.IssueAsync(_wallet.RpcOptions.ChainAdminAddress, asset, 100, 1, 0,
                new Dictionary<string, string> { { "text", "Some Text".ToHex() } });
            RpcResponse<PrepareLockUnspentResult> prepareLockUnspent = await _wallet.PrepareLockUnspentAsync(_wallet.RpcOptions.ChainName, nameof(CreateRawExchangeTestAsync), new Dictionary<string, int> { { asset.Name, 2 } }, true);

            // Act
            RpcResponse<string> rawExchange = await _wallet.CreateRawExchangeAsync(_wallet.RpcOptions.ChainName, nameof(CreateRawExchangeTestAsync), prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            RpcResponse<object> disable = await _wallet.DisableRawTransactionAsync(_wallet.RpcOptions.ChainName, nameof(CreateRawExchangeTestAsync), rawExchange.Result);

            // Assert
            Assert.IsNull(prepareLockUnspent.Error);
            Assert.IsNotNull(prepareLockUnspent.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentResult>>(prepareLockUnspent);

            // Assert
            Assert.IsNull(rawExchange.Error);
            Assert.IsNotNull(rawExchange.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(rawExchange);

            // Assert
            Assert.IsNull(disable.Error);
            Assert.IsNotNull(disable.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(disable);
        }

        [Test]
        public async Task CreateRawSendFromTestAsync()
        {
            // Act

            RpcResponse<object> actual = await _wallet.CreateRawSendFromAsync(_wallet.RpcOptions.ChainName, nameof(CreateRawSendFromTestAsync), _wallet.RpcOptions.ChainAdminAddress, new Dictionary<string, double> { { _wallet.RpcOptions.ChainAdminAddress, 0 } }, Array.Empty<object>(), "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task CreateTestAsync()
        {
            // Act
            RpcResponse<string> actual = await _wallet.CreateAsync(_wallet.RpcOptions.ChainName, nameof(CreateTestAsync), Entity.Stream, StreamEntity.GetUUID(), true, new { });

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task DecodeRawExchangeTestAsync()
        {
            // Act
            var asset = new AssetEntity();

            var issue = await _wallet.IssueAsync(_wallet.RpcOptions.ChainAdminAddress, asset, 100, 1, 0, null);
            RpcResponse<PrepareLockUnspentResult> prepareLockUnspent = await _wallet.PrepareLockUnspentAsync(_wallet.RpcOptions.ChainName, nameof(DecodeRawExchangeTestAsync), new Dictionary<string, int> { { asset.Name, 2 } }, true);

            // Act
            RpcResponse<string> rawExchange = await _wallet.CreateRawExchangeAsync(_wallet.RpcOptions.ChainName, nameof(DecodeRawExchangeTestAsync), prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            RpcResponse<DecodeRawExchangeResult> decode = await _wallet.DecodeRawExchangeAsync(_wallet.RpcOptions.ChainName, nameof(DecodeRawExchangeTestAsync), rawExchange.Result, true);

            // Act
            RpcResponse<object> disable = await _wallet.DisableRawTransactionAsync(_wallet.RpcOptions.ChainName, nameof(DecodeRawExchangeTestAsync), rawExchange.Result);

            // Assert
            Assert.IsNull(prepareLockUnspent.Error);
            Assert.IsNotNull(prepareLockUnspent.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentResult>>(prepareLockUnspent);

            // Assert
            Assert.IsNull(rawExchange.Error);
            Assert.IsNotNull(rawExchange.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(rawExchange);

            // Assert
            Assert.IsNull(decode.Error);
            Assert.IsNotNull(decode.Result);
            Assert.IsInstanceOf<RpcResponse<DecodeRawExchangeResult>>(decode);

            // Assert
            Assert.IsNull(disable.Error);
            Assert.IsNotNull(disable.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(disable);
        }

        [Test]
        public async Task DisableRawTransactionTestAsync()
        {
            // Act
            var asset = new AssetEntity();

            var issue = await _wallet.IssueAsync(_wallet.RpcOptions.ChainAdminAddress, asset, 100, 1, 0, new Dictionary<string, string> { { "text", "Some Text".ToHex() } });
            RpcResponse<PrepareLockUnspentResult> prepareLockUnspent = await _wallet.PrepareLockUnspentAsync(_wallet.RpcOptions.ChainName, nameof(DisableRawTransactionTestAsync), new Dictionary<string, int> { { asset.Name, 2 } }, true);

            // Act
            RpcResponse<string> rawExchange = await _wallet.CreateRawExchangeAsync(_wallet.RpcOptions.ChainName, nameof(DisableRawTransactionTestAsync), prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            RpcResponse<object> disable = await _wallet.DisableRawTransactionAsync(_wallet.RpcOptions.ChainName, nameof(DisableRawTransactionTestAsync), rawExchange.Result);

            // Assert
            Assert.IsNull(prepareLockUnspent.Error);
            Assert.IsNotNull(prepareLockUnspent.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentResult>>(prepareLockUnspent);

            // Assert
            Assert.IsNull(rawExchange.Error);
            Assert.IsNotNull(rawExchange.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(rawExchange);

            // Assert
            Assert.IsNull(disable.Error);
            Assert.IsNotNull(disable.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(disable);
        }

        [Test]
        public async Task DumpPrivKeyTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.DumpPrivKeyAsync(_wallet.RpcOptions.ChainName, nameof(DumpPrivKeyTestAsync), _wallet.RpcOptions.ChainAdminAddress);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Dumping the wallet seems to slow down the network. Test is passing and ignored.")]
        public async Task DumpWalletTestAync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.DumpWalletAsync(_wallet.RpcOptions.ChainName, nameof(DumpWalletTestAync), "test_async");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Test is implemented and ignored since I don't want to encrypt my wallet in staging")]
        public async Task EncryptWalletTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.EncryptWalletAsync(_wallet.RpcOptions.ChainName, nameof(EncryptWalletTestAsync), "some_password");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetAccountAddressTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetAccountAddressAsync(_wallet.RpcOptions.ChainName, nameof(GetAccountAddressTestAsync), "some_account_name");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetAccountAsync(_wallet.RpcOptions.ChainName, nameof(GetAccountTestAsync), _wallet.RpcOptions.ChainAdminAddress);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetAddressBalancesTestAsync()
        {
            // Act
            RpcResponse<GetAddressBalancesResult[]> actual = await _wallet.GetAddressBalancesAsync(_wallet.RpcOptions.ChainName, nameof(GetAddressBalancesTestAsync), _wallet.RpcOptions.ChainAdminAddress, 1, false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAddressBalancesResult[]>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetAddressesByAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetAddressesByAccountAsync(_wallet.RpcOptions.ChainName, nameof(GetAddressesByAccountTestAsync), "some_account_name");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetAddressesTestAsync()
        {
            // Act
            RpcResponse<GetAddressesResult[]> actual = await _wallet.GetAddressesAsync(_wallet.RpcOptions.ChainName, nameof(GetAddressesTestAsync), true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAddressesResult[]>>(actual);
        }

        [Test]
        public async Task GetAddressTransactionTestAsync()
        {
            // Stage
            var transaction = await _wallet.IssueAsync(_wallet.RpcOptions.ChainName, nameof(GetAddressTransactionTestAsync), _wallet.RpcOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, default);

            // Act
            RpcResponse<GetAddressTransactionResult> actual = await _wallet.GetAddressTransactionAsync(_wallet.RpcOptions.ChainName, nameof(GetAddressTransactionTestAsync), _wallet.RpcOptions.ChainAdminAddress, transaction.Result, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAddressTransactionResult>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetAssetBalancesTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetAssetBalancesAsync(_wallet.RpcOptions.ChainName, nameof(GetAssetBalancesTestAsync), "some_account_name", 2, true, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetAssetTransactionTestAsync()
        {
            // Stage
            var asset = await _wallet.IssueAsync(_wallet.RpcOptions.ChainName, nameof(GetAssetTransactionTestAsync), _wallet.RpcOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, default);

            // Stage
            await _wallet.SubscribeAsync(_wallet.RpcOptions.ChainName, nameof(GetAssetTransactionTestAsync), asset.Result, false, "");

            // Act
            RpcResponse<GetAssetTransactionResult> actual = await _wallet.GetAssetTransactionAsync(_wallet.RpcOptions.ChainName, nameof(GetAssetTransactionTestAsync), asset.Result, asset.Result, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAssetTransactionResult>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetBalanceTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetBalanceAsync(_wallet.RpcOptions.ChainName, nameof(GetBalanceTestAsync), "", 1, false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetMultiBalancesTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetMultiBalancesAsync(_wallet.RpcOptions.ChainName, nameof(GetMultiBalancesTestAsync), _wallet.RpcOptions.ChainAdminAddress, null, 1, true, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetNewAddressTestAsync()
        {
            // Act
            RpcResponse<string> actual = await _wallet.GetNewAddressAsync("");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GetRawChangeAddressTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetRawChangeAddressAsync(_wallet.RpcOptions.ChainName, nameof(GetRawChangeAddressTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetReceivedByAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetReceivedByAccountAsync(_wallet.RpcOptions.ChainName, nameof(GetReceivedByAccountTestAsync), "some_account_name", 2);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetReceivedByAddressTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetReceivedByAddressAsync(_wallet.RpcOptions.ChainName, nameof(GetReceivedByAddressTestAsync), _wallet.RpcOptions.ChainAdminAddress, 2);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetStreamItemTestAsync()
        {
            // Stage
            var publish = await _wallet.PublishFromAsync(_wallet.RpcOptions.ChainName, nameof(GetStreamItemTestAsync), _wallet.RpcOptions.ChainAdminAddress, "root", ChainEntity.GetUUID(), "Stream item data".ToHex(), "");

            // Act
            RpcResponse<GetStreamItemResult> actual = await _wallet.GetStreamItemAsync(_wallet.RpcOptions.ChainName, nameof(GetStreamItemTestAsync), "root", publish.Result, true);

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
            await _wallet.PublishFromAsync(_wallet.RpcOptions.ChainName, nameof(GetStreamKeySummaryTestAsync), _wallet.RpcOptions.ChainAdminAddress, "root", streamKey, "Stream item data".ToHex(), "");

            // Act
            RpcResponse<object> actual = await _wallet.GetStreamKeySummaryAsync(_wallet.RpcOptions.ChainName, nameof(GetStreamKeySummaryTestAsync), "root", streamKey, "jsonobjectmerge,ignore,recursive");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetStreamPublisherSummaryTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetStreamPublisherSummaryAsync(_wallet.RpcOptions.ChainName, nameof(GetStreamPublisherSummaryTestAsync), "root", _wallet.RpcOptions.ChainAdminAddress, "jsonobjectmerge,ignore,recursive");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetTotalBalancesTestAsync()
        {
            // Act
            RpcResponse<GetTotalBalancesResult[]> actual = await _wallet.GetTotalBalancesAsync(_wallet.RpcOptions.ChainName, nameof(GetTotalBalancesTestAsync), 1, true, false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetTotalBalancesResult[]>>(actual);
        }

        [Test]
        public async Task GetTransactionTestAsync()
        {
            // Stage
            var txid = await _wallet.IssueFromAsync(_wallet.RpcOptions.ChainName, nameof(GetTransactionTestAsync), _wallet.RpcOptions.ChainAdminAddress, _wallet.RpcOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, new Dictionary<string, string> { { "description", "Some Description" } });

            // Act
            RpcResponse<GetTransactionResult> actual = await _wallet.GetTransactionAsync(_wallet.RpcOptions.ChainName, nameof(GetTransactionTestAsync), txid.Result, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetTransactionResult>>(actual);
        }

        [Test]
        public async Task GetTxOutDataTestAsync()
        {
            // Stage
            var publish = await _wallet.PublishFromAsync(_wallet.RpcOptions.ChainName, nameof(GetTxOutDataTestAsync), _wallet.RpcOptions.ChainAdminAddress, "root", ChainEntity.GetUUID(), "Stream item data".ToHex(), "");

            // Stage
            var transaction = await _wallet.GetTransactionAsync(_wallet.RpcOptions.ChainName, nameof(GetTxOutDataTestAsync), publish.Result, true);

            // Act
            RpcResponse<object> actual = await _wallet.GetTxOutDataAsync(_wallet.RpcOptions.ChainName, nameof(GetTxOutDataTestAsync), transaction.Result.Txid, 0, 10, 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetUnconfirmedBalanceTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.GetUnconfirmedBalanceAsync(_wallet.RpcOptions.ChainName, nameof(GetUnconfirmedBalanceTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetWalletInfoTestAsync()
        {
            // Act
            RpcResponse<GetWalletInfoResult> actual = await _wallet.GetWalletInfoAsync(_wallet.RpcOptions.ChainName, nameof(GetWalletInfoTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetWalletInfoResult>>(actual);
        }

        [Test]
        public async Task GetWalletTransactionTestAsync()
        {
            // Stage
            var publish = await _wallet.PublishFromAsync(_wallet.RpcOptions.ChainName, nameof(GetWalletTransactionTestAsync), _wallet.RpcOptions.ChainAdminAddress, "root", ChainEntity.GetUUID(), "Stream item data".ToHex(), "");

            // Act
            RpcResponse<GetWalletTransactionResult> actual = await _wallet.GetWalletTransactionAsync(_wallet.RpcOptions.ChainName, nameof(GetWalletTransactionTestAsync), publish.Result, true, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetWalletTransactionResult>>(actual);
        }

        [Test]
        public async Task GrantFromTestAsync()
        {
            // Stage
            var newAddress = await _wallet.GetNewAddressAsync(blockchainName: _wallet.RpcOptions.ChainName, nameof(GrantFromTestAsync), "");

            // Act
            RpcResponse<string> actual = await _wallet.GrantFromAsync(_wallet.RpcOptions.ChainName, nameof(GrantFromTestAsync), _wallet.RpcOptions.ChainAdminAddress, newAddress.Result, Permission.Receive, 0, 1, 1000, "", "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GrantTestAsync()
        {
            // Stage
            var newAddress = await _wallet.GetNewAddressAsync(blockchainName: _wallet.RpcOptions.ChainName, nameof(GrantFromTestAsync), "");

            // Act
            RpcResponse<string> actual = await _wallet.GrantAsync(_wallet.RpcOptions.ChainName, nameof(GrantTestAsync), newAddress.Result, Permission.Receive, 0, 1, 1000, "", "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GrantWithDataFromTestAsync()
        {
            // Stage
            var newAddress = await _wallet.GetNewAddressAsync(blockchainName: _wallet.RpcOptions.ChainName, nameof(GrantFromTestAsync), "");

            // Act
            RpcResponse<string> actual = await _wallet.GrantWithDataFromAsync(_wallet.RpcOptions.ChainName, nameof(GrantWithDataFromTestAsync), _wallet.RpcOptions.ChainAdminAddress, newAddress.Result, Permission.Receive, "some_data".ToHex(), 0, 1, 1000);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GrantWithDataTestAsync()
        {
            // Stage
            var newAddress = await _wallet.GetNewAddressAsync(blockchainName: _wallet.RpcOptions.ChainName, nameof(GrantFromTestAsync), "");

            // Act
            RpcResponse<string> actual = await _wallet.GrantWithDataAsync(_wallet.RpcOptions.ChainName, nameof(GrantWithDataTestAsync), newAddress.Result, Permission.Receive, "some_data".ToHex(), 0, 1, 1000);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test, Ignore("I don't want to import any addresses during unit testing")]
        public async Task ImportAddressTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ImportAddressAsync(_wallet.RpcOptions.ChainName, nameof(ImportAddressTestAsync), "some_external_address", "some_label", false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("I don't want to import any private keys during unit testing")]
        public async Task ImportPrivKeyTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ImportPrivKeyAsync(_wallet.RpcOptions.ChainName, nameof(ImportPrivKeyTestAsync), "some_external_private_key", "some_label", false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Tests impacting the current wallet are ignore while general tests are running")]
        public async Task ImportWalletTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ImportWalletAsync(_wallet.RpcOptions.ChainName, nameof(ImportWalletTestAsync), "test", false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        public async Task IssueFromTestStronglyTypedAsync()
        {
            // Act
            RpcResponse<string> act_1 = await _wallet.IssueFromAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, _wallet.RpcOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0.1m, new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_1);

            // Act
            RpcResponse<string> act_2 = await _wallet.IssueFromAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, _wallet.RpcOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0.1m, new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_2);
        }

        public async Task IssueFromTestGenericallyTypedAsync()
        {
            // Act
            RpcResponse<string> act_1 = await _wallet.IssueFromAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, _wallet.RpcOptions.ChainAdminAddress, new { name = UUID.NoHyphens }, 100, 1, 0.1m, new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_1);

            // Act
            RpcResponse<string> act_2 = await _wallet.IssueFromAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, _wallet.RpcOptions.ChainAdminAddress, new { name = UUID.NoHyphens }, 100, 1, 0.1m, new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_2);
        }

        public async Task IssueFromTestStringNameAsync()
        {
            // Act
            RpcResponse<string> act_1 = await _wallet.IssueFromAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, _wallet.RpcOptions.ChainAdminAddress, UUID.NoHyphens, 100, 1, 0.1m, new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_1);

            // Act
            RpcResponse<string> act_2 = await _wallet.IssueFromAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, _wallet.RpcOptions.ChainAdminAddress, UUID.NoHyphens, 100, 1, 0.1m, new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_2);
        }

        [Test]
        public async Task IssueMoreFromTestAsync()
        {
            // Stage
            var issue = await _wallet.IssueFromAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, _wallet.RpcOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, new Dictionary<string, string> { { "text", "some text in hex".ToHex() } });

            // Act
            RpcResponse<object> act_1 = await _wallet.IssueMoreFromAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, _wallet.RpcOptions.ChainAdminAddress, issue.Result.ToString(), 100, 0, new Dictionary<string, string> { { "text", "some text in hex".ToHex() } });

            // Assert
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(act_1);

            // Act
            RpcResponse<object> act_2 = await _wallet.IssueMoreFromAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, _wallet.RpcOptions.ChainAdminAddress, issue.Result.ToString(), 100, 0, new { text = "some text in hex".ToHex() });

            // Assert
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(act_2);
        }

        [Test]
        public async Task IssueMoreTestAsync()
        {
            // Stage
            var issue = await _wallet.IssueAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, new Dictionary<string, string> { { "text", "some text in hex".ToHex() } });

            // Act
            RpcResponse<object> act_1 = await _wallet.IssueMoreAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, issue.Result.ToString(), 100, 0, new Dictionary<string, string> { { "text", "some text in hex".ToHex() } });

            // Assert
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(act_1);

            // Act
            RpcResponse<object> act_2 = await _wallet.IssueMoreAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, issue.Result.ToString(), 100, 0, new { text = "some text in hex".ToHex() });

            // Assert
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(act_2);
        }

        [Test]
        public async Task IssueTestStronglyTypedAsync()
        {
            // Stage
            var newAddress = await _wallet.GetNewAddressAsync(blockchainName: _wallet.RpcOptions.ChainName, nameof(GrantFromTestAsync), "");

            // Stage
            await _wallet.GrantAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000, "", "");

            // Act
            RpcResponse<string> act_1 = await _wallet.IssueAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, newAddress.Result, new AssetEntity(), 100, 1, 0, new Dictionary<string, string> { { "text", "some text in hex".ToHex() } });

            // Assert
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_1);

            // Act
            RpcResponse<string> act_2 = await _wallet.IssueAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, newAddress.Result, new AssetEntity(), 100, 1, 0, new { text = "some text in hex".ToHex() });

            // Assert
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_2);
        }

        [Test]
        public async Task IssueTestGenericallyTypedAsync()
        {
            // Stage
            var newAddress = await _wallet.GetNewAddressAsync(blockchainName: _wallet.RpcOptions.ChainName, nameof(GrantFromTestAsync), "");

            // Stage
            await _wallet.GrantAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000, "", "");

            // Act
            RpcResponse<string> act_1 = await _wallet.IssueAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, newAddress.Result, new { name = UUID.NoHyphens }, 100, 1, 0, new Dictionary<string, string> { { "text", "some text in hex".ToHex() } });

            // Assert
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_1);

            // Act
            RpcResponse<string> act_2 = await _wallet.IssueAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, newAddress.Result, new { name = UUID.NoHyphens }, 100, 1, 0, new { text = "some text in hex".ToHex() });

            // Assert
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_2);
        }

        [Test]
        public async Task IssueTestStringNameAsync()
        {
            // Stage
            var newAddress = await _wallet.GetNewAddressAsync(blockchainName: _wallet.RpcOptions.ChainName, nameof(GrantFromTestAsync), "");

            // Stage
            await _wallet.GrantAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000, "", "");

            // Act
            RpcResponse<string> act_1 = await _wallet.IssueAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, newAddress.Result, UUID.NoHyphens, 100, 1, 0, new Dictionary<string, string> { { "text", "some text in hex".ToHex() } });

            // Assert
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_1);

            // Act
            RpcResponse<string> act_2 = await _wallet.IssueAsync(_wallet.RpcOptions.ChainName, UUID.NoHyphens, newAddress.Result, UUID.NoHyphens, 100, 1, 0, new { text = "some text in hex".ToHex() });

            // Assert
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_2);
        }

        [Test]
        public async Task KeyPoolRefillTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.KeyPoolRefillAsync(_wallet.RpcOptions.ChainName, nameof(KeyPoolRefillTestAsync), 200);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need listaccounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListAccountsTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ListAccountsAsync(_wallet.RpcOptions.ChainName, nameof(ListAccountsTestAsync), 2, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task ListAddressesTestAsync()
        {
            // Act
            RpcResponse<ListAddressesResult[]> actual = await _wallet.ListAddressesAsync(_wallet.RpcOptions.ChainName, nameof(ListAddressesTestAsync), "*", true, 1, 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListAddressesResult[]>>(actual);
        }

        [Test]
        public async Task ListAddressGroupingsTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ListAddressGroupingsAsync(_wallet.RpcOptions.ChainName, nameof(ListAddressGroupingsTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task ListAddressTransactionsTestAsync()
        {
            // Act
            RpcResponse<ListAddressTransactionsResult[]> actual = await _wallet.ListAddressTransactionsAsync(_wallet.RpcOptions.ChainName, nameof(ListAddressTransactionsTestAsync), _wallet.RpcOptions.ChainAdminAddress, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListAddressTransactionsResult[]>>(actual);
        }

        [Test]
        public async Task ListAssetTransactionsTestAsync()
        {
            // Stage
            var issue = await _wallet.IssueAsync(_wallet.RpcOptions.ChainName, nameof(ListAssetTransactionsTestAsync), _wallet.RpcOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, default);

            // Stage
            await _wallet.SubscribeAsync(_wallet.RpcOptions.ChainName, nameof(ListAssetTransactionsTestAsync), issue.Result, false, "");

            // Act
            RpcResponse<ListAssetTransactionsResult[]> actual = await _wallet.ListAssetTransactionsAsync(_wallet.RpcOptions.ChainName, nameof(ListAssetTransactionsTestAsync), issue.Result, true, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListAssetTransactionsResult[]>>(actual);
        }

        [Test]
        public async Task ListLockUnspentTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ListLockUnspentAsync(_wallet.RpcOptions.ChainName, nameof(ListLockUnspentTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need listaccounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListReceivedByAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ListReceivedByAccountAsync(_wallet.RpcOptions.ChainName, nameof(ListReceivedByAccountTestAsync), 2, true, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need listaccounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListReceivedByAddressTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ListReceivedByAddressAsync(_wallet.RpcOptions.ChainName, nameof(ListReceivedByAddressTestAsync), 2, true, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task ListSinceBlockTestAsync()
        {
            // Stage
            var lastBlock = await _blockchain.GetLastBlockInfoAsync(_wallet.RpcOptions.ChainName, nameof(ListSinceBlockTestAsync), 0);

            // Act
            RpcResponse<object> actual = await _wallet.ListSinceBlockAsync(_wallet.RpcOptions.ChainName, nameof(ListSinceBlockTestAsync), lastBlock.Result.Hash, 1, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task ListStreamBlockItemsTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ListStreamBlockItemsAsync(_wallet.RpcOptions.ChainName, nameof(ListStreamBlockItemsTestAsync), "root", "60, 61-65", true, 10, 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task ListStreamItemsTestAsync()
        {
            // Act
            RpcResponse<ListStreamItemsResult[]> actual = await _wallet.ListStreamItemsAsync(_wallet.RpcOptions.ChainName, nameof(ListStreamItemsTestAsync), "root", true, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamItemsResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamKeyItemsTestAsync()
        {
            // Act
            RpcResponse<ListStreamKeyItemsResult[]> actual = await _wallet.ListStreamKeyItemsAsync(_wallet.RpcOptions.ChainName, nameof(ListStreamKeyItemsTestAsync), "root", "some_key", true, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamKeyItemsResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamKeysTestAsync()
        {
            // Act
            RpcResponse<ListStreamKeysResult[]> actual = await _wallet.ListStreamKeysAsync(_wallet.RpcOptions.ChainName, nameof(ListStreamKeysTestAsync), "root", "*", true, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamKeysResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamPublisherItemsTestAsync()
        {
            // Act
            RpcResponse<ListStreamPublisherItemsResult[]> actual = await _wallet.ListStreamPublisherItemsAsync(_wallet.RpcOptions.ChainName, nameof(ListStreamPublisherItemsTestAsync), "root", _wallet.RpcOptions.ChainAdminAddress, true, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamPublisherItemsResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamPublishersTestAsync()
        {
            // Act
            RpcResponse<ListStreamPublishersResult[]> actual = await _wallet.ListStreamPublishersAsync(_wallet.RpcOptions.ChainName, nameof(ListStreamPublishersTestAsync), "root", "*", true, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamPublishersResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamQueryItemsTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.ListStreamQueryItemsAsync(_wallet.RpcOptions.ChainName, nameof(ListStreamQueryItemsTestAsync), "root", new { publisher = _wallet.RpcOptions.ChainAdminAddress }, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task ListStreamTxItemsTestAsync()
        {
            // Stage
            var txid = await _wallet.PublishAsync(_wallet.RpcOptions.ChainName, nameof(ListStreamTxItemsTestAsync), "root", ChainEntity.GetUUID(), "Some Stream Item Data".ToHex(), "");

            // Act
            RpcResponse<object> actual = await _wallet.ListStreamTxItemsAsync(_wallet.RpcOptions.ChainName, nameof(ListStreamTxItemsTestAsync), "root", txid.Result, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Not supported with scalable wallet - if you need listtransactions, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListTransactionsTestAsync()
        {
            // Act
            RpcResponse<ListTransactionsResult[]> actual = await _wallet.ListTransactionsAsync(_wallet.RpcOptions.ChainName, nameof(ListTransactionsTestAsync), "some_account", 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListTransactionsResult[]>>(actual);
        }

        [Test]
        public async Task ListUnspentTestAsync()
        {
            // Act
            RpcResponse<ListUnspentResult[]> actual = await _wallet.ListUnspentAsync(_wallet.RpcOptions.ChainName, nameof(ListUnspentTestAsync), 2, 100, new[] { _wallet.RpcOptions.ChainAdminAddress });

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListUnspentResult[]>>(actual);
        }

        [Test]
        public async Task ListWalletTransactionsTestAsync()
        {
            // Act
            RpcResponse<ListWalletTransactionsResult[]> actual = await _wallet.ListWalletTransactionsAsync(_wallet.RpcOptions.ChainName, nameof(ListWalletTransactionsTestAsync), 10, 0, true, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListWalletTransactionsResult[]>>(actual);
        }

        [Test]
        public async Task LockUnspentTestAsync()
        {
            // Stage
            var asset = new AssetEntity();

            var issue = await _wallet.IssueAsync(
                _wallet.RpcOptions.ChainName, nameof(LockUnspentTestAsync), _wallet.RpcOptions.ChainAdminAddress, asset, 100, 1, 0,
                    new Dictionary<string, string> { { "text", "some text in hex".ToHex() } });

            var unspent = await _wallet.PrepareLockUnspentAsync(_wallet.RpcOptions.ChainName, nameof(LockUnspentTestAsync),
                new Dictionary<string, int> { { asset.Name, 1 } }, false);

            // Act
            RpcResponse<object> actual = await _wallet.LockUnspentAsync(_wallet.RpcOptions.ChainName, nameof(LockUnspentTestAsync),
                false, new Transaction[] { new Transaction { Txid = unspent.Result.Txid, Vout = unspent.Result.Vout } });

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need move, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task MoveTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.MoveAsync(_wallet.RpcOptions.ChainName, nameof(MoveTestAsync), "from_account", "to_account", 0.01, 6, "Testing the Move function");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task PrepareLockUnspentTestAsync()
        {
            // Act
            var asset = new AssetEntity();

            var issue = await _wallet.IssueAsync(
                _wallet.RpcOptions.ChainName, nameof(PrepareLockUnspentTestAsync), _wallet.RpcOptions.ChainAdminAddress, asset, 100, 1, 0,
                    new Dictionary<string, string> { { "text", "some text in hex".ToHex() } });

            var actual = await _wallet.PrepareLockUnspentAsync(_wallet.RpcOptions.ChainName, nameof(PrepareLockUnspentTestAsync),
                new Dictionary<string, int> { { asset.Name, 1 } }, false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentResult>>(actual);
        }

        [Test]
        public async Task PrepareLockUnspentFromTestAsync()
        {
            // Act
            var asset = new AssetEntity();

            var issue = await _wallet.IssueAsync(
                _wallet.RpcOptions.ChainName, nameof(PrepareLockUnspentFromTestAsync), _wallet.RpcOptions.ChainAdminAddress, asset, 100, 1, 0,
                    new Dictionary<string, string> { { "text", "some text in hex".ToHex() } });

            var actual = await _wallet.PrepareLockUnspentFromAsync(_wallet.RpcOptions.ChainName, nameof(PrepareLockUnspentFromTestAsync),
                _wallet.RpcOptions.ChainAdminAddress, new Dictionary<string, int> { { asset.Name, 1 } }, false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentFromResult>>(actual);
        }

        [Test]
        public async Task PublishTestAsync()
        {
            // Act
            RpcResponse<string> actual = await _wallet.PublishAsync(_wallet.RpcOptions.ChainName, nameof(PublishTestAsync), "root", "test_key", "some_data".ToHex(), "offchain");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task PublishFromTestAsync()
        {
            // Act
            RpcResponse<string> actual = await _wallet.PublishFromAsync(_wallet.RpcOptions.ChainName, nameof(PublishFromTestAsync), _wallet.RpcOptions.ChainAdminAddress, "root", "test_key", "some_data".ToHex(), "offchain");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task PublishMultiTestAsync()
        {
            // Act
            RpcResponse<string> actual = await _wallet.PublishMultiAsync(_wallet.RpcOptions.ChainName, nameof(PublishMultiTestAsync), "root", new object[] { new { key = "some_key", data = "some_data".ToHex() } }, "offchain");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task PublishMultiFromTestAsync()
        {
            // Act
            RpcResponse<string> actual = await _wallet.PublishMultiFromAsync(_wallet.RpcOptions.ChainName, nameof(PublishMultiFromTestAsync), _wallet.RpcOptions.ChainAdminAddress, "root", new object[] { new { key = "some_key", data = "some_data".ToHex() } }, "offchain");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test, Ignore("ResendWalletTransaction test is deffered from normal unit testing")]
        public async Task ResendWalletTransactionsTestAsync()
        {
            // Act - ttempt to resend the current wallet's transaction
            RpcResponse<object> actual = await _wallet.ResendWalletTransactionsAsync(_wallet.RpcOptions.ChainName, nameof(ResendWalletTransactionsTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task RevokeTestAsync()
        {
            // Stage - Ask the blockchain network for a new address
            var newAddress = await _wallet.GetNewAddressAsync(_wallet.RpcOptions.ChainName, nameof(RevokeTestAsync), "");

            // Stage - Grant new address receive permissions
            await _wallet.GrantAsync(_wallet.RpcOptions.ChainName, nameof(RevokeTestAsync), newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 0, 10000, "", "");

            // Act - Revoke send permission
            RpcResponse<object> actual = await _wallet.RevokeAsync(_wallet.RpcOptions.ChainName, nameof(RevokeTestAsync), newAddress.Result, "send", 0, "Permissions", "Permissions set");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task RevokeFromTestAsync()
        {
            // Stage - Ask the blockchain network for a new address
            var newAddress = await _wallet.GetNewAddressAsync(blockchainName: _wallet.RpcOptions.ChainName, nameof(RevokeFromTestAsync), "");

            // Stage - Grant new address receive permissions
            await _wallet.GrantAsync(_wallet.RpcOptions.ChainName, nameof(RevokeFromTestAsync), newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 0, 10000, "", "");

            // Act - Revoke send permission
            RpcResponse<object> actual = await _wallet.RevokeFromAsync(_wallet.RpcOptions.ChainName, nameof(RevokeFromTestAsync), _wallet.RpcOptions.ChainAdminAddress, newAddress.Result, "send", 0, "", "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SendTestAsync()
        {
            // Act
            RpcResponse<string> actual = await _wallet.SendAsync(_wallet.RpcOptions.ChainName, nameof(SendTestAsync), _wallet.RpcOptions.ChainAdminAddress, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task SendAssetTestAsync()
        {
            // Stage
            var asset = await _wallet.IssueAsync(_wallet.RpcOptions.ChainName, nameof(SendAssetTestAsync), _wallet.RpcOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, default);

            // Act
            RpcResponse<object> actual = await _wallet.SendAssetAsync(_wallet.RpcOptions.ChainName, nameof(SendAssetTestAsync), _wallet.RpcOptions.ChainAdminAddress, asset.Result, 1, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SendAssetFromTestAsync()
        {
            // Stage
            var newAddress = await _wallet.GetNewAddressAsync(_wallet.RpcOptions.ChainName, nameof(SendAssetFromTestAsync), "");

            // Stage
            await _wallet.GrantAsync(_wallet.RpcOptions.ChainName, nameof(SendAssetFromTestAsync), newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000, "", "");

            // Stage
            var asset = await _wallet.IssueAsync(_wallet.RpcOptions.ChainName, nameof(SendAssetFromTestAsync), _wallet.RpcOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, new Dictionary<string, string> { { "text", "text to hex".ToHex() } });

            // Act
            RpcResponse<object> actual = await _wallet.SendAssetFromAsync(_wallet.RpcOptions.ChainName, nameof(SendAssetFromTestAsync), _wallet.RpcOptions.ChainAdminAddress, newAddress.Result, asset.Result, 1, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SendFromTestAsync()
        {
            // Stage
            var newAddress = await _wallet.GetNewAddressAsync(_wallet.RpcOptions.ChainName, nameof(SendAssetFromTestAsync), "");

            // Stage
            await _wallet.GrantAsync(_wallet.RpcOptions.ChainName, nameof(SendFromTestAsync), newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000, "", "");

            // Act
            RpcResponse<object> actual = await _wallet.SendFromAsync(_wallet.RpcOptions.ChainName, nameof(SendFromTestAsync), _wallet.RpcOptions.ChainAdminAddress, newAddress.Result, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need sendfrom, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task SendFromAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.SendFromAccountAsync(_wallet.RpcOptions.ChainName, nameof(SendFromAccountTestAsync), _wallet.RpcOptions.ChainAdminAddress, _wallet.RpcOptions.ChainAdminAddress, .001, 2, "Comment Text", "Comment_To text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need sendmany, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task SendManyTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.SendManyAsync(_wallet.RpcOptions.ChainName, nameof(SendManyTestAsync), "", new object[] { new Dictionary<string, double> { { _wallet.RpcOptions.ChainAdminAddress, 1 } } }, 2, "Comment text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SendWithDataTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.SendWithDataAsync(_wallet.RpcOptions.ChainName, nameof(SendWithDataTestAsync), _wallet.RpcOptions.ChainAdminAddress, 0, "some data".ToHex());

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SendWithDataFromTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.SendWithDataFromAsync(_wallet.RpcOptions.ChainName, nameof(SendWithDataFromTestAsync), _wallet.RpcOptions.ChainAdminAddress, _wallet.RpcOptions.ChainAdminAddress, 0, "some data".ToHex());

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need move, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task SetAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.SetAccountAsync(_wallet.RpcOptions.ChainName, nameof(SetAccountTestAsync), _wallet.RpcOptions.ChainAdminAddress, "master_account");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Ignored since I do not want to change the TxFee while other transactions are runningh")]
        public async Task SetTxFeeTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.SetTxFeeAsync(_wallet.RpcOptions.ChainName, nameof(SetTxFeeTestAsync), 0.0001);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SignMessageTestAsync()
        {
            // Act
            RpcResponse<string> actual = await _wallet.SignMessageAsync(_wallet.RpcOptions.ChainName, nameof(SignMessageTestAsync), _wallet.RpcOptions.ChainAdminAddress, "Testing the SignMessage function");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task SubscribeTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.SubscribeAsync(_wallet.RpcOptions.ChainName, nameof(SubscribeTestAsync), "root", false, "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task TxOutToBinaryCacheTestAsync()
        {
            // Stage
            var binaryCache = await _utility.CreateBinaryCacheAsync(_wallet.RpcOptions.ChainName, nameof(TxOutToBinaryCacheTestAsync));

            // Stage
            var publish = await _wallet.PublishFromAsync(_wallet.RpcOptions.ChainName, nameof(TxOutToBinaryCacheTestAsync), _wallet.RpcOptions.ChainAdminAddress, "root", ChainEntity.GetUUID(), "A bunch of text data that will be transcribed to this this publish event and this one is async brotato chip".ToHex(), "");

            // Stage
            var transaction = await _wallet.GetAddressTransactionAsync(_wallet.RpcOptions.ChainName, nameof(TxOutToBinaryCacheTestAsync), _wallet.RpcOptions.ChainAdminAddress, publish.Result, true);

            // Act
            RpcResponse<double> actual = await _wallet.TxOutToBinaryCacheAsync(_wallet.RpcOptions.ChainName, nameof(TxOutToBinaryCacheTestAsync), binaryCache.Result, transaction.Result.Txid, transaction.Result.Vout[0].N, 100000, 0);

            // Act
            await _utility.DeleteBinaryCacheAsync(_wallet.RpcOptions.ChainName, nameof(TxOutToBinaryCacheTestAsync), binaryCache.Result);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<double>>(actual);
        }

        [Test]
        public async Task UnsubscribeTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.UnsubscribeAsync(_wallet.RpcOptions.ChainName, nameof(UnsubscribeTestAsync), "root", false);

            // Act
            await _wallet.SubscribeAsync(_wallet.RpcOptions.ChainName, nameof(UnsubscribeTestAsync), "root", false, "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Wallet related tests are ignored while general tests are running")]
        public async Task WalletLockTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.WalletLockAsync(_wallet.RpcOptions.ChainName, nameof(WalletLockTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Wallet related tests are ignored while general tests are running")]
        public async Task WalletPassphraseTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.WalletPassphraseAsync(_wallet.RpcOptions.ChainName, nameof(WalletPassphraseTestAsync), "wallet_passphrase", 10);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Wallet related tests are ignored while general tests are running")]
        public async Task WalletPassphraseChangeTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.WalletPassphraseChangeAsync(_wallet.RpcOptions.ChainName, nameof(WalletPassphraseChangeTestAsync), "old_passphrase", "new_passphrase");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }
    }
}
