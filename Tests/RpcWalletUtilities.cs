using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Test.ServicesPipeline;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Test.Wallet.Utilities
{
    [TestFixture]
    public class RpcWalletUtilities
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

        private readonly IMultiChainRpcWallet _wallet;
        private readonly string _chainName;
        private readonly string _address;

        private readonly ExplicitStartup _services = new ExplicitStartup();

        public RpcWalletUtilities()
        {
            _wallet = _services.GetRequiredService<IMultiChainRpcWallet>();
            _address = _wallet.RpcOptions.ChainAdminAddress;
            _chainName = _wallet.RpcOptions.ChainName;
        }

        [Test, Ignore("BackupWallet test ignored since it halts the blockchain network")]
        public async Task BackupWalletTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expBackup = await _wallet.BackupWalletAsync(_chainName, UUID.NoHyphens, "backup.dat");

            // Assert
            Assert.IsTrue(expBackup.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(expBackup);

            /*
               Inferred blockchain name test
            */

            // Act
            var infBackup = await _wallet.BackupWalletAsync("backup.dat");

            // Assert
            Assert.IsTrue(infBackup.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(infBackup);
        }

        [Test]
        public async Task DumpPrivKeyTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var exp = await _wallet.DumpPrivKeyAsync(_chainName, UUID.NoHyphens, _address);

            // Assert
            Assert.IsTrue(exp.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.DumpPrivKeyAsync(_address);

            // Assert
            Assert.IsTrue(inf.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(inf);
        }

        [Test, Ignore("Dumping the wallet seems to slow down the network. Test is passing and ignored.")]
        public async Task DumpWalletTestAync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var exp = await _wallet.DumpWalletAsync(_chainName, UUID.NoHyphens, "test_async");

            // Assert
            Assert.IsTrue(exp.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.DumpWalletAsync("test_async");

            // Assert
            Assert.IsTrue(inf.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test, Ignore("Test is implemented and ignored since I don't want to encrypt my wallet in staging")]
        public async Task EncryptWalletTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var exp = await _wallet.EncryptWalletAsync(_chainName, UUID.NoHyphens, "some_password");

            // Assert
            Assert.IsTrue(exp.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.EncryptWalletAsync("some_password");

            // Assert
            Assert.IsTrue(inf.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test, Ignore("I don't want to import any addresses during unit testing")]
        public async Task ImportAddressTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var exp = await _wallet.ImportAddressAsync(_chainName, UUID.NoHyphens, "some_external_address", "some_label", false);

            // Assert
            Assert.IsTrue(exp.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.ImportAddressAsync("some_external_address", "some_label", false);

            // Assert
            Assert.IsTrue(inf.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test, Ignore("I don't want to import any private keys during unit testing")]
        public async Task ImportPrivKeyTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var exp = await _wallet.ImportPrivKeyAsync(_chainName, UUID.NoHyphens, "some_external_private_key", "some_label", false);

            // Assert
            Assert.IsTrue(exp.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.ImportPrivKeyAsync("some_external_private_key", "some_label", false);

            // Assert
            Assert.IsTrue(inf.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test, Ignore("Tests impacting the current wallet are ignore while general tests are running")]
        public async Task ImportWalletTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var exp = await _wallet.ImportWalletAsync(_chainName, UUID.NoHyphens, "test", false);

            // Assert
            Assert.IsTrue(exp.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.ImportWalletAsync("test", false);

            // Assert
            Assert.IsTrue(inf.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test, Ignore("Wallet related tests are ignored while general tests are running")]
        public async Task WalletLockTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var exp = await _wallet.WalletLockAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsTrue(exp.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.WalletLockAsync();

            // Assert
            Assert.IsTrue(inf.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test, Ignore("Wallet related tests are ignored while general tests are running")]
        public async Task WalletPassphraseTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var exp = await _wallet.WalletPassphraseAsync(_chainName, UUID.NoHyphens, "wallet_passphrase", 10);

            // Assert
            Assert.IsTrue(exp.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.WalletPassphraseAsync("wallet_passphrase", 10);

            // Assert
            Assert.IsTrue(inf.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test, Ignore("Wallet related tests are ignored while general tests are running")]
        public async Task WalletPassphraseChangeTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var exp = await _wallet.WalletPassphraseChangeAsync(_chainName, UUID.NoHyphens, "old_passphrase", "new_passphrase");

            // Assert
            Assert.IsTrue(exp.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.WalletPassphraseChangeAsync("old_passphrase", "new_passphrase");

            // Assert
            Assert.IsTrue(inf.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }
    }
}