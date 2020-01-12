using System;
using System.Collections.Generic;
using System.Text;

namespace MCWrapper.RPC.Tests.Tests
{
    class RpcWalletGrantTests
    {
        [Test]
        public async Task GrantFromTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Stage
            var newAddress = await _wallet.GetNewAddressAsync();

            // Act
            RpcResponse<string> actual = await _wallet.GrantFromAsync(_address, newAddress.Result, Permission.Receive, 0, 1, 1000, "", "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);

            /*
               Inferred blockchain name test
            */

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
            /*
               Explicit blockchain name test
            */

            // Stage
            var newAddress = await _wallet.GetNewAddressAsync();

            // Act
            RpcResponse<string> actual = await _wallet.GrantAsync(newAddress.Result, Permission.Receive, 0, 1, 1000, "", "");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);

            /*
               Inferred blockchain name test
            */

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
            /*
               Explicit blockchain name test
            */

            // Stage
            var newAddress = await _wallet.GetNewAddressAsync();

            // Act
            RpcResponse<string> actual = await _wallet.GrantWithDataFromAsync(_address, newAddress.Result, Permission.Receive, "some_data".ToHex(), 0, 1, 1000);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);

            /*
               Inferred blockchain name test
            */

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
            /*
               Explicit blockchain name test
            */

            // Stage
            var newAddress = await _wallet.GetNewAddressAsync();

            // Act
            RpcResponse<string> actual = await _wallet.GrantWithDataAsync(newAddress.Result, Permission.Receive, "some_data".ToHex(), 0, 1, 1000);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);

            /*
               Inferred blockchain name test
            */

            // Stage
            var newAddress = await _wallet.GetNewAddressAsync();

            // Act
            RpcResponse<string> actual = await _wallet.GrantWithDataAsync(newAddress.Result, Permission.Receive, "some_data".ToHex(), 0, 1, 1000);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }
    }
}
