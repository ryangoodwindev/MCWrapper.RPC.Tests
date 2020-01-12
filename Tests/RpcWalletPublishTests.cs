using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCWrapper.RPC.Tests.Tests
{
    [TestFixture]
    public class RpcWalletPublishTests
    {

        [Test]
        public async Task PublishTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            /*
               Inferred blockchain name test
            */

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
            /*
              Explicit blockchain name test
           */

            /*
               Inferred blockchain name test
            */

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
            /*
              Explicit blockchain name test
           */

            /*
               Inferred blockchain name test
            */

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
            /*
              Explicit blockchain name test
           */

            /*
               Inferred blockchain name test
            */

            // Act
            RpcResponse<string> actual = await _wallet.PublishMultiFromAsync(_address, "root", new object[] { new { key = "some_key", data = "some_data".ToHex() } }, "offchain");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }
    }
}
