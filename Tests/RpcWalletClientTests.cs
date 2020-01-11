using MCWrapper.Data.Models.Wallet;
using MCWrapper.Data.Models.Wallet.CustomModels;
using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.FilterHelpers;
using MCWrapper.RPC.Tests.ServicesPipeline;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class RpcWalletClientTests
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
        private readonly IMultiChainRpcUtility _utility;
        private readonly IMultiChainRpcWallet _wallet;
        private readonly string _chainName;
        private readonly string _address;

        // Use mock startup service container
        private readonly ExplicitStartup _services = new ExplicitStartup();

        // Create a new RpcWalletClientTests instance
        public RpcWalletClientTests()
        {
            _utility = _services.GetRequiredService<IMultiChainRpcUtility>();
            _wallet = _services.GetRequiredService<IMultiChainRpcWallet>();
            _address = _wallet.RpcOptions.ChainAdminAddress;
            _chainName = _wallet.RpcOptions.ChainName;
        }

        [Test]
        public async Task AddMultiSigAddressTestAsync()
        {
            // Act
            var inf = await _wallet.AddMultiSigAddressAsync(1, new[] { _address });

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);

            // Act
            var exp = await _wallet.AddMultiSigAddressAsync(_chainName, UUID.NoHyphens, 1, new[] { _address });

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);
        }

        [Test]
        public async Task AppendRawExchangeTestAsync()
        {
            // Stage
            var infAsset = await _wallet.IssueAsync(_address, new AssetEntity(), 100, 1, 0, null);

            // Act
            var infLocked = await _wallet.PrepareLockUnspentFromAsync(_address, new Dictionary<string, decimal> { { "", 0 }, { infAsset.Result, 10 } }, true);

            // Assert
            Assert.IsNull(infLocked.Error);
            Assert.IsNotNull(infLocked.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentFromResult>>(infLocked);

            // Act
            var infRawExch = await _wallet.CreateRawExchangeAsync(infLocked.Result.Txid, infLocked.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Assert
            Assert.IsNull(infRawExch.Error);
            Assert.IsNotNull(infRawExch.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infRawExch);

            // Act
            var infAppendRaw = await _wallet.AppendRawExchangeAsync(infRawExch.Result, infLocked.Result.Txid, infLocked.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Assert
            Assert.IsNull(infAppendRaw.Error);
            Assert.IsNotNull(infAppendRaw.Result);
            Assert.IsInstanceOf<RpcResponse<AppendRawExchangeResult>>(infAppendRaw);

            // Act
            var infDisable = await _wallet.DisableRawTransactionAsync(infAppendRaw.Result.Hex);

            // Assert
            Assert.IsNull(infDisable.Error);
            Assert.IsNotNull(infDisable.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infDisable);

            // Stage
            var expAsset = await _wallet.IssueAsync(_chainName, UUID.NoHyphens, _address, new AssetEntity(), 100, 1, 0, null);

            // Act
            var expLocked = await _wallet.PrepareLockUnspentFromAsync(_chainName, UUID.NoHyphens, _address, new Dictionary<string, decimal> { { expAsset.Result, 10 } }, true);

            // Assert
            Assert.IsNull(expLocked.Error);
            Assert.IsNotNull(expLocked.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentFromResult>>(expLocked);

            // Act
            var expRawExch = await _wallet.CreateRawExchangeAsync(_chainName, UUID.NoHyphens, expLocked.Result.Txid, expLocked.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Assert
            Assert.IsNull(expRawExch.Error);
            Assert.IsNotNull(expRawExch.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expRawExch);

            // Act
            var expAppendRaw = await _wallet.AppendRawExchangeAsync(_chainName, UUID.NoHyphens, expRawExch.Result, expLocked.Result.Txid, expLocked.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Assert
            Assert.IsNull(expAppendRaw.Error);
            Assert.IsNotNull(expAppendRaw.Result);
            Assert.IsInstanceOf<RpcResponse<AppendRawExchangeResult>>(expAppendRaw);

            // Act
            var expDisable = await _wallet.DisableRawTransactionAsync(_chainName, UUID.NoHyphens, expAppendRaw.Result.Hex);

            // Assert
            Assert.IsNull(expDisable.Error);
            Assert.IsNotNull(expDisable.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expDisable);
        }

        [Test]
        public async Task ApproveFromTestAsync()
        {
            // Stage
            var infFilter = await _wallet.CreateAsync(Entity.TxFilter, StreamFilterEntity.GetUUID(), new { }, JsCode.DummyTxFilterCode);

            // Act
            var infApprove = await _wallet.ApproveFromAsync(_address, infFilter.Result, true);

            // Assert
            Assert.IsNull(infApprove.Error);
            Assert.IsNotNull(infApprove.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infApprove);

            // Stage
            var expFilter = await _wallet.CreateAsync(_chainName, UUID.NoHyphens, Entity.TxFilter, StreamFilterEntity.GetUUID(), new { }, JsCode.DummyTxFilterCode);

            // Act
            var expApprove = await _wallet.ApproveFromAsync(_chainName, UUID.NoHyphens, _address, expFilter.Result, true);

            // Assert
            Assert.IsNull(expApprove.Error);
            Assert.IsNotNull(expApprove.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expApprove);
        }

        [Test, Ignore("BackupWallet test ignored since it halts the blockchain network")]
        public async Task BackupWalletTestAsync()
        {
            // Act
            var infBackup = await _wallet.BackupWalletAsync("backup.dat");

            // Assert
            Assert.IsNull(infBackup.Error);
            Assert.IsNotNull(infBackup.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infBackup);

            // Act
            RpcResponse<object> expBackup = await _wallet.BackupWalletAsync(_chainName, UUID.NoHyphens, "backup.dat");

            // Assert
            Assert.IsNull(expBackup.Error);
            Assert.IsNotNull(expBackup.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expBackup);
        }

        [Test]
        public async Task CombineUnspentTestAsync()
        {
            // Act
            var infCombine = await _wallet.CombineUnspentAsync(_address, 1, 100, 2, 1000, 15);

            // Assert
            Assert.IsNull(infCombine.Error);
            Assert.IsNotNull(infCombine.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infCombine);
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
            var actual = await _wallet.CreateFromAsync(_address, Entity.Stream, StreamEntity.GetUUID(), true, new { });

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
            var actual = await _wallet.CreateRawSendFromAsync(_address, new Dictionary<string, double> { { _address, 0 } }, Array.Empty<object>(), "");

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
            var actual = await _wallet.DumpPrivKeyAsync(_address);

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

        [Test]
        public async Task GrantFromTestAsync()
        {
            // Stage
            var newAddress = await _wallet.GetNewAddressAsync();

            // Act
            RpcResponse<string> actual = await _wallet.GrantFromAsync(_address, newAddress.Result, Permission.Receive, 0, 1, 1000, "", "");

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
            RpcResponse<string> actual = await _wallet.GrantWithDataFromAsync(_address, newAddress.Result, Permission.Receive, "some_data".ToHex(), 0, 1, 1000);

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

        [Test]
        public async Task LockUnspentTestAsync()
        {
            // Stage
            var unspent = await _wallet.PrepareLockUnspentAsync(new Dictionary<string, int> { { "", 0 } }, true);

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
            RpcResponse<PrepareLockUnspentFromResult> actual = await _wallet.PrepareLockUnspentFromAsync(_address, new Dictionary<string, double> { { "", 0 } }, false);

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
            RpcResponse<string> actual = await _wallet.PublishFromAsync(_address, "root", "test_key", "some_data".ToHex(), "offchain");

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
            RpcResponse<string> actual = await _wallet.PublishMultiFromAsync(_address, "root", new object[] { new { key = "some_key", data = "some_data".ToHex() } }, "offchain");

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
            RpcResponse<object> actual = await _wallet.RevokeFromAsync(_address, newAddress.Result, "send", 0, "", "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SendTestAsync()
        {
            // Act
            RpcResponse<string> actual = await _wallet.SendAsync(_address, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task SendAssetTestAsync()
        {
            // Stage
            var asset = await _wallet.IssueAsync(_address, new AssetEntity(), 100, 1, 0, default);

            // Act
            RpcResponse<object> actual = await _wallet.SendAssetAsync(_address, asset.Result, 1, 0, "Comment text", "Comment_To text");

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
            var asset = await _wallet.IssueAsync(_address, new AssetEntity(), 100, 1, 0,
                new Dictionary<string, string> { { "text", "text to hex".ToHex() } });

            // Act
            RpcResponse<object> actual = await _wallet.SendAssetFromAsync(_address, newAddress.Result, asset.Result, 1, 0, "Comment text", "Comment_To text");

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
            RpcResponse<object> actual = await _wallet.SendFromAsync(_address, newAddress.Result, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need sendfrom, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task SendFromAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.SendFromAccountAsync(_address, _address, .001, 2, "Comment Text", "Comment_To text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need sendmany, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task SendManyTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.SendManyAsync("", new object[] { new Dictionary<string, double> { { _address, 1 } } }, 2, "Comment text");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SendWithDataTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.SendWithDataAsync(_address, 0, "some data".ToHex());

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SendWithDataFromTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.SendWithDataFromAsync(_address, _address, 0, "some data".ToHex());

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need move, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task SetAccountTestAsync()
        {
            // Act
            RpcResponse<object> actual = await _wallet.SetAccountAsync(_address, "master_account");

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
            RpcResponse<string> actual = await _wallet.SignMessageAsync(_address, "Testing the SignMessage function");

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
            var publish = await _wallet.PublishFromAsync(_address, "root", ChainEntity.GetUUID(), "A bunch of text data that will be transcribed to this this publish event and this one is async brotato chip".ToHex(), "");

            // Stage
            var transaction = await _wallet.GetAddressTransactionAsync(_address, publish.Result, true);

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
