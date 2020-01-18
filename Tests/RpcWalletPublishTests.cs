using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Test.ServicesPipeline;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Test.Publish
{
    [TestFixture]
    public class RpcWalletPublishTests
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

        public RpcWalletPublishTests()
        {
            _wallet = _services.GetRequiredService<IMultiChainRpcWallet>();
            _address = _wallet.RpcOptions.ChainAdminAddress;
            _chainName = _wallet.RpcOptions.ChainName;
        }

        [Test]
        public async Task PublishTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act
            var exp = await _wallet.PublishAsync(_chainName, UUID.NoHyphens, "root", "test_key", "some_data".ToHex(), "offchain");

            // Assert
            Assert.IsTrue(exp.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.PublishAsync("root", "test_key", "some_data".ToHex(), "offchain");

            // Assert
            Assert.IsTrue(inf.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(inf);
        }

        [Test]
        public async Task PublishFromTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act
            var exp = await _wallet.PublishFromAsync(_chainName, UUID.NoHyphens, _address, "root", "test_key", "some_data".ToHex(), "offchain");

            // Assert
            Assert.IsTrue(exp.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.PublishFromAsync(_address, "root", "test_key", "some_data".ToHex(), "offchain");

            // Assert
            Assert.IsTrue(inf.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(inf);
        }

        [Test]
        public async Task PublishMultiTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act
            var exp = await _wallet.PublishMultiAsync(_chainName, UUID.NoHyphens, "root", new object[] { new { key = "some_key", data = "some_data".ToHex() } }, "offchain");

            // Assert
            Assert.IsTrue(exp.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.PublishMultiAsync("root", new object[] { new { key = "some_key", data = "some_data".ToHex() } }, "offchain");

            // Assert
            Assert.IsTrue(inf.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(inf);
        }

        [Test]
        public async Task PublishMultiFromTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act
            var exp = await _wallet.PublishMultiFromAsync(_chainName, UUID.NoHyphens, _address, "root", new object[] { new { key = "some_key", data = "some_data".ToHex() } }, "offchain");

            // Assert
            Assert.IsTrue(exp.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.PublishMultiFromAsync(_address, "root", new object[] { new { key = "some_key", data = "some_data".ToHex() } }, "offchain");

            // Assert
            Assert.IsTrue(inf.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(inf);
        }
    }
}