﻿using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServiceHelpers;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class GenerateRPCClientExplicitTests
    {
        // private field
        private readonly IMultiChainRpcGenerate _generate;

        /// <summary>
        /// Create a new GenerateServiceTests instance
        /// </summary>
        public GenerateRPCClientExplicitTests()
        {
            // instantiate test services provider
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch service from provider
            _generate = provider.GetService<IMultiChainRpcGenerate>();
        }

        [Test]
        public async Task GetGenerateTestAsync()
        {
            // Act - Get coin generation status
            var actual = await _generate.GetGenerateAsync(blockchainName: _generate.RpcOptions.ChainName, id: nameof(GetGenerateTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(actual);
        }

        [Test]
        public async Task GetHashesPerSecTestAsync()
        {
            // Act - Fetch hashes per sec statistic
            var actual = await _generate.GetHashesPerSecAsync(blockchainName: _generate.RpcOptions.ChainName, id: nameof(GetHashesPerSecTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<int>>(actual);
        }

        [Test, Ignore("SetGenerate tests should be ran independent of other tests")]
        public async Task SetGenerateTestAsync()
        {
            // Act - Set coin generation status
            var actual = await _generate.SetGenerateAsync(
                blockchainName: _generate.RpcOptions.ChainName,
                id: nameof(SetGenerateTestAsync),
                generate: true,
                gen_proc_limit: 1);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }
    }
}
