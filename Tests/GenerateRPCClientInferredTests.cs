using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServiceHelpers;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class GenerateRPCClientInferredTests
    {
        // private field
        private readonly GenerateRpcClient Generate;

        /// <summary>
        /// Create a new GenerateServiceTests instance
        /// </summary>
        public GenerateRPCClientInferredTests()
        {
            // instantiate test services provider
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch service from provider
            Generate = provider.GetService<GenerateRpcClient>();
        }

        [Test]
        public async Task GetGenerateTestAsync()
        {
            // Act - Get coin generation status
            var actual = await Generate.GetGenerateAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(actual);
        }

        [Test]
        public async Task GetHashesPerSecTestAsync()
        {
            // Act - Fetch hashes per sec statistic
            var actual = await Generate.GetHashesPerSecAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<int>>(actual);
        }

        [Test, Ignore("SetGenerate tests should be ran independent of other tests")]
        public async Task SetGenerateTestAsync()
        {
            // Act - Set coin generation status
            var actual = await Generate.SetGenerateAsync(generate: true, gen_proc_limit: 1);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }
    }
}
