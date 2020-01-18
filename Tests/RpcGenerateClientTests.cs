using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Test.ServicesPipeline;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Test.Generate
{
    [TestFixture]
    public class RpcGenerateClientTests
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

        // Inject services
        private readonly IMultiChainRpcGenerate _generate;
        private readonly string _chainName;

        // Use mock startup service container
        private readonly ExplicitStartup _services = new ExplicitStartup();

        // Create a new RpcGenerateClientTests instance
        public RpcGenerateClientTests()
        {
            _generate = _services.GetRequiredService<IMultiChainRpcGenerate>();
            _chainName = _generate.RpcOptions.ChainName;
        }

        [Test]
        public async Task GetGenerateTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expGet = await _generate.GetGenerateAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsTrue(expGet.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<bool>>(expGet);

            /*
              Inferred blockchain name test
           */

            // Act
            var infGet = await _generate.GetGenerateAsync();

            // Assert
            Assert.IsTrue(infGet.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<bool>>(infGet);
        }

        [Test]
        public async Task GetHashesPerSecTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expGet = await _generate.GetHashesPerSecAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsTrue(expGet.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<int>>(expGet);

            /*
              Inferred blockchain name test
           */

            // Act
            var infGet = await _generate.GetHashesPerSecAsync();

            // Assert
            Assert.IsTrue(infGet.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<int>>(infGet);
        }

        [Test, Ignore("SetGenerate tests should be ran independent of other tests")]
        public async Task SetGenerateTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expSet = await _generate.SetGenerateAsync(_chainName, UUID.NoHyphens, true, 1);

            // Assert
            Assert.IsTrue(expSet.IsSuccess());
            Assert.IsInstanceOf<RpcResponse>(expSet);

            /*
              Inferred blockchain name test
           */

            var infSet = await _generate.SetGenerateAsync(true, 1);

            // Assert
            Assert.IsTrue(infSet.IsSuccess());
            Assert.IsInstanceOf<RpcResponse>(infSet);
        }
    }
}