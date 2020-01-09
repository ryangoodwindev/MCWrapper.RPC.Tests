using MCWrapper.Data.Models.Network;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServicesPipeline;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class RpcNetworkClientTests
    {
        // Inject services
        private readonly IMultiChainRpcNetwork _network;
        private readonly string ChainName;

        // Create new RpcNetworkClientTests instance
        public RpcNetworkClientTests()
        {
            // instantiate mock services container
            var services = new ParameterlessStartup();

            // fetch service from service container
            _network = services.GetRequiredService<IMultiChainRpcNetwork>();

            ChainName = _network.RpcOptions.ChainName;
        }

        [Test, Ignore("AddNode test is ignored since I don't care about peers right now")]
        public async Task AddNodeTestAsync()
        {
            // Act - Add a peer
            var expAdd = await _network.AddNodeAsync(ChainName, UUID.NoHyphens, "192.168.0.90:3333", PeerConnection.Add);

            // Assert
            Assert.IsNull(expAdd.Error);
            Assert.IsNotNull(expAdd.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expAdd);

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
            // Act - Informatinon about added nodes
            var expNodeInfo = await _network.GetAddedNodeInfoAsync(ChainName, UUID.NoHyphens, true, "192.168.0.90:3333");

            // Assert
            Assert.IsNull(expNodeInfo.Error);
            Assert.IsNotNull(expNodeInfo.Result);
            Assert.IsInstanceOf<RpcResponse<GetAddNodeInfoResult[]>>(expNodeInfo);

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
            // Act - Fetch chunk queue information
            var expChunkQueue = await _network.GetChunkQueueInfoAsync(ChainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expChunkQueue.Error);
            Assert.IsNotNull(expChunkQueue.Result);
            Assert.IsInstanceOf<RpcResponse<GetChunkQueueInfoResult>>(expChunkQueue);

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
            // Act - Chunks delivery status
            var expChunkQueue = await _network.GetChunkQueueTotalsAsync(ChainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expChunkQueue.Error);
            Assert.IsNotNull(expChunkQueue.Result);
            Assert.IsInstanceOf<RpcResponse<GetChunkQueueInfoTotalsResult>>(expChunkQueue);

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
            // Act - Get number of connection to network
            var expConn = await _network.GetConnectionCountAsync(ChainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expConn.Error);
            Assert.IsNotNull(expConn.Result);
            Assert.IsInstanceOf<RpcResponse<int>>(expConn);

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
            // Act - Information about network traffic
            var expNetTotals = await _network.GetNetTotalsAsync(ChainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expNetTotals.Error);
            Assert.IsNotNull(expNetTotals.Result);
            Assert.IsInstanceOf<RpcResponse<GetNetTotalsResult>>(expNetTotals);

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
            // Act - Request information about the network
            var expInfo = await _network.GetNetworkInfoAsync(ChainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expInfo.Error);
            Assert.IsNotNull(expInfo.Result);
            Assert.IsInstanceOf<RpcResponse<GetNetworkInfoResult>>(expInfo);

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
            // Act - Request information about any connected peers
            var expPeer = await _network.GetPeerInfoAsync(ChainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expPeer.Error);
            Assert.IsNotNull(expPeer.Result);
            Assert.IsInstanceOf<RpcResponse<GetPeerInfoResult[]>>(expPeer);

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
            // Act - Ping connect peers
            var expPing = await _network.PingAsync(ChainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expPing.Error);
            Assert.IsNull(expPing.Result);
            Assert.IsInstanceOf<RpcResponse>(expPing);

            // Act - Ping connect peers
            var infPing = await _network.PingAsync();

            // Assert
            Assert.IsNull(infPing.Error);
            Assert.IsNull(infPing.Result);
            Assert.IsInstanceOf<RpcResponse>(infPing);
        }
    }
}
