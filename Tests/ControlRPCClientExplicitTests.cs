using MCWrapper.Data.Models.Control;
using MCWrapper.Ledger.Actions;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServiceHelpers;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class ControlRPCClientExplicitTests
    {
        // private field
        private readonly ControlRpcClient Control;

        /// <summary>
        /// Create a new ControlServiceTests instance
        /// </summary>
        public ControlRPCClientExplicitTests()
        {
            // instantiate new test services provider
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch service from provider
            Control = provider.GetService<ControlRpcClient>();
        }

        [Test, Ignore("ClearMemPoolTests should be ran independent of other tests since the network must be paused for incoming and mining tasks")]
        public async Task ClearMemPoolTestAsync()
        {
            // Act - Pause blockchain network actions
            var pause = await Control.PauseAsync(
                blockchainName: Control.BlockchainOptions.ChainName,
                id: nameof(ClearMemPoolTestAsync),
                tasks: NodeTask.All);

            // Assert
            Assert.IsNull(pause.Error);
            Assert.IsNotNull(pause.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(pause);

            // Act - Clear blockchain mem pool
            var clearMemPool = await Control.ClearMemPoolAsync(Control.BlockchainOptions.ChainName, nameof(ClearMemPoolTestAsync));

            // Assert
            Assert.IsNull(pause.Error);
            Assert.IsNotNull(pause.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(clearMemPool);

            // Act - Resume blockchain network actions
            var resume = await Control.ResumeAsync(
                blockchainName: Control.BlockchainOptions.ChainName,
                id: nameof(ClearMemPoolTestAsync),
                tasks: NodeTask.All);

            // Assert
            Assert.IsNull(pause.Error);
            Assert.IsNotNull(pause.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(resume);
        }

        [Test]
        public async Task GetBlockchainParamsTestAsync()
        {
            // Act - Ask network for blockchain params
            var actual = await Control.GetBlockchainParamsAsync(
                blockchainName: Control.BlockchainOptions.ChainName,
                id: nameof(GetBlockchainParamsTestAsync), 
                display_names: true,
                with_upgrades: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockchainParamsResult>>(actual);
        }

        [Test]
        public async Task GetInfoTestAsync()
        {
            // Act - Ask network for information about this blockchain
            var actual = await Control.GetInfoAsync(Control.BlockchainOptions.ChainName, nameof(GetInfoTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetInfoResult>>(actual);
        }

        [Test]
        public async Task GetRuntimeParamsTestAsync()
        {
            // Act - Ask blockchain network for runtime parameters
            var actual = await Control.GetRuntimeParamsAsync(Control.BlockchainOptions.ChainName, nameof(GetRuntimeParamsTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetRuntimeParamsResult>>(actual);
        }

        [Test]
        public async Task HelpTestAsync()
        {
            // Act - Get help information based on blockchain method name
            var actual = await Control.HelpAsync(
                blockchainName: Control.BlockchainOptions.ChainName,
                id: nameof(HelpTestAsync),
                command: BlockchainAction.GetAssetInfoMethod);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Test is ignored since it can be destructive to the current blockchain")]
        public async Task SetLastBlockTestAsync()
        {
            // Act - Sets last block in blockchain
            var actual = await Control.SetLastBlockAsync(
                blockchainName: Control.BlockchainOptions.ChainName,
                id: nameof(SetLastBlockTestAsync),
                hash_or_height: 60);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SetRuntimeParamTestAsync()
        {
            // Stage - One mebibyte
            var OneMiB = 1048576;

            // ### Act - Set a specific runtime parameter with a specific value
            var actual = await Control.SetRuntimeParamAsync(
                blockchainName: Control.BlockchainOptions.ChainName,
                id: nameof(SetRuntimeParamTestAsync),
                runtimeParam: RuntimeParam.MaxShownData,
                parameter_value: OneMiB);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Test is ignored since it can be destructive to the current blockchain")]
        public async Task StopTestAsync()
        {
            // Act - Stops the current blockchain network
            var actual = await Control.StopAsync(Control.BlockchainOptions.ChainName, nameof(StopTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }
    }
}
