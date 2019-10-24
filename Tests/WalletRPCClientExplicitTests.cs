using MCWrapper.Data.Models.Wallet;
using MCWrapper.Data.Models.Wallet.CustomModels;
using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients.Blockchain;
using MCWrapper.RPC.Ledger.Clients.Utility;
using MCWrapper.RPC.Ledger.Clients.Wallet;
using MCWrapper.RPC.Tests.ServiceHelpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class WalletRPCClientExplicitTests
    {
        // private fields
        private readonly WalletRpcClient Wallet;
        private readonly UtilityRpcClient Utility;
        private readonly BlockchainRpcClient Blockchain;

        /// <summary>
        /// Create a new WalletServiceTests instance
        /// </summary>
        public WalletRPCClientExplicitTests()
        {
            // instantiate test services provider
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch service from provider
            Wallet = provider.GetService<WalletRpcClient>();
            Utility = provider.GetService<UtilityRpcClient>();
            Blockchain = provider.GetService<BlockchainRpcClient>();
        }

        [Test]
        public async Task AddMultiSigAddressTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.AddMultiSigAddressAsync(
                blockchainName: Wallet.BlockchainOptions.ChainName,
                id: nameof(AddMultiSigAddressTestAsync),
                n_required: 1,
                keys: new[] { Wallet.BlockchainOptions.ChainAdminAddress }, "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task AppendRawExchangeTestAsync()
        {
            // Stage - Issue a new asset to the blockchain node 
            RpcResponse<string> asset = await Wallet.IssueAsync(
                blockchainName: Wallet.BlockchainOptions.ChainName,
                id: nameof(IssueTestAsync),
                to_address: Wallet.BlockchainOptions.ChainAdminAddress,
                asset_params: new AssetEntity(),
                quantity: 100,
                smallest_unit: 1);

            // Act
            RpcResponse<PrepareLockUnspentFromResult> prepareLockUnspent = await Wallet.PrepareLockUnspentFromAsync(
                blockchainName: Wallet.BlockchainOptions.ChainName,
                id: nameof(AppendRawExchangeTestAsync),
                from_address: Wallet.BlockchainOptions.ChainAdminAddress,
                asset_quantities: new Dictionary<string, decimal> { { asset.Result, 10 } },
                _lock: true);

            // Act
            RpcResponse<string> rawExchange = await Wallet.CreateRawExchangeAsync(
                blockchainName: Wallet.BlockchainOptions.ChainName,
                id: nameof(AppendRawExchangeTestAsync),
                txid: prepareLockUnspent.Result.Txid,
                vout: prepareLockUnspent.Result.Vout,
                ask_assets: new Dictionary<string, int> { { "", 0 } });

            // Act
            RpcResponse<AppendRawExchangeResult> appendRaw = await Wallet.AppendRawExchangeAsync(
                blockchainName: Wallet.BlockchainOptions.ChainName,
                id: nameof(AppendRawExchangeTestAsync),
                hex: rawExchange.Result,
                txid: prepareLockUnspent.Result.Txid,
                vout: prepareLockUnspent.Result.Vout,
                ask_assets: new Dictionary<string, int> { { "", 0 } });

            // Act
            RpcResponse<object> disable = await Wallet.DisableRawTransactionAsync(
                blockchainName: Wallet.BlockchainOptions.ChainName,
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
            var filter = await Wallet.CreateAsync(Wallet.BlockchainOptions.ChainName, nameof(ApproveFromTestAsync), Entity.TxFilter, StreamFilterEntity.GetUUID(), new { }, jsCode);

            // Act
            RpcResponse<object> actual = await Wallet.ApproveFromAsync(Wallet.BlockchainOptions.ChainName, nameof(ApproveFromTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, filter.Result, true); // we are going to expect this to fail since there are no upgrades available

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("BackupWallet test ignored since it halts the blockchain network")]
        public async Task BackupWalletTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.BackupWalletAsync(Wallet.BlockchainOptions.ChainName, nameof(BackupWalletTestAsync), "backup.dat");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task CombineUnspentTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.CombineUnspentAsync(Wallet.BlockchainOptions.ChainName, nameof(CombineUnspentTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, 1, 100, 2, 1000, 15);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task CompleteRawExchangeTestAsync()
        {
            // Act
            RpcResponse<PrepareLockUnspentResult> prepareLockUnspent = await Wallet.PrepareLockUnspentAsync(Wallet.BlockchainOptions.ChainName, nameof(CompleteRawExchangeTestAsync), new Dictionary<string, int> { { "", 0 } }, true);

            // Act
            RpcResponse<string> rawExchange = await Wallet.CreateRawExchangeAsync(Wallet.BlockchainOptions.ChainName, nameof(CompleteRawExchangeTestAsync), prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            RpcResponse<AppendRawExchangeResult> appendRaw = await Wallet.AppendRawExchangeAsync(Wallet.BlockchainOptions.ChainName, nameof(CompleteRawExchangeTestAsync), rawExchange.Result, prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            RpcResponse<object> complete = await Wallet.CompleteRawExchangeAsync(Wallet.BlockchainOptions.ChainName, nameof(CompleteRawExchangeTestAsync), appendRaw.Result.Hex, prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } }, "test".ToHex());

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
            RpcResponse<string> actual = await Wallet.CreateFromAsync(Wallet.BlockchainOptions.ChainName, nameof(CreateFromTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, Entity.Stream, StreamEntity.GetUUID(), true, new { });

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

            var issue = await Wallet.IssueAsync(Wallet.BlockchainOptions.ChainAdminAddress, asset, 100, 1, 0, new { text = "Some Text".ToHex() });
            RpcResponse<PrepareLockUnspentResult> prepareLockUnspent = await Wallet.PrepareLockUnspentAsync(Wallet.BlockchainOptions.ChainName, nameof(CreateRawExchangeTestAsync), new Dictionary<string, int> { { asset.Name, 2 } }, true);

            // Act
            RpcResponse<string> rawExchange = await Wallet.CreateRawExchangeAsync(Wallet.BlockchainOptions.ChainName, nameof(CreateRawExchangeTestAsync), prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            RpcResponse<object> disable = await Wallet.DisableRawTransactionAsync(Wallet.BlockchainOptions.ChainName, nameof(CreateRawExchangeTestAsync), rawExchange.Result);

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

            RpcResponse<object> actual = await Wallet.CreateRawSendFromAsync(Wallet.BlockchainOptions.ChainName, nameof(CreateRawSendFromTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, new Dictionary<string, double> { { Wallet.BlockchainOptions.ChainAdminAddress, 0 } }, new object[] { }, "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task CreateTestAsync()
        {
            // Act
            RpcResponse<string> actual = await Wallet.CreateAsync(Wallet.BlockchainOptions.ChainName, nameof(CreateTestAsync), Entity.Stream, StreamEntity.GetUUID(), true, new { });

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

            var issue = await Wallet.IssueAsync(Wallet.BlockchainOptions.ChainAdminAddress, asset, 100, 1);
            RpcResponse<PrepareLockUnspentResult> prepareLockUnspent = await Wallet.PrepareLockUnspentAsync(Wallet.BlockchainOptions.ChainName, nameof(DecodeRawExchangeTestAsync), new Dictionary<string, int> { { asset.Name, 2 } }, true);

            // Act
            RpcResponse<string> rawExchange = await Wallet.CreateRawExchangeAsync(Wallet.BlockchainOptions.ChainName, nameof(DecodeRawExchangeTestAsync), prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            RpcResponse<DecodeRawExchangeResult> decode = await Wallet.DecodeRawExchangeAsync(Wallet.BlockchainOptions.ChainName, nameof(DecodeRawExchangeTestAsync), rawExchange.Result, true);

            // Act
            RpcResponse<object> disable = await Wallet.DisableRawTransactionAsync(Wallet.BlockchainOptions.ChainName, nameof(DecodeRawExchangeTestAsync), rawExchange.Result);

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

            var issue = await Wallet.IssueAsync(Wallet.BlockchainOptions.ChainAdminAddress, asset, 100, 1, 0, new { text = "Some Text".ToHex() });
            RpcResponse<PrepareLockUnspentResult> prepareLockUnspent = await Wallet.PrepareLockUnspentAsync(Wallet.BlockchainOptions.ChainName, nameof(DisableRawTransactionTestAsync), new Dictionary<string, int> { { asset.Name, 2 } }, true);

            // Act
            RpcResponse<string> rawExchange = await Wallet.CreateRawExchangeAsync(Wallet.BlockchainOptions.ChainName, nameof(DisableRawTransactionTestAsync), prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            RpcResponse<object> disable = await Wallet.DisableRawTransactionAsync(Wallet.BlockchainOptions.ChainName, nameof(DisableRawTransactionTestAsync), rawExchange.Result);

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
            RpcResponse<object> actual = await Wallet.DumpPrivKeyAsync(Wallet.BlockchainOptions.ChainName, nameof(DumpPrivKeyTestAsync), Wallet.BlockchainOptions.ChainAdminAddress);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Dumping the wallet seems to slow down the network. Test is passing and ignored.")]
        public async Task DumpWalletTestAync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.DumpWalletAsync(Wallet.BlockchainOptions.ChainName, nameof(DumpWalletTestAync), "test_async");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Test is implemented and ignored since I don't want to encrypt my wallet in staging")]
        public async Task EncryptWalletTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.EncryptWalletAsync(Wallet.BlockchainOptions.ChainName, nameof(EncryptWalletTestAsync), "some_password");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetAccountAddressTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.GetAccountAddressAsync(Wallet.BlockchainOptions.ChainName, nameof(GetAccountAddressTestAsync), "some_account_name");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.GetAccountAsync(Wallet.BlockchainOptions.ChainName, nameof(GetAccountTestAsync), Wallet.BlockchainOptions.ChainAdminAddress);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetAddressBalancesTestAsync()
        {
            // Act
            RpcResponse<GetAddressBalancesResult[]> actual = await Wallet.GetAddressBalancesAsync(Wallet.BlockchainOptions.ChainName, nameof(GetAddressBalancesTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, 1, false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAddressBalancesResult[]>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetAddressesByAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.GetAddressesByAccountAsync(Wallet.BlockchainOptions.ChainName, nameof(GetAddressesByAccountTestAsync), "some_account_name");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetAddressesTestAsync()
        {
            // Act
            RpcResponse<GetAddressesResult[]> actual = await Wallet.GetAddressesAsync(Wallet.BlockchainOptions.ChainName, nameof(GetAddressesTestAsync), true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAddressesResult[]>>(actual);
        }

        [Test]
        public async Task GetAddressTransactionTestAsync()
        {
            // Stage
            var transaction = await Wallet.IssueAsync(Wallet.BlockchainOptions.ChainName, nameof(GetAddressTransactionTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, new { });

            // Act
            RpcResponse<GetAddressTransactionResult> actual = await Wallet.GetAddressTransactionAsync(Wallet.BlockchainOptions.ChainName, nameof(GetAddressTransactionTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, transaction.Result, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAddressTransactionResult>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetAssetBalancesTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.GetAssetBalancesAsync(Wallet.BlockchainOptions.ChainName, nameof(GetAssetBalancesTestAsync), "some_account_name", 2, true, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetAssetTransactionTestAsync()
        {
            // Stage
            var asset = await Wallet.IssueAsync(Wallet.BlockchainOptions.ChainName, nameof(GetAssetTransactionTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, new { });

            // Stage
            await Wallet.SubscribeAsync(Wallet.BlockchainOptions.ChainName, nameof(GetAssetTransactionTestAsync), asset.Result, false, "");

            // Act
            RpcResponse<GetAssetTransactionResult> actual = await Wallet.GetAssetTransactionAsync(Wallet.BlockchainOptions.ChainName, nameof(GetAssetTransactionTestAsync), asset.Result, asset.Result, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAssetTransactionResult>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetBalanceTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.GetBalanceAsync(Wallet.BlockchainOptions.ChainName, nameof(GetBalanceTestAsync), "", 1, false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetMultiBalancesTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.GetMultiBalancesAsync(Wallet.BlockchainOptions.ChainName, nameof(GetMultiBalancesTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, null, 1, true, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetNewAddressTestAsync()
        {
            // Act
            RpcResponse<string> actual = await Wallet.GetNewAddressAsync("");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GetRawChangeAddressTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.GetRawChangeAddressAsync(Wallet.BlockchainOptions.ChainName, nameof(GetRawChangeAddressTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetReceivedByAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.GetReceivedByAccountAsync(Wallet.BlockchainOptions.ChainName, nameof(GetReceivedByAccountTestAsync), "some_account_name", 2);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetReceivedByAddressTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.GetReceivedByAddressAsync(Wallet.BlockchainOptions.ChainName, nameof(GetReceivedByAddressTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, 2);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetStreamItemTestAsync()
        {
            // Stage
            var publish = await Wallet.PublishFromAsync(Wallet.BlockchainOptions.ChainName, nameof(GetStreamItemTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, "root", ChainEntity.GetUUID(), "Stream item data".ToHex(), "");

            // Act
            RpcResponse<GetStreamItemResult> actual = await Wallet.GetStreamItemAsync(Wallet.BlockchainOptions.ChainName, nameof(GetStreamItemTestAsync), "root", publish.Result, true);

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
            await Wallet.PublishFromAsync(Wallet.BlockchainOptions.ChainName, nameof(GetStreamKeySummaryTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, "root", streamKey, "Stream item data".ToHex(), "");

            // Act
            RpcResponse<object> actual = await Wallet.GetStreamKeySummaryAsync(Wallet.BlockchainOptions.ChainName, nameof(GetStreamKeySummaryTestAsync), "root", streamKey, "jsonobjectmerge,ignore,recursive");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetStreamPublisherSummaryTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.GetStreamPublisherSummaryAsync(Wallet.BlockchainOptions.ChainName, nameof(GetStreamPublisherSummaryTestAsync), "root", Wallet.BlockchainOptions.ChainAdminAddress, "jsonobjectmerge,ignore,recursive");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetTotalBalancesTestAsync()
        {
            // Act
            RpcResponse<GetTotalBalancesResult[]> actual = await Wallet.GetTotalBalancesAsync(Wallet.BlockchainOptions.ChainName, nameof(GetTotalBalancesTestAsync), 1, true, false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetTotalBalancesResult[]>>(actual);
        }

        [Test]
        public async Task GetTransactionTestAsync()
        {
            // Stage
            var txid = await Wallet.IssueFromAsync(Wallet.BlockchainOptions.ChainName, nameof(GetTransactionTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, Wallet.BlockchainOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, new { description = "Some Description" });

            // Act
            RpcResponse<GetTransactionResult> actual = await Wallet.GetTransactionAsync(Wallet.BlockchainOptions.ChainName, nameof(GetTransactionTestAsync), txid.Result, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetTransactionResult>>(actual);
        }

        [Test]
        public async Task GetTxOutDataTestAsync()
        {
            // Stage
            var publish = await Wallet.PublishFromAsync(Wallet.BlockchainOptions.ChainName, nameof(GetTxOutDataTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, "root", ChainEntity.GetUUID(), "Stream item data".ToHex(), "");

            // Stage
            var transaction = await Wallet.GetTransactionAsync(Wallet.BlockchainOptions.ChainName, nameof(GetTxOutDataTestAsync), publish.Result, true);

            // Act
            RpcResponse<object> actual = await Wallet.GetTxOutDataAsync(Wallet.BlockchainOptions.ChainName, nameof(GetTxOutDataTestAsync), transaction.Result.Txid, 0, 10, 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetUnconfirmedBalanceTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.GetUnconfirmedBalanceAsync(Wallet.BlockchainOptions.ChainName, nameof(GetUnconfirmedBalanceTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetWalletInfoTestAsync()
        {
            // Act
            RpcResponse<GetWalletInfoResult> actual = await Wallet.GetWalletInfoAsync(Wallet.BlockchainOptions.ChainName, nameof(GetWalletInfoTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetWalletInfoResult>>(actual);
        }

        [Test]
        public async Task GetWalletTransactionTestAsync()
        {
            // Stage
            var publish = await Wallet.PublishFromAsync(Wallet.BlockchainOptions.ChainName, nameof(GetWalletTransactionTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, "root", ChainEntity.GetUUID(), "Stream item data".ToHex(), "");

            // Act
            RpcResponse<GetWalletTransactionResult> actual = await Wallet.GetWalletTransactionAsync(Wallet.BlockchainOptions.ChainName, nameof(GetWalletTransactionTestAsync), publish.Result, true, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetWalletTransactionResult>>(actual);
        }

        [Test]
        public async Task GrantFromTestAsync()
        {
            // Stage
            var newAddress = await Wallet.GetNewAddressAsync(blockchainName: Wallet.BlockchainOptions.ChainName, nameof(GrantFromTestAsync), "");

            // Act
            RpcResponse<object> actual = await Wallet.GrantFromAsync(Wallet.BlockchainOptions.ChainName, nameof(GrantFromTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, newAddress.Result, Permission.Receive, 0, 1, 1000, "", "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GrantTestAsync()
        {
            // Stage
            var newAddress = await Wallet.GetNewAddressAsync(blockchainName: Wallet.BlockchainOptions.ChainName, nameof(GrantFromTestAsync), "");

            // Act
            RpcResponse<object> actual = await Wallet.GrantAsync(Wallet.BlockchainOptions.ChainName, nameof(GrantTestAsync), newAddress.Result, Permission.Receive, 0, 1, 1000, "", "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GrantWithDataFromTestAsync()
        {
            // Stage
            var newAddress = await Wallet.GetNewAddressAsync(blockchainName: Wallet.BlockchainOptions.ChainName, nameof(GrantFromTestAsync), "");

            // Act
            RpcResponse<object> actual = await Wallet.GrantWithDataFromAsync(Wallet.BlockchainOptions.ChainName, nameof(GrantWithDataFromTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, newAddress.Result, Permission.Receive, "some_data".ToHex(), 0, 1, 1000);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GrantWithDataTestAsync()
        {
            // Stage
            var newAddress = await Wallet.GetNewAddressAsync(blockchainName: Wallet.BlockchainOptions.ChainName, nameof(GrantFromTestAsync), "");

            // Act
            RpcResponse<object> actual = await Wallet.GrantWithDataAsync(Wallet.BlockchainOptions.ChainName, nameof(GrantWithDataTestAsync), newAddress.Result, Permission.Receive, "some_data".ToHex(), 0, 1, 1000);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("I don't want to import any addresses during unit testing")]
        public async Task ImportAddressTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.ImportAddressAsync(Wallet.BlockchainOptions.ChainName, nameof(ImportAddressTestAsync), "some_external_address", "some_label", false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("I don't want to import any private keys during unit testing")]
        public async Task ImportPrivKeyTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.ImportPrivKeyAsync(Wallet.BlockchainOptions.ChainName, nameof(ImportPrivKeyTestAsync), "some_external_private_key", "some_label", false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Tests impacting the current wallet are ignore while general tests are running")]
        public async Task ImportWalletTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.ImportWalletAsync(Wallet.BlockchainOptions.ChainName, nameof(ImportWalletTestAsync), "test", false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        public async Task IssueFromTestAsync()
        {
            // Act
            RpcResponse<string> actual = await Wallet.IssueFromAsync(Wallet.BlockchainOptions.ChainName, nameof(IssueFromTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, Wallet.BlockchainOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0.1m, new { });

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task IssueMoreFromTestAsync()
        {
            // Stage
            var issue = await Wallet.IssueFromAsync(Wallet.BlockchainOptions.ChainName, nameof(IssueMoreFromTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, Wallet.BlockchainOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, new { });

            // Act
            RpcResponse<object> actual = await Wallet.IssueMoreFromAsync(Wallet.BlockchainOptions.ChainName, nameof(IssueMoreFromTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, Wallet.BlockchainOptions.ChainAdminAddress, issue.Result.ToString(), 100, 0, new { });

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task IssueMoreTestAsync()
        {
            // Stage
            var issue = await Wallet.IssueAsync(Wallet.BlockchainOptions.ChainName, nameof(IssueMoreTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, new { });

            // Act
            RpcResponse<object> actual = await Wallet.IssueMoreAsync(Wallet.BlockchainOptions.ChainName, nameof(IssueMoreTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, issue.Result.ToString(), 100, 0, new { });

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task IssueTestAsync()
        {
            // Stage
            var newAddress = await Wallet.GetNewAddressAsync(blockchainName: Wallet.BlockchainOptions.ChainName, nameof(GrantFromTestAsync), "");

            // Stage
            await Wallet.GrantAsync(Wallet.BlockchainOptions.ChainName, nameof(IssueTestAsync), newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000, "", "");

            // Act
            RpcResponse<string> actual = await Wallet.IssueAsync(Wallet.BlockchainOptions.ChainName, nameof(IssueTestAsync), newAddress.Result, new AssetEntity(), 100, 1, 0, new { text = "some text in hex".ToHex() });

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task KeyPoolRefillTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.KeyPoolRefillAsync(Wallet.BlockchainOptions.ChainName, nameof(KeyPoolRefillTestAsync), 200);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need listaccounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListAccountsTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.ListAccountsAsync(Wallet.BlockchainOptions.ChainName, nameof(ListAccountsTestAsync), 2, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task ListAddressesTestAsync()
        {
            // Act
            RpcResponse<ListAddressesResult[]> actual = await Wallet.ListAddressesAsync(Wallet.BlockchainOptions.ChainName, nameof(ListAddressesTestAsync), "*", true, 1, 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListAddressesResult[]>>(actual);
        }

        [Test]
        public async Task ListAddressGroupingsTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.ListAddressGroupingsAsync(Wallet.BlockchainOptions.ChainName, nameof(ListAddressGroupingsTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task ListAddressTransactionsTestAsync()
        {
            // Act
            RpcResponse<ListAddressTransactionsResult[]> actual = await Wallet.ListAddressTransactionsAsync(Wallet.BlockchainOptions.ChainName, nameof(ListAddressTransactionsTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListAddressTransactionsResult[]>>(actual);
        }

        [Test]
        public async Task ListAssetTransactionsTestAsync()
        {
            // Stage
            var issue = await Wallet.IssueAsync(Wallet.BlockchainOptions.ChainName, nameof(ListAssetTransactionsTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, new { });

            // Stage
            await Wallet.SubscribeAsync(Wallet.BlockchainOptions.ChainName, nameof(ListAssetTransactionsTestAsync), issue.Result, false, "");

            // Act
            RpcResponse<ListAssetTransactionsResult[]> actual = await Wallet.ListAssetTransactionsAsync(Wallet.BlockchainOptions.ChainName, nameof(ListAssetTransactionsTestAsync), issue.Result, true, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListAssetTransactionsResult[]>>(actual);
        }

        [Test]
        public async Task ListLockUnspentTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.ListLockUnspentAsync(Wallet.BlockchainOptions.ChainName, nameof(ListLockUnspentTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need listaccounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListReceivedByAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.ListReceivedByAccountAsync(Wallet.BlockchainOptions.ChainName, nameof(ListReceivedByAccountTestAsync), 2, true, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need listaccounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListReceivedByAddressTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.ListReceivedByAddressAsync(Wallet.BlockchainOptions.ChainName, nameof(ListReceivedByAddressTestAsync), 2, true, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task ListSinceBlockTestAsync()
        {
            // Stage
            var lastBlock = await Blockchain.GetLastBlockInfoAsync(Wallet.BlockchainOptions.ChainName, nameof(ListSinceBlockTestAsync), 0);

            // Act
            RpcResponse<object> actual = await Wallet.ListSinceBlockAsync(Wallet.BlockchainOptions.ChainName, nameof(ListSinceBlockTestAsync), lastBlock.Result.Hash, 1, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task ListStreamBlockItemsTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.ListStreamBlockItemsAsync(Wallet.BlockchainOptions.ChainName, nameof(ListStreamBlockItemsTestAsync), "root", "60, 61-65", true, 10, 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task ListStreamItemsTestAsync()
        {
            // Act
            RpcResponse<ListStreamItemsResult[]> actual = await Wallet.ListStreamItemsAsync(Wallet.BlockchainOptions.ChainName, nameof(ListStreamItemsTestAsync), "root", true, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamItemsResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamKeyItemsTestAsync()
        {
            // Act
            RpcResponse<ListStreamKeyItemsResult[]> actual = await Wallet.ListStreamKeyItemsAsync(Wallet.BlockchainOptions.ChainName, nameof(ListStreamKeyItemsTestAsync), "root", "some_key", true, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamKeyItemsResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamKeysTestAsync()
        {
            // Act
            RpcResponse<ListStreamKeysResult[]> actual = await Wallet.ListStreamKeysAsync(Wallet.BlockchainOptions.ChainName, nameof(ListStreamKeysTestAsync), "root", "*", true, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamKeysResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamPublisherItemsTestAsync()
        {
            // Act
            RpcResponse<ListStreamPublisherItemsResult[]> actual = await Wallet.ListStreamPublisherItemsAsync(Wallet.BlockchainOptions.ChainName, nameof(ListStreamPublisherItemsTestAsync), "root", Wallet.BlockchainOptions.ChainAdminAddress, true, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamPublisherItemsResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamPublishersTestAsync()
        {
            // Act
            RpcResponse<ListStreamPublishersResult[]> actual = await Wallet.ListStreamPublishersAsync(Wallet.BlockchainOptions.ChainName, nameof(ListStreamPublishersTestAsync), "root", "*", true, 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamPublishersResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamQueryItemsTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.ListStreamQueryItemsAsync(Wallet.BlockchainOptions.ChainName, nameof(ListStreamQueryItemsTestAsync), "root", new { publisher = Wallet.BlockchainOptions.ChainAdminAddress }, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task ListStreamTxItemsTestAsync()
        {
            // Stage
            var txid = await Wallet.PublishAsync(Wallet.BlockchainOptions.ChainName, nameof(ListStreamTxItemsTestAsync), "root", ChainEntity.GetUUID(), "Some Stream Item Data".ToHex(), "");

            // Act
            RpcResponse<object> actual = await Wallet.ListStreamTxItemsAsync(Wallet.BlockchainOptions.ChainName, nameof(ListStreamTxItemsTestAsync), "root", txid.Result, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Not supported with scalable wallet - if you need listtransactions, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListTransactionsTestAsync()
        {
            // Act
            RpcResponse<ListTransactionsResult[]> actual = await Wallet.ListTransactionsAsync(Wallet.BlockchainOptions.ChainName, nameof(ListTransactionsTestAsync), "some_account", 10, 0, true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListTransactionsResult[]>>(actual);
        }

        [Test]
        public async Task ListUnspentTestAsync()
        {
            // Act
            RpcResponse<ListUnspentResult[]> actual = await Wallet.ListUnspentAsync(Wallet.BlockchainOptions.ChainName, nameof(ListUnspentTestAsync), 2, 100, new[] { Wallet.BlockchainOptions.ChainAdminAddress });

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListUnspentResult[]>>(actual);
        }

        [Test]
        public async Task ListWalletTransactionsTestAsync()
        {
            // Act
            RpcResponse<ListWalletTransactionsResult[]> actual = await Wallet.ListWalletTransactionsAsync(Wallet.BlockchainOptions.ChainName, nameof(ListWalletTransactionsTestAsync), 10, 0, true, true);

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

            var issue = await Wallet.IssueAsync(
                Wallet.BlockchainOptions.ChainName, nameof(LockUnspentTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, asset, 100, 1, 0,
                new { text = "some text in hex".ToHex() });

            var unspent = await Wallet.PrepareLockUnspentAsync(Wallet.BlockchainOptions.ChainName, nameof(LockUnspentTestAsync),
                new Dictionary<string, int> { { asset.Name, 1 } }, false);

            // Act
            RpcResponse<object> actual = await Wallet.LockUnspentAsync(Wallet.BlockchainOptions.ChainName, nameof(LockUnspentTestAsync),
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
            RpcResponse<object> actual = await Wallet.MoveAsync(Wallet.BlockchainOptions.ChainName, nameof(MoveTestAsync), "from_account", "to_account", 0.01, 6, "Testing the Move function");

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

            var issue = await Wallet.IssueAsync(
                Wallet.BlockchainOptions.ChainName, nameof(PrepareLockUnspentTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, asset, 100, 1, 0,
                new { text = "some text in hex".ToHex() });

            var actual = await Wallet.PrepareLockUnspentAsync(Wallet.BlockchainOptions.ChainName, nameof(PrepareLockUnspentTestAsync),
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

            var issue = await Wallet.IssueAsync(
                Wallet.BlockchainOptions.ChainName, nameof(PrepareLockUnspentFromTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, asset, 100, 1, 0,
                new { text = "some text in hex".ToHex() });

            var actual = await Wallet.PrepareLockUnspentFromAsync(Wallet.BlockchainOptions.ChainName, nameof(PrepareLockUnspentFromTestAsync),
                Wallet.BlockchainOptions.ChainAdminAddress, new Dictionary<string, int> { { asset.Name, 1 } }, false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentFromResult>>(actual);
        }

        [Test]
        public async Task PublishTestAsync()
        {
            // Act
            RpcResponse<string> actual = await Wallet.PublishAsync(Wallet.BlockchainOptions.ChainName, nameof(PublishTestAsync), "root", "test_key", "some_data".ToHex(), "offchain");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task PublishFromTestAsync()
        {
            // Act
            RpcResponse<string> actual = await Wallet.PublishFromAsync(Wallet.BlockchainOptions.ChainName, nameof(PublishFromTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, "root", "test_key", "some_data".ToHex(), "offchain");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task PublishMultiTestAsync()
        {
            // Act
            RpcResponse<string> actual = await Wallet.PublishMultiAsync(Wallet.BlockchainOptions.ChainName, nameof(PublishMultiTestAsync), "root", new object[] { new { key = "some_key", data = "some_data".ToHex() } }, "offchain");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task PublishMultiFromTestAsync()
        {
            // Act
            RpcResponse<string> actual = await Wallet.PublishMultiFromAsync(Wallet.BlockchainOptions.ChainName, nameof(PublishMultiFromTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, "root", new object[] { new { key = "some_key", data = "some_data".ToHex() } }, "offchain");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test, Ignore("ResendWalletTransaction test is deffered from normal unit testing")]
        public async Task ResendWalletTransactionsTestAsync()
        {
            // Act - ttempt to resend the current wallet's transaction
            RpcResponse<object> actual = await Wallet.ResendWalletTransactionsAsync(Wallet.BlockchainOptions.ChainName, nameof(ResendWalletTransactionsTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task RevokeTestAsync()
        {
            // Stage - Ask the blockchain network for a new address
            var newAddress = await Wallet.GetNewAddressAsync(Wallet.BlockchainOptions.ChainName, nameof(RevokeTestAsync), "");

            // Stage - Grant new address receive permissions
            await Wallet.GrantAsync(Wallet.BlockchainOptions.ChainName, nameof(RevokeTestAsync), newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 0, 10000, "", "");

            // Act - Revoke send permission
            RpcResponse<object> actual = await Wallet.RevokeAsync(Wallet.BlockchainOptions.ChainName, nameof(RevokeTestAsync), newAddress.Result, "send", 0, "Permissions", "Permissions set");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task RevokeFromTestAsync()
        {
            // Stage - Ask the blockchain network for a new address
            var newAddress = await Wallet.GetNewAddressAsync(blockchainName: Wallet.BlockchainOptions.ChainName, nameof(RevokeFromTestAsync), "");

            // Stage - Grant new address receive permissions
            await Wallet.GrantAsync(Wallet.BlockchainOptions.ChainName, nameof(RevokeFromTestAsync), newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 0, 10000, "", "");

            // Act - Revoke send permission
            RpcResponse<object> actual = await Wallet.RevokeFromAsync(Wallet.BlockchainOptions.ChainName, nameof(RevokeFromTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, newAddress.Result, "send", 0, "", "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SendTestAsync()
        {
            // Act
            RpcResponse<string> actual = await Wallet.SendAsync(Wallet.BlockchainOptions.ChainName, nameof(SendTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task SendAssetTestAsync()
        {
            // Stage
            var asset = await Wallet.IssueAsync(Wallet.BlockchainOptions.ChainName, nameof(SendAssetTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, new { });

            // Act
            RpcResponse<object> actual = await Wallet.SendAssetAsync(Wallet.BlockchainOptions.ChainName, nameof(SendAssetTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, asset.Result, 1, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SendAssetFromTestAsync()
        {
            // Stage
            var newAddress = await Wallet.GetNewAddressAsync(Wallet.BlockchainOptions.ChainName, nameof(SendAssetFromTestAsync), "");

            // Stage
            await Wallet.GrantAsync(Wallet.BlockchainOptions.ChainName, nameof(SendAssetFromTestAsync), newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000, "", "");

            // Stage
            var asset = await Wallet.IssueAsync(Wallet.BlockchainOptions.ChainName, nameof(SendAssetFromTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, new AssetEntity(), 100, 1, 0, new { text = "text to hex".ToHex() });

            // Act
            RpcResponse<object> actual = await Wallet.SendAssetFromAsync(Wallet.BlockchainOptions.ChainName, nameof(SendAssetFromTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, newAddress.Result, asset.Result, 1, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SendFromTestAsync()
        {
            // Stage
            var newAddress = await Wallet.GetNewAddressAsync(Wallet.BlockchainOptions.ChainName, nameof(SendAssetFromTestAsync), "");

            // Stage
            await Wallet.GrantAsync(Wallet.BlockchainOptions.ChainName, nameof(SendFromTestAsync), newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000, "", "");

            // Act
            RpcResponse<object> actual = await Wallet.SendFromAsync(Wallet.BlockchainOptions.ChainName, nameof(SendFromTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, newAddress.Result, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need sendfrom, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task SendFromAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.SendFromAccountAsync(Wallet.BlockchainOptions.ChainName, nameof(SendFromAccountTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, Wallet.BlockchainOptions.ChainAdminAddress, .001, 2, "Comment Text", "Comment_To text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need sendmany, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task SendManyTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.SendManyAsync(Wallet.BlockchainOptions.ChainName, nameof(SendManyTestAsync), "", new object[] { new Dictionary<string, double> { { Wallet.BlockchainOptions.ChainAdminAddress, 1 } } }, 2, "Comment text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SendWithDataTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.SendWithDataAsync(Wallet.BlockchainOptions.ChainName, nameof(SendWithDataTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, 0, "some data".ToHex());

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SendWithDataFromTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.SendWithDataFromAsync(Wallet.BlockchainOptions.ChainName, nameof(SendWithDataFromTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, Wallet.BlockchainOptions.ChainAdminAddress, 0, "some data".ToHex());

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need move, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task SetAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.SetAccountAsync(Wallet.BlockchainOptions.ChainName, nameof(SetAccountTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, "master_account");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Ignored since I do not want to change the TxFee while other transactions are runningh")]
        public async Task SetTxFeeTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.SetTxFeeAsync(Wallet.BlockchainOptions.ChainName, nameof(SetTxFeeTestAsync), 0.0001);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SignMessageTestAsync()
        {
            // Act
            RpcResponse<string> actual = await Wallet.SignMessageAsync(Wallet.BlockchainOptions.ChainName, nameof(SignMessageTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, "Testing the SignMessage function");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task SubscribeTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.SubscribeAsync(Wallet.BlockchainOptions.ChainName, nameof(SubscribeTestAsync), "root", false, "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task TxOutToBinaryCacheTestAsync()
        {
            // Stage
            var binaryCache = await Utility.CreateBinaryCacheAsync(Wallet.BlockchainOptions.ChainName, nameof(TxOutToBinaryCacheTestAsync));

            // Stage
            var publish = await Wallet.PublishFromAsync(Wallet.BlockchainOptions.ChainName, nameof(TxOutToBinaryCacheTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, "root", ChainEntity.GetUUID(), "A bunch of text data that will be transcribed to this this publish event and this one is async brotato chip".ToHex(), "");

            // Stage
            var transaction = await Wallet.GetAddressTransactionAsync(Wallet.BlockchainOptions.ChainName, nameof(TxOutToBinaryCacheTestAsync), Wallet.BlockchainOptions.ChainAdminAddress, publish.Result, true);

            // Act
            RpcResponse<double> actual = await Wallet.TxOutToBinaryCacheAsync(Wallet.BlockchainOptions.ChainName, nameof(TxOutToBinaryCacheTestAsync), binaryCache.Result, transaction.Result.Txid, transaction.Result.Vout[0].N, 100000, 0);

            // Act
            await Utility.DeleteBinaryCacheAsync(Wallet.BlockchainOptions.ChainName, nameof(TxOutToBinaryCacheTestAsync), binaryCache.Result);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<double>>(actual);
        }

        [Test]
        public async Task UnsubscribeTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.UnsubscribeAsync(Wallet.BlockchainOptions.ChainName, nameof(UnsubscribeTestAsync), "root", false);

            // Act
            await Wallet.SubscribeAsync(Wallet.BlockchainOptions.ChainName, nameof(UnsubscribeTestAsync), "root", false, "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Wallet related tests are ignored while general tests are running")]
        public async Task WalletLockTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.WalletLockAsync(Wallet.BlockchainOptions.ChainName, nameof(WalletLockTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Wallet related tests are ignored while general tests are running")]
        public async Task WalletPassphraseTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.WalletPassphraseAsync(Wallet.BlockchainOptions.ChainName, nameof(WalletPassphraseTestAsync), "wallet_passphrase", 10);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Wallet related tests are ignored while general tests are running")]
        public async Task WalletPassphraseChangeTestAsync()
        {
            // Act
            RpcResponse<object> actual = await Wallet.WalletPassphraseChangeAsync(Wallet.BlockchainOptions.ChainName, nameof(WalletPassphraseChangeTestAsync), "old_passphrase", "new_passphrase");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }
    }
}
