using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Test.ServicesPipeline;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Test.Issue
{
    [TestFixture]
    public class RpcWalletIssueTests
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

        // Use mock startup service container
        private readonly ExplicitStartup _services = new ExplicitStartup();

        // Create a new IssueAssetTests instance
        public RpcWalletIssueTests()
        {
            _wallet = _services.GetRequiredService<IMultiChainRpcWallet>();
            _address = _wallet.RpcOptions.ChainAdminAddress;
            _chainName = _wallet.RpcOptions.ChainName;
        }

        public async Task IssueFromTestStronglyTypedAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act
            var expAct_1 = await _wallet.IssueFromAsync(_chainName, UUID.NoHyphens, _address, _address, new AssetEntity(), 100, 1, 0.1m,
                new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(expAct_1.Error);
            Assert.IsNotNull(expAct_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expAct_1);

            // Act
            var expAct_2 = await _wallet.IssueFromAsync(_chainName, UUID.NoHyphens, _address, _address, new AssetEntity(), 100, 1, 0.1m,
                new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(expAct_2.Error);
            Assert.IsNotNull(expAct_2.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expAct_2);

            /*
              Inferred blockchain name test
           */

            // Act
            var infAct_1 = await _wallet.IssueFromAsync(_address, _address, new AssetEntity(), 100, 1, 0.1m,
                new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(infAct_1.Error);
            Assert.IsNotNull(infAct_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infAct_1);

            // Act
            var infAct_2 = await _wallet.IssueFromAsync(_address, _address, new AssetEntity(), 100, 1, 0.1m, 
                new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(infAct_2.Error);
            Assert.IsNotNull(infAct_2.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infAct_2);
        }

        public async Task IssueFromTestGenericallyTypedAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act
            var expAct_1 = await _wallet.IssueFromAsync(_chainName, UUID.NoHyphens, _address, _address, new { name = UUID.NoHyphens, isOpen = true }, 100, 1, 0.1m,
                new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(expAct_1.Error);
            Assert.IsNotNull(expAct_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expAct_1);

            // Act
            var expAct_2 = await _wallet.IssueFromAsync(_chainName, UUID.NoHyphens, _address, _address, new { name = UUID.NoHyphens, isOpen = true }, 100, 1, 0.1m,
                new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(expAct_2.Error);
            Assert.IsNotNull(expAct_2.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expAct_2);

            /*
              Inferred blockchain name test
           */

            // Act
            var infAct_1 = await _wallet.IssueFromAsync(_address, _address, new { name = UUID.NoHyphens, isOpen = true }, 100, 1, 0.1m,
                new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(infAct_1.Error);
            Assert.IsNotNull(infAct_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infAct_1);

            // Act
            var infAct_2 = await _wallet.IssueFromAsync(_address, _address, new { name = UUID.NoHyphens, isOpen = true }, 100, 1, 0.1m,
                new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(infAct_2.Error);
            Assert.IsNotNull(infAct_2.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infAct_2);
        }

        public async Task IssueFromTestStringNameAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act
            var expAct_1 = await _wallet.IssueFromAsync(_chainName, UUID.NoHyphens, _address, _address, UUID.NoHyphens, 100, 1, 0.1m,
                new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(expAct_1.Error);
            Assert.IsNotNull(expAct_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expAct_1);

            // Act
            var expAct_2 = await _wallet.IssueFromAsync(_chainName, UUID.NoHyphens, _address, _address, UUID.NoHyphens, 100, 1, 0.1m,
                new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(expAct_2.Error);
            Assert.IsNotNull(expAct_2.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expAct_2);

            /*
              Inferred blockchain name test
           */

            // Act
            var infAct_1 = await _wallet.IssueFromAsync(_address, _address, UUID.NoHyphens, 100, 1, 0.1m, 
                new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(infAct_1.Error);
            Assert.IsNotNull(infAct_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infAct_1);

            // Act
            var infAct_2 = await _wallet.IssueFromAsync(_address, _address, UUID.NoHyphens, 100, 1, 0.1m,
                new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(infAct_2.Error);
            Assert.IsNotNull(infAct_2.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infAct_2);
        }

        [Test]
        public async Task IssueMoreFromTestAsync()
        {
            // Stage
            var issue = await _wallet.IssueFromAsync(_address, _address, new AssetEntity(), 100, 1, 0, default);

            /*
              Explicit blockchain name test
           */

            // Act
            var expAct_1 = await _wallet.IssueMoreFromAsync(_chainName, UUID.NoHyphens, _address, _address, issue.Result, 100, 0,
                new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(expAct_1.Error);
            Assert.IsNotNull(expAct_1.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expAct_1);


            // Act
            var expAct_2 = await _wallet.IssueMoreFromAsync(_chainName, UUID.NoHyphens, _address, _address, issue.Result, 100, 0,
                new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(expAct_2.Error);
            Assert.IsNotNull(expAct_2.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expAct_2);

            /*
              Inferred blockchain name test
           */

            // Act
            var infAct_1 = await _wallet.IssueMoreFromAsync(_address, _address, issue.Result, 100, 0,
                new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(infAct_1.Error);
            Assert.IsNotNull(infAct_1.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infAct_1);


            // Act
            var infAct_2 = await _wallet.IssueMoreFromAsync(_address, _address, issue.Result, 100, 0,
                new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(infAct_2.Error);
            Assert.IsNotNull(infAct_2.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infAct_2);
        }

        [Test]
        public async Task IssueMoreTestAsync()
        {
            // Stage
            var issue = await _wallet.IssueAsync(_address, new AssetEntity(), 100, 1, 0, default);

            /*
              Explicit blockchain name test
           */

            // Act
            var expAct_1 = await _wallet.IssueMoreAsync(_chainName, UUID.NoHyphens, _address, issue.Result.ToString(), 100, 0,
                new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(expAct_1.Error);
            Assert.IsNotNull(expAct_1.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expAct_1);

            // Act
            var expAct_2 = await _wallet.IssueMoreAsync(_chainName, UUID.NoHyphens, _address, issue.Result, 100, 0,
                new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(expAct_2.Error);
            Assert.IsNotNull(expAct_2.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expAct_2);

            /*
              Inferred blockchain name test
           */

            // Act
            var infAct_1 = await _wallet.IssueMoreAsync(_address, issue.Result.ToString(), 100, 0,
                new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(infAct_1.Error);
            Assert.IsNotNull(infAct_1.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infAct_1);

            // Act
            var infAct_2 = await _wallet.IssueMoreAsync(_address, issue.Result, 100, 0,
                new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(infAct_2.Error);
            Assert.IsNotNull(infAct_2.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infAct_2);
        }

        [Test]
        public async Task IssueTestStronlyTypedAsync()
        {
            // Act - ask target network for a new blockchain address
            var newAddress = await _wallet.GetNewAddressAsync();
            await _wallet.GrantAsync(newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000);

            /*
              Explicit blockchain name test
           */

            // Act - issue a new asset using a AssetEntity class object
            var expAct_1 = await _wallet.IssueAsync(_chainName, UUID.NoHyphens, newAddress.Result, new AssetEntity(name: UUID.NoHyphens, isOpen: true, restrictions: "send,receive"), 100, 1, 0,
                new Dictionary<string, string>()
                {
                    { "text", "Dictionary test".ToHex() },
                    { "description", "Even more text data".ToHex() }
                });

            // Assert - proof issue response
            Assert.IsNull(expAct_1.Error);
            Assert.IsNotNull(expAct_1.Result);
            Assert.IsInstanceOf<string>(expAct_1.Result);
            Assert.GreaterOrEqual(expAct_1.Result.Length, 64);


            // Act - issue a new asset using a generic object
            var expAct_2 = await _wallet.IssueAsync(_chainName, UUID.NoHyphens, newAddress.Result, new AssetEntity(name: UUID.NoHyphens, isOpen: true, restrictions: "send,receive"), 100, 1, 0,
                new { text = "Dictionary test".ToHex(), description = "Even more text data".ToHex() });

            // Assert - proof issue response
            Assert.IsNull(expAct_2.Error);
            Assert.IsNotNull(expAct_2.Result);
            Assert.IsInstanceOf<string>(expAct_2.Result);
            Assert.GreaterOrEqual(expAct_2.Result.Length, 64);

            /*
              Inferred blockchain name test
           */

            // Act - issue a new asset using a AssetEntity class object
            var infAct_1 = await _wallet.IssueAsync(newAddress.Result, new AssetEntity(name: UUID.NoHyphens, isOpen: true, restrictions: "send,receive"), 100, 1, 0,
                new Dictionary<string, string>()
                {
                    { "text", "Dictionary test".ToHex() },
                    { "description", "Even more text data".ToHex() }
                });

            // Assert - proof issue response
            Assert.IsNull(infAct_1.Error);
            Assert.IsNotNull(infAct_1.Result);
            Assert.IsInstanceOf<string>(infAct_1.Result);
            Assert.GreaterOrEqual(infAct_1.Result.Length, 64);


            // Act - issue a new asset using a generic object
            var infAct_2 = await _wallet.IssueAsync(newAddress.Result, new AssetEntity(name: UUID.NoHyphens, isOpen: true, restrictions: "send,receive"), 100, 1, 0,
                new { text = "Dictionary test".ToHex(), description = "Even more text data".ToHex() });

            // Assert - proof issue response
            Assert.IsNull(infAct_2.Error);
            Assert.IsNotNull(infAct_2.Result);
            Assert.IsInstanceOf<string>(infAct_2.Result);
            Assert.GreaterOrEqual(infAct_2.Result.Length, 64);
        }

        [Test]
        public async Task IssueTestGenericallyTypedAsync()
        {
            // Act - ask target network for a new blockchain address
            var newAddress = await _wallet.GetNewAddressAsync();
            await _wallet.GrantAsync(newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000);

            /*
              Explicit blockchain name test
           */

            // Act - issue a new asset using a strongly typed AssetEntity class object
            var expAct_1 = await _wallet.IssueAsync(_chainName, UUID.NoHyphens, newAddress.Result, new { name = UUID.NoHyphens, isOpen = true, restrictions = "send,receive" }, 100, 1, 0,
                new Dictionary<string, string>()
                {
                    { "text", "Dictionary test".ToHex() },
                    { "description", "Even more text data".ToHex() }
                });

            // Assert - proof issue response
            Assert.IsNull(expAct_1.Error);
            Assert.IsNotNull(expAct_1.Result);
            Assert.IsInstanceOf<string>(expAct_1.Result);
            Assert.GreaterOrEqual(expAct_1.Result.Length, 64);


            // Act - issue a new asset using a strongly typed AssetEntity class object
            var expAct_2 = await _wallet.IssueAsync(_chainName, UUID.NoHyphens, newAddress.Result, new { name = UUID.NoHyphens, isOpen = true, restrictions = "send,receive" }, 100, 1, 0,
                new { text = "Dictionary test".ToHex(), description = "Even more text data".ToHex() });

            // Assert - proof issue response
            Assert.IsNull(expAct_2.Error);
            Assert.IsNotNull(expAct_2.Result);
            Assert.IsInstanceOf<string>(expAct_2.Result);
            Assert.GreaterOrEqual(expAct_2.Result.Length, 64);

            /*
              Inferred blockchain name test
           */

            // Act - issue a new asset using a strongly typed AssetEntity class object
            var infAct_1 = await _wallet.IssueAsync(newAddress.Result, new { name = UUID.NoHyphens, isOpen = true, restrictions = "send,receive" }, 100, 1, 0,
                new Dictionary<string, string>()
                {
                    { "text", "Dictionary test".ToHex() },
                    { "description", "Even more text data".ToHex() }
                });

            // Assert - proof issue response
            Assert.IsNull(infAct_1.Error);
            Assert.IsNotNull(infAct_1.Result);
            Assert.IsInstanceOf<string>(infAct_1.Result);
            Assert.GreaterOrEqual(infAct_1.Result.Length, 64);


            // Act - issue a new asset using a strongly typed AssetEntity class object
            var infAct_2 = await _wallet.IssueAsync(newAddress.Result, new { name = UUID.NoHyphens, isOpen = true, restrictions = "send,receive" }, 100, 1, 0,
                new { text = "Dictionary test".ToHex(), description = "Even more text data".ToHex() });

            // Assert - proof issue response
            Assert.IsNull(infAct_2.Error);
            Assert.IsNotNull(infAct_2.Result);
            Assert.IsInstanceOf<string>(infAct_2.Result);
            Assert.GreaterOrEqual(infAct_2.Result.Length, 64);
        }

        [Test]
        public async Task IssueTestStringNameAsync()
        {
            // Stage - ask target network for a new blockchain address
            var newAddress = await _wallet.GetNewAddressAsync();
            await _wallet.GrantAsync(newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000);

            /*
              Explicit blockchain name test
           */

            // Act - issue a new asset using a strongly typed AssetEntity class object
            var expAct_1 = await _wallet.IssueAsync(_chainName, UUID.NoHyphens, newAddress.Result, UUID.NoHyphens, 100, 1, 0,
                new Dictionary<string, string>()
                {
                    { "text", "Dictionary test".ToHex() },
                    { "description", "Even more text data".ToHex() }
                });

            // Assert - proof issue response
            Assert.IsNull(expAct_1.Error);
            Assert.IsNotNull(expAct_1.Result);
            Assert.IsInstanceOf<string>(expAct_1.Result);
            Assert.GreaterOrEqual(expAct_1.Result.Length, 64);


            // Act - issue a new asset using a strongly typed AssetEntity class object
            var expAct_2 = await _wallet.IssueAsync(_chainName, UUID.NoHyphens, newAddress.Result, UUID.NoHyphens, 100, 1, 0,
                new { text = "Dictionary test".ToHex(), description = "Even more text data".ToHex() });

            // Assert - proof issue response
            Assert.IsNull(expAct_2.Error);
            Assert.IsNotNull(expAct_2.Result);
            Assert.IsInstanceOf<string>(expAct_2.Result);
            Assert.GreaterOrEqual(expAct_2.Result.Length, 64);

            /*
              Inferred blockchain name test
           */

            // Act - issue a new asset using a strongly typed AssetEntity class object
            var infAct_1 = await _wallet.IssueAsync(newAddress.Result, UUID.NoHyphens, 100, 1, 0,
                new Dictionary<string, string>()
                {
                    { "text", "Dictionary test".ToHex() },
                    { "description", "Even more text data".ToHex() }
                });

            // Assert - proof issue response
            Assert.IsNull(infAct_1.Error);
            Assert.IsNotNull(infAct_1.Result);
            Assert.IsInstanceOf<string>(infAct_1.Result);
            Assert.GreaterOrEqual(infAct_1.Result.Length, 64);


            // Act - issue a new asset using a strongly typed AssetEntity class object
            var infAct_2 = await _wallet.IssueAsync(newAddress.Result, UUID.NoHyphens, 100, 1, 0,
                new { text = "Dictionary test".ToHex(), description = "Even more text data".ToHex() });

            // Assert - proof issue response
            Assert.IsNull(infAct_2.Error);
            Assert.IsNotNull(infAct_2.Result);
            Assert.IsInstanceOf<string>(infAct_2.Result);
            Assert.GreaterOrEqual(infAct_2.Result.Length, 64);
        }
    }
}
