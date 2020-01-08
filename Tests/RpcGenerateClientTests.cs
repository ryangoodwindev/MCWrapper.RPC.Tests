using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServicesPipeline;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class RpcGenerateClientTests
    {
        // Inject services
        private readonly IMultiChainRpcGenerate _generate;
        private readonly string ChainName;

        // Create a new RpcGenerateClientTests instance
        public RpcGenerateClientTests()
        {
            // instantiate mock services container
            var services = new ParameterlessMockServices();

            // fetch service from service container
            _generate = services.GetRequiredService<IMultiChainRpcGenerate>();

            ChainName = _generate.RpcOptions.ChainName;
        }

        [Test]
        public async Task GetGenerateTestAsync()
        {
            // Act
            var expGet = await _generate.GetGenerateAsync(ChainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expGet.Error);
            Assert.IsNotNull(expGet.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(expGet);

            // Act
            var infGet = await _generate.GetGenerateAsync();

            // Assert
            Assert.IsNull(infGet.Error);
            Assert.IsNotNull(infGet.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(infGet);
        }

        [Test]
        public async Task GetHashesPerSecTestAsync()
        {
            // Act
            var expGet = await _generate.GetHashesPerSecAsync(ChainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expGet.Error);
            Assert.IsNotNull(expGet.Result);
            Assert.IsInstanceOf<RpcResponse<int>>(expGet);

            // Act
            var infGet = await _generate.GetHashesPerSecAsync();

            // Assert
            Assert.IsNull(infGet.Error);
            Assert.IsNotNull(infGet.Result);
            Assert.IsInstanceOf<RpcResponse<int>>(infGet);
        }

        [Test, Ignore("SetGenerate tests should be ran independent of other tests")]
        public async Task SetGenerateTestAsync()
        {
            // Act
            var expSet = await _generate.SetGenerateAsync(ChainName, UUID.NoHyphens, true, 1);

            // Assert
            Assert.IsNull(expSet.Error);
            Assert.IsInstanceOf<object>(expSet.Result);
            Assert.IsInstanceOf<RpcResponse>(expSet);

            var infSet = await _generate.SetGenerateAsync(true, 1);

            // Assert
            Assert.IsNull(infSet.Error);
            Assert.IsInstanceOf<object>(infSet.Result);
            Assert.IsInstanceOf<RpcResponse>(infSet);
        }
    }
}
