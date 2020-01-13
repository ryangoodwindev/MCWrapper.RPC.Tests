using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Test.ServicesPipeline;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests.Tests
{
    [TestFixture]
    public class RpcWalletSendTests
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

        public RpcWalletSendTests()
        {
            _wallet = _services.GetRequiredService<IMultiChainRpcWallet>();
            _address = _wallet.RpcOptions.ChainAdminAddress;
            _chainName = _wallet.RpcOptions.ChainName;
        }

        [Test]
        public async Task SendTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act
            var exp = await _wallet.SendAsync(_chainName, UUID.NoHyphens, _address, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.SendAsync(_address, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(inf);
        }

        [Test]
        public async Task SendAssetTestAsync()
        {
            // Stage
            var asset = await _wallet.IssueAsync(_address, new AssetEntity(), 100, 1, 0, default);

            /*
              Explicit blockchain name test
           */

            // Act
            var exp = await _wallet.SendAssetAsync(_chainName, UUID.NoHyphens, _address, asset.Result, 1, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.SendAssetAsync(_address, asset.Result, 1, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test]
        public async Task SendAssetFromTestAsync()
        {
            // Stage
            var newAddress = await _wallet.GetNewAddressAsync();
            await _wallet.GrantAsync(newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000, "", "");
            var asset = await _wallet.IssueAsync(_address, new AssetEntity(), 100, 1, 0, new Dictionary<string, string>
                { { "text", "text to hex".ToHex() } });

            /*
              Explicit blockchain name test
           */

            // Act
            var exp = await _wallet.SendAssetFromAsync(_chainName, UUID.NoHyphens, _address, newAddress.Result, asset.Result, 1, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.SendAssetFromAsync(_address, newAddress.Result, asset.Result, 1, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test]
        public async Task SendFromTestAsync()
        {
            // Stage
            var newAddress = await _wallet.GetNewAddressAsync();
            await _wallet.GrantAsync(newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000, "", "");

            /*
              Explicit blockchain name test
           */

            // Act
            var exp = await _wallet.SendFromAsync(_chainName, UUID.NoHyphens, _address, newAddress.Result, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.SendFromAsync(_address, newAddress.Result, 0, "Comment text", "Comment_To text");

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need sendfrom, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task SendFromAccountTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act
            var exp = await _wallet.SendFromAccountAsync(_chainName, UUID.NoHyphens, _address, _address, .001, 2, "Comment Text", "Comment_To text");

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.SendFromAccountAsync(_address, _address, .001, 2, "Comment Text", "Comment_To text");

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need sendmany, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task SendManyTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act
            var exp = await _wallet.SendManyAsync(_chainName, UUID.NoHyphens, "", new object[] { new Dictionary<string, double> { { _address, 1 } } }, 2, "Comment text");

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.SendManyAsync("", new object[] { new Dictionary<string, double> { { _address, 1 } } }, 2, "Comment text");

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test]
        public async Task SendWithDataTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act
            var exp = await _wallet.SendWithDataAsync(_chainName, UUID.NoHyphens, _address, 0, "some data".ToHex());

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.SendWithDataAsync(_address, 0, "some data".ToHex());

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }

        [Test]
        public async Task SendWithDataFromTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act
            var exp = await _wallet.SendWithDataFromAsync(_chainName, UUID.NoHyphens, _address, _address, 0, "some data".ToHex());

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.SendWithDataFromAsync(_address, _address, 0, "some data".ToHex());

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
        }
    }
}
