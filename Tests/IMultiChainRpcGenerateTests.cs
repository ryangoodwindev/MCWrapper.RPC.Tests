using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServiceHelpers;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class IMultiChainRpcGenerateTests
    {
        // private field
        private readonly IMultiChainRpcGenerate _generate;

        /// <summary>
        /// Create a new GenerateServiceTests instance
        /// </summary>
        public IMultiChainRpcGenerateTests()
        {
            // instantiate test services provider
            var provider = new ParameterlessMockServices();

            // fetch service from provider
            _generate = provider.GetService<IMultiChainRpcGenerate>();
        }

        [Test]
        public async Task GetGenerateExplicitTestAsync()
        {
            // Act - Get coin generation status
            var actual = await _generate.GetGenerateAsync(_generate.RpcOptions.ChainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(actual);
        }

        [Test]
        public async Task GetHashesPerSecExplicitTestAsync()
        {
            // Act - Fetch hashes per sec statistic
            var actual = await _generate.GetHashesPerSecAsync(_generate.RpcOptions.ChainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<int>>(actual);
        }

        [Test, Ignore("SetGenerate tests should be ran independent of other tests")]
        public async Task SetGenerateExplicitTestAsync()
        {
            // Act - Set coin generation status
            await _generate.SetGenerateAsync(_generate.RpcOptions.ChainName, UUID.NoHyphens, true, 1);
        }

        // Inferred blockchainName tests //

        [Test]
        public async Task GetGenerateInferredTestAsync()
        {
            // Act - Get coin generation status
            var actual = await _generate.GetGenerateAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(actual);
        }

        [Test]
        public async Task GetHashesPerSecInferredTestAsync()
        {
            // Act - Fetch hashes per sec statistic
            var actual = await _generate.GetHashesPerSecAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<int>>(actual);
        }

        [Test, Ignore("SetGenerate tests should be ran independent of other tests")]
        public async Task SetGenerateInferredTestAsync()
        {
            // Act - Set coin generation status
            await _generate.SetGenerateAsync(true, 1);
        }
    }
}
