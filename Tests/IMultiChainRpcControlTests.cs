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
    public class IMultiChainRpcControlTests
    {
        // private field
        private readonly IMultiChainRpcControl _control;

        /// <summary>
        /// Create a new ControlServiceTests instance
        /// </summary>
        public IMultiChainRpcControlTests()
        {
            // instantiate new test services provider
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch service from provider
            _control = provider.GetService<IMultiChainRpcControl>();
        }

        [Test, Ignore("ClearMemPoolTests should be ran independent of other tests since the network must be paused for incoming and mining tasks")]
        public async Task ClearMemPoolExplicitTestAsync()
        {
            // Act - Pause blockchain network actions
            var pause = await _control.PauseAsync(
                blockchainName: _control.RpcOptions.ChainName,
                id: nameof(ClearMemPoolExplicitTestAsync),
                tasks: NodeTask.All);

            // Assert
            Assert.IsNull(pause.Error);
            Assert.IsNotNull(pause.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(pause);

            // Act - Clear blockchain mem pool
            var clearMemPool = await _control.ClearMemPoolAsync(_control.RpcOptions.ChainName, nameof(ClearMemPoolExplicitTestAsync));

            // Assert
            Assert.IsNull(pause.Error);
            Assert.IsNotNull(pause.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(clearMemPool);

            // Act - Resume blockchain network actions
            var resume = await _control.ResumeAsync(
                blockchainName: _control.RpcOptions.ChainName,
                id: nameof(ClearMemPoolExplicitTestAsync),
                tasks: NodeTask.All);

            // Assert
            Assert.IsNull(pause.Error);
            Assert.IsNotNull(pause.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(resume);
        }

        [Test]
        public async Task GetBlockchainParamsExplicitTestAsync()
        {
            // Act - Ask network for blockchain params
            var actual = await _control.GetBlockchainParamsAsync(
                blockchainName: _control.RpcOptions.ChainName,
                id: nameof(GetBlockchainParamsExplicitTestAsync), 
                display_names: true,
                with_upgrades: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockchainParamsResult>>(actual);
        }

        [Test]
        public async Task GetInfoExplicitTestAsync()
        {
            // Act - Ask network for information about this blockchain
            var actual = await _control.GetInfoAsync(_control.RpcOptions.ChainName, nameof(GetInfoExplicitTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetInfoResult>>(actual);
        }

        [Test]
        public async Task GetInitStatusExplicitTestAsync()
        {
            // Act - Ask netowrk for init status
            var actual = await _control.GetInitStatusAsync(_control.RpcOptions.ChainName, nameof(GetInitStatusExplicitTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetInitStatusResult>>(actual);
        }

        [Test]
        public async Task GetRuntimeParamsExplicitTestAsync()
        {
            // Act - Ask blockchain network for runtime parameters
            var actual = await _control.GetRuntimeParamsAsync(_control.RpcOptions.ChainName, nameof(GetRuntimeParamsExplicitTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetRuntimeParamsResult>>(actual);
        }

        [Test]
        public async Task HelpExplicitTestAsync()
        {
            // Act - Get help information based on blockchain method name
            var actual = await _control.HelpAsync(
                blockchainName: _control.RpcOptions.ChainName,
                id: nameof(HelpExplicitTestAsync),
                command: BlockchainAction.GetAssetInfoMethod);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test, Ignore("Test is ignored since it can be destructive to the current blockchain")]
        public async Task SetLastBlockExplicitTestAsync()
        {
            // Act - Sets last block in blockchain
            var actual = await _control.SetLastBlockAsync(
                blockchainName: _control.RpcOptions.ChainName,
                id: nameof(SetLastBlockExplicitTestAsync),
                hash_or_height: 60);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task SetRuntimeParamExplicitTestAsync()
        {
            // Stage - One mebibyte
            var OneMiB = 1048576;

            // ### Act - Set a specific runtime parameter with a specific value
            var actual = await _control.SetRuntimeParamAsync(
                blockchainName: _control.RpcOptions.ChainName,
                id: nameof(SetRuntimeParamExplicitTestAsync),
                runtimeParam: RuntimeParam.MaxShownData,
                parameter_value: OneMiB);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Test is ignored since it can be destructive to the current blockchain")]
        public async Task StopExplicitTestAsync()
        {
            // Act - Stops the current blockchain network
            var actual = await _control.StopAsync(_control.RpcOptions.ChainName, nameof(StopExplicitTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        // Inferred blockchainName tests //

        [Test, Ignore("ClearMemPoolTests should be ran independent of other tests since the network must be paused for incoming and mining tasks")]
        public async Task ClearMemPoolInferredTestAsync()
        {
            // Act - Pause blockchain network actions
            var pause = await _control.PauseAsync(tasks: NodeTask.All);

            // Assert
            Assert.IsNull(pause.Error);
            Assert.IsNotNull(pause.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(pause);

            // Act - Clear blockchain mem pool
            var clearMemPool = await _control.ClearMemPoolAsync();

            // Assert
            Assert.IsNull(pause.Error);
            Assert.IsNotNull(pause.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(clearMemPool);

            // Act - Resume blockchain network actions
            var resume = await _control.ResumeAsync(tasks: NodeTask.All);

            // Assert
            Assert.IsNull(pause.Error);
            Assert.IsNotNull(pause.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(resume);
        }

        [Test]
        public async Task GetBlockchainParamsInferredTestAsync()
        {
            // Act - Ask network for blockchain params
            var actual = await _control.GetBlockchainParamsAsync(
                display_names: true,
                with_upgrades: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockchainParamsResult>>(actual);
        }

        [Test]
        public async Task GetInfoInferredTestAsync()
        {
            // Act - Ask network for information about this blockchain
            var actual = await _control.GetInfoAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetInfoResult>>(actual);
        }

        [Test]
        public async Task GetInitStatusInferredTestAsync()
        {
            // Act - Ask netowrk for init status
            var actual = await _control.GetInitStatusAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetInitStatusResult>>(actual);
        }

        [Test]
        public async Task GetRuntimeParamsInferredTestAsync()
        {
            // Act - Ask blockchain network for runtime parameters
            var actual = await _control.GetRuntimeParamsAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetRuntimeParamsResult>>(actual);
        }

        [Test]
        public async Task HelpInferredTestAsync()
        {
            // Act - Get help information based on blockchain method name
            var actual = await _control.HelpAsync(command: BlockchainAction.GetAssetInfoMethod);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test, Ignore("Test is ignored since it can be destructive to the current blockchain")]
        public async Task SetLastBlockInferredTestAsync()
        {
            // Act - Sets last block in blockchain
            var actual = await _control.SetLastBlockAsync(hash_or_height: 60);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task SetRuntimeParamInferredTestAsync()
        {
            // Stage - One mebibyte
            var OneMiB = 1048576;

            // ### Act - Set a specific runtime parameter with a specific value
            var actual = await _control.SetRuntimeParamAsync(
                runtimeParam: RuntimeParam.MaxShownData,
                parameter_value: OneMiB);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("Test is ignored since it can be destructive to the current blockchain")]
        public async Task StopInferredTestAsync()
        {
            // Act - Stops the current blockchain network
            var actual = await _control.StopAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }
    }
}
