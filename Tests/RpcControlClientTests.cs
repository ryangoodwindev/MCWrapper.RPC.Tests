using MCWrapper.Data.Models.Control;
using MCWrapper.Ledger.Actions;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Test.ServicesPipeline;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Test.Control
{
    [TestFixture]
    public class RpcControlClientTests
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
        private readonly IMultiChainRpcControl _control;
        private readonly string _chainName;

        // Use mock startup service container
        private readonly ExplicitStartup _services = new ExplicitStartup();

        // Create a new RpcControlClientTests instance
        public RpcControlClientTests()
        {
            _control = _services.GetRequiredService<IMultiChainRpcControl>();
            _chainName = _control.RpcOptions.ChainName;
        }

        [Test]
        public async Task GetBlockchainParamsTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Ask network for blockchain params
            var expParams = await _control.GetBlockchainParamsAsync(_chainName, UUID.NoHyphens, true, true);

            // Assert
            Assert.IsNull(expParams.Error);
            Assert.IsNotNull(expParams.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockchainParamsResult>>(expParams);

            /*
               Inferred blockchain name test
            */

            // Act - Ask network for blockchain params
            var infParams = await _control.GetBlockchainParamsAsync(true, true);

            // Assert
            Assert.IsNull(infParams.Error);
            Assert.IsNotNull(infParams.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockchainParamsResult>>(infParams);
        }

        [Test]
        public async Task GetInfoTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Ask network for information about this blockchain
            var expInfo = await _control.GetInfoAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expInfo.Error);
            Assert.IsNotNull(expInfo.Result);
            Assert.IsInstanceOf<RpcResponse<GetInfoResult>>(expInfo);

            /*
               Inferred blockchain name test
            */

            // Act - Ask network for information about this blockchain
            var infInfo = await _control.GetInfoAsync();

            // Assert
            Assert.IsNull(infInfo.Error);
            Assert.IsNotNull(infInfo.Result);
            Assert.IsInstanceOf<RpcResponse<GetInfoResult>>(infInfo);
        }

        [Test]
        public async Task GetInitStatusTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Ask netowrk for init status
            var expInit = await _control.GetInitStatusAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expInit.Error);
            Assert.IsNotNull(expInit.Result);
            Assert.IsInstanceOf<RpcResponse<GetInitStatusResult>>(expInit);

            /*
               Inferred blockchain name test
            */

            // Act - Ask netowrk for init status
            var infInit = await _control.GetInitStatusAsync();

            // Assert
            Assert.IsNull(infInit.Error);
            Assert.IsNotNull(infInit.Result);
            Assert.IsInstanceOf<RpcResponse<GetInitStatusResult>>(infInit);
        }

        [Test]
        public async Task GetRuntimeParamsTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Ask blockchain network for runtime parameters
            var expParams = await _control.GetRuntimeParamsAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expParams.Error);
            Assert.IsNotNull(expParams.Result);
            Assert.IsInstanceOf<RpcResponse<GetRuntimeParamsResult>>(expParams);

            /*
               Inferred blockchain name test
            */

            // Act - Ask blockchain network for runtime parameters
            var infParams = await _control.GetRuntimeParamsAsync();

            // Assert
            Assert.IsNull(infParams.Error);
            Assert.IsNotNull(infParams.Result);
            Assert.IsInstanceOf<RpcResponse<GetRuntimeParamsResult>>(infParams);
        }

        [Test]
        public async Task HelpTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Get help information based on blockchain method name
            var expHelp = await _control.HelpAsync(_chainName, UUID.NoHyphens, BlockchainAction.GetAssetInfoMethod);

            // Assert
            Assert.IsNull(expHelp.Error);
            Assert.IsNotNull(expHelp.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expHelp);

            /*
               Inferred blockchain name test
            */

            // Act - Get help information based on blockchain method name
            var infHelp = await _control.HelpAsync(BlockchainAction.GetAssetInfoMethod);

            // Assert
            Assert.IsNull(infHelp.Error);
            Assert.IsNotNull(infHelp.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infHelp);
        }

        [Test]
        public async Task SetRuntimeParamTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // ### Act - Set a specific runtime parameter with a specific value
            var expSet = await _control.SetRuntimeParamAsync(_chainName, UUID.NoHyphens, RuntimeParam.MaxShownData, 1048576);

            // Assert
            Assert.IsNull(expSet.Error);
            Assert.IsNull(expSet.Result);
            Assert.IsInstanceOf<RpcResponse>(expSet);

            /*
               Inferred blockchain name test
            */

            // ### Act - Set a specific runtime parameter with a specific value
            var infSet = await _control.SetRuntimeParamAsync(RuntimeParam.MaxShownData, 1048576);

            // Assert
            Assert.IsNull(infSet.Error);
            Assert.IsNull(infSet.Result);
            Assert.IsInstanceOf<RpcResponse>(infSet);
        }

        [Test, Ignore("ClearMemPoolTests should be ran independent of other tests since the network must be paused for incoming and mining tasks")]
        public async Task ClearMemPoolTestAsync()
        {
            /*
                Explicit blockchain name tests
             */

            // Act - Pause blockchain network actions
            var expPause = await _control.PauseAsync(_chainName, UUID.NoHyphens, NodeTask.All);

            // Assert
            Assert.IsNull(expPause.Error);
            Assert.IsNotNull(expPause.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expPause);

            // Act - Clear blockchain mem pool
            var expClearMemPool = await _control.ClearMemPoolAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expClearMemPool.Error);
            Assert.IsNotNull(expClearMemPool.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expClearMemPool);

            // Act - Resume blockchain network actions
            var expResume = await _control.ResumeAsync(_chainName, UUID.NoHyphens, NodeTask.All);

            // Assert
            Assert.IsNull(expResume.Error);
            Assert.IsNotNull(expResume.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expResume);

            /*
                Inferred blockchain name tests
             */

            // Act - Pause blockchain network actions
            var infPause = await _control.PauseAsync(tasks: NodeTask.All);

            // Assert
            Assert.IsNull(infPause.Error);
            Assert.IsNotNull(infPause.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infPause);

            // Act - Clear blockchain mem pool
            var infClearMemPool = await _control.ClearMemPoolAsync();

            // Assert
            Assert.IsNull(infClearMemPool.Error);
            Assert.IsNotNull(infClearMemPool.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infClearMemPool);

            // Act - Resume blockchain network actions
            var infResume = await _control.ResumeAsync(tasks: NodeTask.All);

            // Assert
            Assert.IsNull(infResume.Error);
            Assert.IsNotNull(infResume.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infResume);
        }

        [Test, Ignore("Test is ignored since it can be destructive to the target blockchain")]
        public async Task SetLastBlockExplicitTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Sets last block in blockchain
            await _control.PauseAsync(_chainName, UUID.NoHyphens);
            var expSet = await _control.SetLastBlockAsync(_chainName, UUID.NoHyphens, "Enter a block hash or height index");
            await _control.ResumeAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expSet.Error);
            Assert.IsNotNull(expSet.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expSet);

            /*
               Inferred blockchain name test
            */

            // Act - Sets last block in blockchain
            await _control.PauseAsync();
            var infSet = await _control.SetLastBlockAsync("Enter a block hash or height index");
            await _control.ResumeAsync();

            // Assert
            Assert.IsNull(infSet.Error);
            Assert.IsNotNull(infSet.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infSet);
        }

        [Test, Ignore("Test is ignored since it can be destructive to the target blockchain")]
        public async Task StopTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Stops the current blockchain network
            var expStop = await _control.StopAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expStop.Error);
            Assert.IsNotNull(expStop.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expStop);

            /*
               Inferred blockchain name test - Test is commented out since
               we cannot call the stop method twice in a row due the blockchain node having already shutdown
            */

            // Act - Stops the current blockchain network
            // var infStop = await _control.StopAsync();

            // Assert
            // Assert.IsNull(infStop.Error);
            // Assert.IsNotNull(infStop.Result);
            // Assert.IsInstanceOf<RpcResponse<string>>(infStop);
        }
    }
}