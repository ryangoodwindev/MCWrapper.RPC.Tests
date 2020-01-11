using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServicesPipeline;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class RpcWalletIssueTests
    {
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
            // Act
            RpcResponse<string> act_1 = await _wallet.IssueFromAsync(_address, _address, new AssetEntity(), 100, 1, 0.1m,
                new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_1);

            // Act
            RpcResponse<string> act_2 = await _wallet.IssueFromAsync(_address, _address, new AssetEntity(), 100, 1, 0.1m, 
                new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_2);
        }

        public async Task IssueFromTestGenericallyTypedAsync()
        {
            // Act
            RpcResponse<string> act_1 = await _wallet.IssueFromAsync(_address, _address, new { name = UUID.NoHyphens, isOpen = true }, 100, 1, 0.1m,
                new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_1);

            // Act
            RpcResponse<string> act_2 = await _wallet.IssueFromAsync(_address, _address, new { name = UUID.NoHyphens, isOpen = true }, 100, 1, 0.1m,
                new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_2);
        }

        public async Task IssueFromTestStringNameAsync()
        {
            // Act
            RpcResponse<string> act_1 = await _wallet.IssueFromAsync(_address, _address, UUID.NoHyphens, 100, 1, 0.1m, 
                new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_1);

            // Act
            RpcResponse<string> act_2 = await _wallet.IssueFromAsync(_address, _address, UUID.NoHyphens, 100, 1, 0.1m,
                new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(act_2);
        }

        [Test]
        public async Task IssueMoreFromTestAsync()
        {
            // Stage
            var issue = await _wallet.IssueFromAsync(_address, _address, new AssetEntity(), 100, 1, 0, default);

            // Act
            RpcResponse<object> act_1 = await _wallet.IssueMoreFromAsync(_address, _address, issue.Result, 100, 0,
                new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(act_1);


            // Act
            RpcResponse<object> act_2 = await _wallet.IssueMoreFromAsync(_address, _address, issue.Result, 100, 0,
                new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(act_2);
        }

        [Test]
        public async Task IssueMoreTestAsync()
        {
            // Stage
            var issue = await _wallet.IssueAsync(_address, new AssetEntity(), 100, 1, 0, default);

            // Act
            RpcResponse<object> act_1 = await _wallet.IssueMoreAsync(_address, issue.Result.ToString(), 100, 0,
                new Dictionary<string, string> { { "text", "Some test data text".ToHex() } });

            // Assert
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(act_1);

            // Act
            RpcResponse<object> act_2 = await _wallet.IssueMoreAsync(_address, issue.Result, 100, 0,
                new { text = "Some test data text".ToHex() });

            // Assert
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(act_2);
        }

        [Test]
        public async Task IssueTestStronlyTypedAsync()
        {
            // Act - ask target network for a new blockchain address
            var newAddress = await _wallet.GetNewAddressAsync();

            // Assert - proof new address response
            Assert.IsNull(newAddress.Error);
            Assert.IsNotNull(newAddress.Result);
            Assert.IsInstanceOf<string>(newAddress.Result);
            Assert.GreaterOrEqual(newAddress.Result.Length, 35);


            // Act - grant the new address receive,send permissions
            var grant = await _wallet.GrantAsync(newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000);

            // Assert - proof grant response
            Assert.IsNull(grant.Error);
            Assert.IsNotNull(grant.Result);
            Assert.IsInstanceOf<string>(grant.Result);
            Assert.GreaterOrEqual(grant.Result.Length, 64);


            // Act - issue a new asset using a AssetEntity class object
            RpcResponse<string> act_1 = await _wallet.IssueAsync(newAddress.Result, new AssetEntity(name: UUID.NoHyphens, isOpen: true, restrictions: "send,receive"), 100, 1, 0,
                new Dictionary<string, string>()
                {
                    { "text", "Dictionary test".ToHex() },
                    { "description", "Even more text data".ToHex() }
                });

            // Assert - proof issue response
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<string>(act_1.Result);
            Assert.GreaterOrEqual(act_1.Result.Length, 64);


            // Act - issue a new asset using a generic object
            RpcResponse<string> act_2 = await _wallet.IssueAsync(newAddress.Result, new AssetEntity(name: UUID.NoHyphens, isOpen: true, restrictions: "send,receive"), 100, 1, 0,
                new { text = "Dictionary test".ToHex(), description = "Even more text data".ToHex() });

            // Assert - proof issue response
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<string>(act_2.Result);
            Assert.GreaterOrEqual(act_2.Result.Length, 64);
        }

        [Test]
        public async Task IssueTestGenericallyTypedAsync()
        {
            // Act - ask target network for a new blockchain address
            var newAddress = await _wallet.GetNewAddressAsync();

            // Assert - proof new address response
            Assert.IsNull(newAddress.Error);
            Assert.IsNotNull(newAddress.Result);
            Assert.IsInstanceOf<string>(newAddress.Result);
            Assert.GreaterOrEqual(newAddress.Result.Length, 35);


            // Act - grant the new address receive,send permissions
            var grant = await _wallet.GrantAsync(newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000);

            // Assert - proof grant response
            Assert.IsNull(grant.Error);
            Assert.IsNotNull(grant.Result);
            Assert.IsInstanceOf<string>(grant.Result);
            Assert.GreaterOrEqual(grant.Result.Length, 64);


            // Act - issue a new asset using a strongly typed AssetEntity class object
            RpcResponse<string> act_1 = await _wallet.IssueAsync(newAddress.Result, new { name = UUID.NoHyphens, isOpen = true, restrictions = "send,receive" }, 100, 1, 0,
                new Dictionary<string, string>()
                {
                    { "text", "Dictionary test".ToHex() },
                    { "description", "Even more text data".ToHex() }
                });

            // Assert - proof issue response
            Assert.IsNull(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<string>(act_1.Result);
            Assert.GreaterOrEqual(act_1.Result.Length, 64);


            // Act - issue a new asset using a strongly typed AssetEntity class object
            RpcResponse<string> act_2 = await _wallet.IssueAsync(newAddress.Result, new { name = UUID.NoHyphens, isOpen = true, restrictions = "send,receive" }, 100, 1, 0,
                new { text = "Dictionary test".ToHex(), description = "Even more text data".ToHex() });

            // Assert - proof issue response
            Assert.IsNull(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<string>(act_2.Result);
            Assert.GreaterOrEqual(act_2.Result.Length, 64);
        }

        [Test]
        public async Task IssueTestStringNameAsync()
        {
            // Act - ask target network for a new blockchain address
            var newAddress = await _wallet.GetNewAddressAsync();

            // Assert - proof new address response
            Assert.IsNull(newAddress.Error);
            Assert.IsNotNull(newAddress.Result);
            Assert.IsInstanceOf<string>(newAddress.Result);
            Assert.GreaterOrEqual(newAddress.Result.Length, 35);


            // Act - grant the new address receive,send permissions
            var infGrant = await _wallet.GrantAsync(newAddress.Result, $"{Permission.Receive},{Permission.Send}", 0, 1, 10000);

            // Assert - proof grant response
            Assert.IsNull(infGrant.Error);
            Assert.IsNotNull(infGrant.Result);
            Assert.IsInstanceOf<string>(infGrant.Result);
            Assert.GreaterOrEqual(infGrant.Result.Length, 64);


            // Act - issue a new asset using a strongly typed AssetEntity class object
            RpcResponse<string> infAct_1 = await _wallet.IssueAsync(newAddress.Result, UUID.NoHyphens, 100, 1, 0,
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
            RpcResponse<string> infAct_2 = await _wallet.IssueAsync(newAddress.Result, UUID.NoHyphens, 100, 1, 0,
                new { text = "Dictionary test".ToHex(), description = "Even more text data".ToHex() });

            // Assert - proof issue response
            Assert.IsNull(infAct_2.Error);
            Assert.IsNotNull(infAct_2.Result);
            Assert.IsInstanceOf<string>(infAct_2.Result);
            Assert.GreaterOrEqual(infAct_2.Result.Length, 64);
        }
    }
}
