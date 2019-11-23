﻿using MCWrapper.Data.Models.Network;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServiceHelpers;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class NetworkRPCClientExplicitTests
    {
        // private field
        private readonly IMultiChainRpcNetwork _network;

        /// <summary>
        /// Create new NetworkServiceTests instance
        /// </summary>
        public NetworkRPCClientExplicitTests()
        {
            // instantiate test services provider
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch service from provider
            _network = provider.GetService<IMultiChainRpcNetwork>();
        }

        [Test, Ignore("AddNode test is ignored since I don't care about peers right now")]
        public async Task AddNodeTestAsync()
        {
            // Act - Add a peer
            var actual = await _network.AddNodeAsync(
                blockchainName: _network.RpcOptions.ChainName,
                id: nameof(AddNodeTestAsync),
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
            RpcResponse<GetAddNodeInfoResult[]> actual = await _network.GetAddedNodeInfoAsync(
                blockchainName: _network.RpcOptions.ChainName,
                id: nameof(GetAddNodeInfoTestAsync),
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
            var actual = await _network.GetChunkQueueInfoAsync(_network.RpcOptions.ChainName, nameof(GetChunkQueueInfoTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetChunkQueueTotalsTestAsync()
        {
            // Act - Chunks delivery status
            var actual = await _network.GetChunkQueueTotalsAsync(_network.RpcOptions.ChainName, nameof(GetChunkQueueTotalsTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetConnectionCountTestAsync()
        {
            // Act - Get number of connection to network
            var actual = await _network.GetConnectionCountAsync(_network.RpcOptions.ChainName, nameof(GetConnectionCountTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetNetTotalsTestAsync()
        {
            // Act - Information about network traffic
            var actual = await _network.GetNetTotalsAsync(_network.RpcOptions.ChainName, nameof(GetNetTotalsTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task GetNetworkInfoTestAsync()
        {
            // Act - Request information about the network
            RpcResponse<GetNetworkInfoResult> actual = await _network.GetNetworkInfoAsync(_network.RpcOptions.ChainName, nameof(GetNetworkInfoTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetNetworkInfoResult>>(actual);
        }

        [Test]
        public async Task GetPeerInfoTestAsync()
        {
            // Act - Request information about any connected peers
            var actual = await _network.GetPeerInfoAsync(_network.RpcOptions.ChainName, nameof(GetPeerInfoTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task PingTestAsync()
        {
            // Act - Ping connect peers
            var actual = await _network.PingAsync(_network.RpcOptions.ChainName, nameof(PingTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }
    }
}
