using MCWrapper.Data.Models.Network;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServiceHelpers;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class NetworkRPCClientInferredTests
    {
        // private field
        private readonly NetworkRpcClient Network;

        /// <summary>
        /// Create new NetworkServiceTests instance
        /// </summary>
        public NetworkRPCClientInferredTests()
        {
            // instantiate test services provider
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch service from provider
            Network = provider.GetService<NetworkRpcClient>();
        }

        [Test, Ignore("AddNode test is ignored since I don't care about peers right now")]
        public async Task AddNodeTestAsync()
        {
            // Act - Add a peer
            var actual = await Network.AddNodeAsync(
                node: "192.168.0.90:3333",
                action: PeerConnection.Add);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test, Ignore("GetAddNodeInfo test is ignored since I don't care about peers right now")]
        public async Task GetAddNodeInfoTestAsync()
        {
            // Act - Informatinon about added nodes
            RpcResponse<GetAddNodeInfoResult[]> actual = await Network.GetAddedNodeInfoAsync(
                dns: true,
                node: "192.168.0.90:3333");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetAddNodeInfoResult[]>>(actual);
        }

        [Test]
        public async Task GetChunkQueueInfoTestAsync()
        {
            // Act - Fetch chunk queue information
            var actual = await Network.GetChunkQueueInfoAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetChunkQueueTotalsTestAsync()
        {
            // Act - Chunks delivery status
            var actual = await Network.GetChunkQueueTotalsAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetConnectionCountTestAsync()
        {
            // Act - Get number of connection to network
            var actual = await Network.GetConnectionCountAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetNetTotalsTestAsync()
        {
            // Act - Information about network traffic
            var actual = await Network.GetNetTotalsAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetNetworkInfoTestAsync()
        {
            // Act - Request information about the network
            RpcResponse<GetNetworkInfoResult> actual = await Network.GetNetworkInfoAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetNetworkInfoResult>>(actual);
        }

        [Test]
        public async Task GetPeerInfoTestAsync()
        {
            // Act - Request information about any connected peers
            var actual = await Network.GetPeerInfoAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task PingTestAsync()
        {
            // Act - Ping connect peers
            var actual = await Network.PingAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }
    }
}
