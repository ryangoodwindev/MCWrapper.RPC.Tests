using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Test.ServicesPipeline;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests.Tests
{
    [TestFixture]
    public class RpcWalletGrantTests
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
        public RpcWalletGrantTests()
        {
            _wallet = _services.GetRequiredService<IMultiChainRpcWallet>();
            _address = _wallet.RpcOptions.ChainAdminAddress;
            _chainName = _wallet.RpcOptions.ChainName;
        }

        [Test]
        public async Task GrantFromTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Stage
            var expNewAddress = await _wallet.GetNewAddressAsync(_chainName, UUID.NoHyphens);

            // Act
            var exp = await _wallet.GrantFromAsync(_chainName, UUID.NoHyphens, _address, expNewAddress.Result, Permission.Receive, 0, 1, 1000, "", "");

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(exp);

            /*
               Inferred blockchain name test
            */

            // Stage
            var newAddress = await _wallet.GetNewAddressAsync();

            // Act
            var actual = await _wallet.GrantFromAsync(_address, newAddress.Result, Permission.Receive, 0, 1, 1000, "", "");

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
            var expNewAddress = await _wallet.GetNewAddressAsync(_chainName, UUID.NoHyphens);

            // Act
            var exp = await _wallet.GrantAsync(_chainName, UUID.NoHyphens, expNewAddress.Result, Permission.Receive, 0, 1, 1000, "", "");

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(exp);

            /*
               Inferred blockchain name test
            */

            // Stage
            var infNewAddress = await _wallet.GetNewAddressAsync();

            // Act
            var inf = await _wallet.GrantAsync(infNewAddress.Result, Permission.Receive, 0, 1, 1000, "", "");

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(inf);
        }

        [Test]
        public async Task GrantWithDataFromTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Stage
            var expNewAddress = await _wallet.GetNewAddressAsync(_chainName, UUID.NoHyphens);

            // Act
            var exp = await _wallet.GrantWithDataFromAsync(_chainName, UUID.NoHyphens, _address, expNewAddress.Result, Permission.Receive, "some_data".ToHex(), 0, 1, 1000);

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(exp);

            /*
               Inferred blockchain name test
            */

            // Stage
            var infNewAddress = await _wallet.GetNewAddressAsync();

            // Act
            var inf = await _wallet.GrantWithDataFromAsync(_address, infNewAddress.Result, Permission.Receive, "some_data".ToHex(), 0, 1, 1000);

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(inf);
        }

        [Test]
        public async Task GrantWithDataTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Stage
            var expNewAddress = await _wallet.GetNewAddressAsync(_chainName, UUID.NoHyphens);

            // Act
            var exp = await _wallet.GrantWithDataAsync(_chainName, UUID.NoHyphens, expNewAddress.Result, Permission.Receive, "some_data".ToHex(), 0, 1, 1000);

            // Assert
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(exp);

            /*
               Inferred blockchain name test
            */

            // Stage
            var infNewAddress = await _wallet.GetNewAddressAsync();

            // Act
            var inf = await _wallet.GrantWithDataAsync(infNewAddress.Result, Permission.Receive, "some_data".ToHex(), 0, 1, 1000);

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(inf);
        }
    }
}
