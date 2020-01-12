using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCWrapper.RPC.Tests.Tests
{
    [TestFixture]
    public class RpcWalletSendTests
    {
        [Test]
        public async Task SendTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            /*
               Inferred blockchain name test
            */

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
            /*
              Explicit blockchain name test
           */

            /*
               Inferred blockchain name test
            */

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
            /*
              Explicit blockchain name test
           */

            /*
               Inferred blockchain name test
            */

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
            /*
              Explicit blockchain name test
           */

            /*
               Inferred blockchain name test
            */

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
            /*
              Explicit blockchain name test
           */

            /*
               Inferred blockchain name test
            */

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
            /*
              Explicit blockchain name test
           */

            /*
               Inferred blockchain name test
            */

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
            /*
              Explicit blockchain name test
           */

            /*
               Inferred blockchain name test
            */

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
            /*
              Explicit blockchain name test
           */

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
