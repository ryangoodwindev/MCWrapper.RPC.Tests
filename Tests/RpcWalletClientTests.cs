using MCWrapper.Data.Models.Wallet;
using MCWrapper.Data.Models.Wallet.CustomModels;
using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Test.FilterHelpers;
using MCWrapper.RPC.Test.ServicesPipeline;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Test.Wallet
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
            /*
               Explicit blockchain name test
            */

            // Act
            var exp = await _wallet.AddMultiSigAddressAsync(_chainName, UUID.NoHyphens, 1, new[] { _address });

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.AddMultiSigAddressAsync(1, new[] { _address });

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test]
        public async Task AppendRawExchangeTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Stage
            var expAsset = await _wallet.IssueAsync(_chainName, UUID.NoHyphens, _address, new AssetEntity(), 100, 1, 0, null);
            var expLocked = await _wallet.PrepareLockUnspentFromAsync(_chainName, UUID.NoHyphens, _address, new Dictionary<string, decimal> { { expAsset.Result, 10 } }, true);
            var expRawExch = await _wallet.CreateRawExchangeAsync(_chainName, UUID.NoHyphens, expLocked.Result.Txid, expLocked.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            var expAppendRaw = await _wallet.AppendRawExchangeAsync(_chainName, UUID.NoHyphens, expRawExch.Result, expLocked.Result.Txid, expLocked.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Assert
            Assert.IsNull(expAppendRaw.Error);
            Assert.IsNotNull(expAppendRaw.Result);
            Assert.IsInstanceOf<RpcResponse<AppendRawExchangeResult>>(expAppendRaw);

            // Clean-up
            await _wallet.DisableRawTransactionAsync(_chainName, UUID.NoHyphens, expAppendRaw.Result.Hex);

            /*
               Inferred blockchain name test
            */

            // Stage
            var infAsset = await _wallet.IssueAsync(_address, new AssetEntity(), 100, 1, 0, null);
            var infLocked = await _wallet.PrepareLockUnspentFromAsync(_address, new Dictionary<string, decimal> { { "", 0 }, { infAsset.Result, 10 } }, true);
            var infRawExch = await _wallet.CreateRawExchangeAsync(infLocked.Result.Txid, infLocked.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            var infAppendRaw = await _wallet.AppendRawExchangeAsync(infRawExch.Result, infLocked.Result.Txid, infLocked.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Assert
            Assert.IsNull(infAppendRaw.Error);
            Assert.IsNotNull(infAppendRaw.Result);
            Assert.IsInstanceOf<RpcResponse<AppendRawExchangeResult>>(infAppendRaw);

            // Clean-up
            await _wallet.DisableRawTransactionAsync(infAppendRaw.Result.Hex);
        }

        [Test]
        public async Task ApproveFromTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Stage
            var expFilter = await _wallet.CreateAsync(_chainName, UUID.NoHyphens, Entity.TxFilter, StreamFilterEntity.GetUUID(), new { }, JsCode.DummyTxFilterCode);

            // Act
            var expApprove = await _wallet.ApproveFromAsync(_chainName, UUID.NoHyphens, _address, expFilter.Result, true);

            // Assert
            Assert.IsNull(expApprove.Error);
            Assert.IsNotNull(expApprove.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expApprove);

            /*
               Inferred blockchain name test
            */

            // Stage
            var infFilter = await _wallet.CreateAsync(Entity.TxFilter, StreamFilterEntity.GetUUID(), new { }, JsCode.DummyTxFilterCode);

            // Act
            var infApprove = await _wallet.ApproveFromAsync(_address, infFilter.Result, true);

            // Assert
            Assert.IsNull(infApprove.Error);
            Assert.IsNotNull(infApprove.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infApprove);
        }

        [Test]
        public async Task CombineUnspentTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expCombine = await _wallet.CombineUnspentAsync(_chainName, UUID.NoHyphens, _address, 1, 100, 2, 1000, 15);

            // Assert
            Assert.IsNull(expCombine.Error);
            Assert.IsNotNull(expCombine.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expCombine);

            /*
               Inferred blockchain name test
            */

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
            /*
               Explicit blockchain name test
            */

            // Stage
            var expPrepareLockUnspent = await _wallet.PrepareLockUnspentAsync(_chainName, UUID.NoHyphens, new Dictionary<string, int> { { "", 0 } }, true);
            var expRawExchange = await _wallet.CreateRawExchangeAsync(_chainName, UUID.NoHyphens, expPrepareLockUnspent.Result.Txid, expPrepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });
            var expAppendRaw = await _wallet.AppendRawExchangeAsync(_chainName, UUID.NoHyphens, expRawExchange.Result, expPrepareLockUnspent.Result.Txid, expPrepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            var expComplete = await _wallet.CompleteRawExchangeAsync(_chainName, UUID.NoHyphens, expAppendRaw.Result.Hex, expPrepareLockUnspent.Result.Txid, expPrepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } }, "test".ToHex());

            // Assert
            Assert.IsNull(expComplete.Error);
            Assert.IsNotNull(expComplete.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expComplete);

            /*
               Inferred blockchain name test
            */

            // Stage
            var infPrepareLockUnspent = await _wallet.PrepareLockUnspentAsync(new Dictionary<string, int> { { "", 0 } }, true);
            var infRawExchange = await _wallet.CreateRawExchangeAsync(infPrepareLockUnspent.Result.Txid, infPrepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });
            var infAppendRaw = await _wallet.AppendRawExchangeAsync(infRawExchange.Result, infPrepareLockUnspent.Result.Txid, infPrepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            var infComplete = await _wallet.CompleteRawExchangeAsync(infAppendRaw.Result.Hex, infPrepareLockUnspent.Result.Txid, infPrepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } }, "test".ToHex());

            // Assert
            Assert.IsNull(infComplete.Error);
            Assert.IsNotNull(infComplete.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infComplete);
        }

        [Test]
        public async Task DecodeRawExchangeTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Stage
            var prepareLockUnspent = await _wallet.PrepareLockUnspentAsync(_chainName, UUID.NoHyphens, new Dictionary<string, int> { { "", 0 } }, true);
            var rawExchange = await _wallet.CreateRawExchangeAsync(_chainName, UUID.NoHyphens, prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            var expDecode = await _wallet.DecodeRawExchangeAsync(_chainName, UUID.NoHyphens, rawExchange.Result, true);

            // Assert
            Assert.IsNull(expDecode.Error);
            Assert.IsNotNull(expDecode.Result);
            Assert.IsInstanceOf<RpcResponse<DecodeRawExchangeResult>>(expDecode);

            // Clean-up
            await _wallet.DisableRawTransactionAsync(_chainName, UUID.NoHyphens, rawExchange.Result);

            /*
               Inferred blockchain name test
            */

            // Stage
            var infPrepareLockUnspent = await _wallet.PrepareLockUnspentAsync(new Dictionary<string, int> { { "", 0 } }, true);
            var infRawExchange = await _wallet.CreateRawExchangeAsync(infPrepareLockUnspent.Result.Txid, infPrepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            var infDecode = await _wallet.DecodeRawExchangeAsync(infRawExchange.Result, true);

            // Assert
            Assert.IsNull(infDecode.Error);
            Assert.IsNotNull(infDecode.Result);
            Assert.IsInstanceOf<RpcResponse<DecodeRawExchangeResult>>(infDecode);

            // Clean-up
            await _wallet.DisableRawTransactionAsync(infRawExchange.Result);
        }

        [Test]
        public async Task DisableRawTransactionTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expPrepareLockUnspent = await _wallet.PrepareLockUnspentAsync(_chainName, UUID.NoHyphens, new Dictionary<string, int> { { "", 0 } }, true);
            var iexpRawExchange = await _wallet.CreateRawExchangeAsync(_chainName, UUID.NoHyphens, expPrepareLockUnspent.Result.Txid, expPrepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            var expDisable = await _wallet.DisableRawTransactionAsync(_chainName, UUID.NoHyphens, iexpRawExchange.Result);

            // Assert
            Assert.IsNull(expDisable.Error);
            Assert.IsNotNull(expDisable.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expDisable);

            /*
               Inferred blockchain name test
            */

            // Act
            var infPrepareLockUnspent = await _wallet.PrepareLockUnspentAsync(new Dictionary<string, int> { { "", 0 } }, true);
            var infRawExchange = await _wallet.CreateRawExchangeAsync(infPrepareLockUnspent.Result.Txid, infPrepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            var infDisable = await _wallet.DisableRawTransactionAsync(infRawExchange.Result);

            // Assert
            Assert.IsNull(infDisable.Error);
            Assert.IsNotNull(infDisable.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infDisable);
        }

        [Test]
        public async Task KeyPoolRefillTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act
            var exp = await _wallet.KeyPoolRefillAsync(_chainName, UUID.NoHyphens, 200);

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.KeyPoolRefillAsync(200);

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test]
        public async Task LockUnspentTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Stage
            var expUnspent = await _wallet.PrepareLockUnspentAsync(_chainName, UUID.NoHyphens, new Dictionary<string, int> { { "", 0 } }, true);

            // Act
            var exp = await _wallet.LockUnspentAsync(_chainName, UUID.NoHyphens, false, new Transaction[] { new Transaction { Txid = expUnspent.Result.Txid, Vout = expUnspent.Result.Vout } });

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Stage
            var infUnspent = await _wallet.PrepareLockUnspentAsync(new Dictionary<string, int> { { "", 0 } }, true);

            // Act
            var inf = await _wallet.LockUnspentAsync(false, new Transaction[] { new Transaction { Txid = infUnspent.Result.Txid, Vout = infUnspent.Result.Vout } });

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need move, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task MoveTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act
            var exp = await _wallet.MoveAsync(_chainName, UUID.NoHyphens, "from_account", "to_account", 0.01, 6, "Testing the Move function");

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.MoveAsync("from_account", "to_account", 0.01, 6, "Testing the Move function");

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test]
        public async Task PrepareLockUnspentTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act
            var exp = await _wallet.PrepareLockUnspentAsync(_chainName, UUID.NoHyphens, new Dictionary<string, double> { { "", 0 } }, false);

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentResult>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.PrepareLockUnspentAsync(new Dictionary<string, double> { { "", 0 } }, false);

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentResult>>(inf);
        }

        [Test]
        public async Task PrepareLockUnspentFromTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act
            var exp = await _wallet.PrepareLockUnspentFromAsync(_chainName, UUID.NoHyphens, _address, new Dictionary<string, double> { { "", 0 } }, false);

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentFromResult>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.PrepareLockUnspentFromAsync(_address, new Dictionary<string, double> { { "", 0 } }, false);

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentFromResult>>(inf);
        }

        [Test, Ignore("ResendWalletTransaction test is deffered from normal unit testing")]
        public async Task ResendWalletTransactionsTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act - ttempt to resend the current wallet's transaction
            var exp = await _wallet.ResendWalletTransactionsAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act - ttempt to resend the current wallet's transaction
            var inf = await _wallet.ResendWalletTransactionsAsync();

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test]
        public async Task RevokeTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - Ask the blockchain network for a new address
            var expNewAddress = await _wallet.GetNewAddressAsync(_chainName, UUID.NoHyphens);

            // Stage - Grant new address receive permissions
            await _wallet.GrantAsync(_chainName, UUID.NoHyphens, expNewAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 0, 10000, "", "");

            // Act - Revoke send permission
            var exp = await _wallet.RevokeAsync(_chainName, UUID.NoHyphens, expNewAddress.Result, "send", 0, "Permissions", "Permissions set");

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Stage - Ask the blockchain network for a new address
            var infNewAddress = await _wallet.GetNewAddressAsync();

            // Stage - Grant new address receive permissions
            await _wallet.GrantAsync(infNewAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 0, 10000, "", "");

            // Act - Revoke send permission
            var inf = await _wallet.RevokeAsync(infNewAddress.Result, "send", 0, "Permissions", "Permissions set");

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test]
        public async Task RevokeFromTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - Ask the blockchain network for a new address
            var expNewAddress = await _wallet.GetNewAddressAsync(_chainName, UUID.NoHyphens);

            // Stage - Grant new address receive permissions
            await _wallet.GrantAsync(_chainName, UUID.NoHyphens, expNewAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 0, 10000, "", "");

            // Act - Revoke send permission
            var exp = await _wallet.RevokeFromAsync(_chainName, UUID.NoHyphens, _address, expNewAddress.Result, "send", 0, "", "");

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Stage - Ask the blockchain network for a new address
            var infNewAddress = await _wallet.GetNewAddressAsync();

            // Stage - Grant new address receive permissions
            await _wallet.GrantAsync(infNewAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 0, 10000, "", "");

            // Act - Revoke send permission
            var inf = await _wallet.RevokeFromAsync(_address, infNewAddress.Result, "send", 0, "", "");

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need move, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task SetAccountTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var exp = await _wallet.SetAccountAsync(_chainName, UUID.NoHyphens, _address, "master_account");

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.SetAccountAsync(_address, "master_account");

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test, Ignore("Ignored since I do not want to change the TxFee while other transactions are runningh")]
        public async Task SetTxFeeTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var exp = await _wallet.SetTxFeeAsync(_chainName, UUID.NoHyphens, 0.0001);

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.SetTxFeeAsync(0.0001);

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test]
        public async Task SignMessageTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var exp = await _wallet.SignMessageAsync(_chainName, UUID.NoHyphens, _address, "Testing the SignMessage function");

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.SignMessageAsync(_address, "Testing the SignMessage function");

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(inf);
        }

        [Test]
        public async Task SubscribeTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var exp = await _wallet.SubscribeAsync(_chainName, UUID.NoHyphens, "root", false, "");

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.SubscribeAsync("root", false, "");

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test]
        public async Task TxOutToBinaryCacheTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Stage
            var expBinaryCache = await _utility.CreateBinaryCacheAsync(_chainName, UUID.NoHyphens);
            var expPublish = await _wallet.PublishFromAsync(_chainName, UUID.NoHyphens, _address, "root", ChainEntity.GetUUID(), "A bunch of text data that will be transcribed to this this publish event and this one is async brotato chip".ToHex(), "");
            var expTransaction = await _wallet.GetAddressTransactionAsync(_chainName, UUID.NoHyphens, _address, expPublish.Result, true);

            // Act
            var exp = await _wallet.TxOutToBinaryCacheAsync(_chainName, UUID.NoHyphens, expBinaryCache.Result, expTransaction.Result.Txid, expTransaction.Result.Vout[0].N, 100000, 0);

            // Clean-up
            await _utility.DeleteBinaryCacheAsync(_chainName, UUID.NoHyphens, expBinaryCache.Result);

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<double>>(exp);

            /*
               Inferred blockchain name test
            */

            // Stage
            var infBinaryCache = await _utility.CreateBinaryCacheAsync();
            var infPublish = await _wallet.PublishFromAsync(_address, "root", ChainEntity.GetUUID(), "A bunch of text data that will be transcribed to this this publish event and this one is async brotato chip".ToHex(), "");
            var infTransaction = await _wallet.GetAddressTransactionAsync(_address, infPublish.Result, true);

            // Act
            var inf = await _wallet.TxOutToBinaryCacheAsync(infBinaryCache.Result, infTransaction.Result.Txid, infTransaction.Result.Vout[0].N, 100000, 0);

            // Clean-up
            await _utility.DeleteBinaryCacheAsync(infBinaryCache.Result);

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<double>>(inf);
        }

        [Test]
        public async Task UnsubscribeTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var exp = await _wallet.UnsubscribeAsync(_chainName, UUID.NoHyphens, "root", false);

            // Act
            await _wallet.SubscribeAsync(_chainName, UUID.NoHyphens, "root", false, "");

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.UnsubscribeAsync("root", false);

            // Act
            await _wallet.SubscribeAsync("root", false, "");

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }
    }
}
