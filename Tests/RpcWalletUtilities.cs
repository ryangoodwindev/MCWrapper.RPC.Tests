using System;
using System.Collections.Generic;
using System.Text;

namespace MCWrapper.RPC.Tests.Tests
{
    class RpcWalletUtilities
    {
        [Test, Ignore("BackupWallet test ignored since it halts the blockchain network")]
        public async Task BackupWalletTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expBackup = await _wallet.BackupWalletAsync(_chainName, UUID.NoHyphens, "backup.dat");

            // Assert
            Assert.IsNull(expBackup.Error);
            Assert.IsNotNull(expBackup.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expBackup);

            /*
               Inferred blockchain name test
            */

            // Act
            var infBackup = await _wallet.BackupWalletAsync("backup.dat");

            // Assert
            Assert.IsNull(infBackup.Error);
            Assert.IsNotNull(infBackup.Result);
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
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.DumpPrivKeyAsync(_address);

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
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
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.DumpWalletAsync("test_async");

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
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
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.EncryptWalletAsync("some_password");

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }



        [Test, Ignore("I don't want to import any addresses during unit testing")]
        public async Task ImportAddressTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            RpcResponse<object> actual = await _wallet.ImportAddressAsync("some_external_address", "some_label", false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);

            /*
               Inferred blockchain name test
            */

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
            /*
               Explicit blockchain name test
            */

            // Act
            RpcResponse<object> actual = await _wallet.ImportPrivKeyAsync("some_external_private_key", "some_label", false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);

            /*
               Inferred blockchain name test
            */

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
            /*
               Explicit blockchain name test
            */

            // Act
            RpcResponse<object> actual = await _wallet.ImportWalletAsync("test", false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);

            /*
               Inferred blockchain name test
            */

            // Act
            RpcResponse<object> actual = await _wallet.ImportWalletAsync("test", false);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
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
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.WalletLockAsync();

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
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
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.WalletPassphraseAsync("wallet_passphrase", 10);

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
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
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.WalletPassphraseChangeAsync("old_passphrase", "new_passphrase");

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }
    }
}
