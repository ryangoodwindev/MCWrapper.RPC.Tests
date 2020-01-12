using MCWrapper.Data.Models.Network;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Test.ServicesPipeline;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Test.Network
{
    [TestFixture]
    public class RpcNetworkClientTests
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
        private readonly IMultiChainRpcNetwork _network;
        private readonly string _chainName;

        // Use mock startup service container
        private readonly ExplicitStartup _services = new ExplicitStartup();

        // Create new RpcNetworkClientTests instance
        public RpcNetworkClientTests()
        {
            _network = _services.GetRequiredService<IMultiChainRpcNetwork>();
            _chainName = _network.RpcOptions.ChainName;
        }

        [Test, Ignore("AddNode test is ignored since I don't care about peers right now")]
        public async Task AddNodeTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Add a peer
            var expAdd = await _network.AddNodeAsync(_chainName, UUID.NoHyphens, "192.168.0.90:3333", PeerConnection.Add);

            // Assert
            Assert.IsNull(expAdd.Error);
            Assert.IsNotNull(expAdd.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expAdd);

            /*
               Inferred blockchain name test
            */

            // Act - Add a peer
            var infAdd = await _network.AddNodeAsync("192.168.0.90:3333", PeerConnection.Add);

            // Assert
            Assert.IsNull(infAdd.Error);
            Assert.IsNotNull(infAdd.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infAdd);
        }

        [Test, Ignore("GetAddNodeInfo test is ignored since I don't care about peers right now")]
        public async Task GetAddNodeInfoTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Informatinon about added nodes
            var expNodeInfo = await _network.GetAddedNodeInfoAsync(_chainName, UUID.NoHyphens, true, "192.168.0.90:3333");

            // Assert
            Assert.IsNull(expNodeInfo.Error);
            Assert.IsNotNull(expNodeInfo.Result);
            Assert.IsInstanceOf<RpcResponse<GetAddNodeInfoResult[]>>(expNodeInfo);

            /*
               Inferred blockchain name test
            */

            // Act - Informatinon about added nodes
            var infNodeInfo = await _network.GetAddedNodeInfoAsync(true, "192.168.0.90:3333");

            // Assert
            Assert.IsNull(infNodeInfo.Error);
            Assert.IsNotNull(infNodeInfo.Result);
            Assert.IsInstanceOf<RpcResponse<GetAddNodeInfoResult[]>>(infNodeInfo);
        }

        [Test]
        public async Task GetChunkQueueInfoTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Fetch chunk queue information
            var expChunkQueue = await _network.GetChunkQueueInfoAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expChunkQueue.Error);
            Assert.IsNotNull(expChunkQueue.Result);
            Assert.IsInstanceOf<RpcResponse<GetChunkQueueInfoResult>>(expChunkQueue);

            /*
               Inferred blockchain name test
            */

            // Act - Fetch chunk queue information
            var infChunkQueue = await _network.GetChunkQueueInfoAsync();

            // Assert
            Assert.IsNull(infChunkQueue.Error);
            Assert.IsNotNull(infChunkQueue.Result);
            Assert.IsInstanceOf<RpcResponse<GetChunkQueueInfoResult>>(infChunkQueue);
        }

        [Test]
        public async Task GetChunkQueueTotalsTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Chunks delivery status
            var expChunkQueue = await _network.GetChunkQueueTotalsAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expChunkQueue.Error);
            Assert.IsNotNull(expChunkQueue.Result);
            Assert.IsInstanceOf<RpcResponse<GetChunkQueueInfoTotalsResult>>(expChunkQueue);

            /*
               Inferred blockchain name test
            */

            // Act - Chunks delivery status
            var infChunkQueue = await _network.GetChunkQueueTotalsAsync();

            // Assert
            Assert.IsNull(infChunkQueue.Error);
            Assert.IsNotNull(infChunkQueue.Result);
            Assert.IsInstanceOf<RpcResponse<GetChunkQueueInfoTotalsResult>>(infChunkQueue);
        }

        [Test]
        public async Task GetConnectionCountTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Get number of connection to network
            var expConn = await _network.GetConnectionCountAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expConn.Error);
            Assert.IsNotNull(expConn.Result);
            Assert.IsInstanceOf<RpcResponse<int>>(expConn);

            /*
               Inferred blockchain name test
            */

            // Act - Get number of connection to network
            var infConn = await _network.GetConnectionCountAsync();

            // Assert
            Assert.IsNull(infConn.Error);
            Assert.IsNotNull(infConn.Result);
            Assert.IsInstanceOf<RpcResponse<int>>(infConn);
        }

        [Test]
        public async Task GetNetTotalsTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Information about network traffic
            var expNetTotals = await _network.GetNetTotalsAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expNetTotals.Error);
            Assert.IsNotNull(expNetTotals.Result);
            Assert.IsInstanceOf<RpcResponse<GetNetTotalsResult>>(expNetTotals);

            /*
               Inferred blockchain name test
            */

            // Act - Information about network traffic
            var infNetTotals = await _network.GetNetTotalsAsync();

            // Assert
            Assert.IsNull(infNetTotals.Error);
            Assert.IsNotNull(infNetTotals.Result);
            Assert.IsInstanceOf<RpcResponse<GetNetTotalsResult>>(infNetTotals);
        }

        [Test]
        public async Task GetNetworkInfoTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Request information about the network
            var expInfo = await _network.GetNetworkInfoAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expInfo.Error);
            Assert.IsNotNull(expInfo.Result);
            Assert.IsInstanceOf<RpcResponse<GetNetworkInfoResult>>(expInfo);

            /*
               Inferred blockchain name test
            */

            // Act - Request information about the network
            var infInfo = await _network.GetNetworkInfoAsync();

            // Assert
            Assert.IsNull(infInfo.Error);
            Assert.IsNotNull(infInfo.Result);
            Assert.IsInstanceOf<RpcResponse<GetNetworkInfoResult>>(infInfo);
        }

        [Test]
        public async Task GetPeerInfoTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Request information about any connected peers
            var expPeer = await _network.GetPeerInfoAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expPeer.Error);
            Assert.IsNotNull(expPeer.Result);
            Assert.IsInstanceOf<RpcResponse<GetPeerInfoResult[]>>(expPeer);

            /*
               Inferred blockchain name test
            */

            // Act - Request information about any connected peers
            var infPeer = await _network.GetPeerInfoAsync();

            // Assert
            Assert.IsNull(infPeer.Error);
            Assert.IsNotNull(infPeer.Result);
            Assert.IsInstanceOf<RpcResponse<GetPeerInfoResult[]>>(infPeer);
        }

        [Test]
        public async Task PingTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Ping connect peers
            var expPing = await _network.PingAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expPing.Error);
            Assert.IsNull(expPing.Result);
            Assert.IsInstanceOf<RpcResponse>(expPing);

            /*
               Inferred blockchain name test
            */

            // Act - Ping connect peers
            var infPing = await _network.PingAsync();

            // Assert
            Assert.IsNull(infPing.Error);
            Assert.IsNull(infPing.Result);
            Assert.IsInstanceOf<RpcResponse>(infPing);
        }
    }
}
