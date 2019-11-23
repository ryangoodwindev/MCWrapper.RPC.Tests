using MCWrapper.Data.Models.Blockchain;
using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.FilterHelpers;
using MCWrapper.RPC.Tests.ServiceHelpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class BlockchainRPCClientExplicitTests
    {
        /// <summary>
        /// Remote Procedure Call (RPC) clients
        /// </summary>
        private readonly IMultiChainRpcWallet _wallet;
        private readonly IMultiChainRpcGeneral _blockchain;

        /// <summary>
        /// Create a new BlockchainServiceTests instance
        /// </summary>
        public BlockchainRPCClientExplicitTests()
        {
            // instantiate test services provider
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch service from provider
            var clientFactory = provider.GetService<IMultiChainRpcClientFactory>();

            _wallet = clientFactory.GetRequiredRpcClient<IMultiChainRpcWallet>();
            _blockchain = clientFactory.GetRequiredRpcClient<IMultiChainRpcGeneral>();
        }

        [Test]
        public async Task GetAssetInfoTestAsync()
        {
            // Stage - Issue a new asset
            var asset = await _wallet.IssueAsync(
                blockchainName: _wallet.RpcOptions.ChainName,
                id: Guid.NewGuid().ToString("N"),
                to_address: _wallet.RpcOptions.ChainAdminAddress,
                asset_params: new AssetEntity(),
                quantity: 1,
                smallest_unit: 0.1, 0, new { text = "Some Text in Hex".ToHex() });

            // Act - Try to get verbose Asset information from the blockchain network
            var verbose = await _blockchain.GetAssetInfoAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(GetAssetInfoTestAsync),
                asset_identifier: asset.Result,
                verbose: true);

            // Act - Try to get precise Asset information from the blockchain network
            var nonVerbose = await _blockchain.GetAssetInfoAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(GetAssetInfoTestAsync),
                asset_identifier: asset.Result,
                verbose: false);

            // Assert
            Assert.IsNull(verbose.Error);
            Assert.IsNotNull(verbose.Result);
            Assert.IsInstanceOf<RpcResponse<GetAssetInfoResult>>(verbose);

            // Assert
            Assert.IsNull(nonVerbose.Error);
            Assert.IsNotNull(nonVerbose.Result);
            Assert.IsInstanceOf<RpcResponse<GetAssetInfoResult>>(nonVerbose);
        }

        [Test]
        public async Task GetBlockchainInfoTestAsync()
        {
            // Act - Ask the network for information about the blockchain
            var actual = await _blockchain.GetBlockchainInfoAsync(_blockchain.RpcOptions.ChainName, nameof(GetBlockchainInfoTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockchainInfoResult>>(actual);
        }

        [Test]
        public async Task GetBestBlockHashTestAsync()
        {
            // Act - Ask blockchain network for the best block hash value
            var actual = await _blockchain.GetBestBlockHashAsync(_blockchain.RpcOptions.ChainName, nameof(GetBestBlockHashTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GetBlockCountTestAsync()
        {
            // Act - Ask blockchain network for block count in longest chain
            var actual = await _blockchain.GetBlockCountAsync(_blockchain.RpcOptions.ChainName, nameof(GetBlockCountTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<long>>(actual);
        }

        [Test]
        public async Task GetBlockHashTestAsync()
        {
            // Act - Ask blockchain network for the block hash of a specific index (block height)
            var actual = await _blockchain.GetBlockHashAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(GetBlockHashTestAsync),
                index: 1);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GetBlockTestAsync()
        {
            // Act - Ask blockchain network for a block at the specific index in a specific format
            var verbose = await _blockchain.GetBlockAsync<RpcResponse<GetBlockVerboseResult>>(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(GetBlockTestAsync),
                hash_or_height: "1",
                verbose: true);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var concise = await _blockchain.GetBlockAsync<RpcResponse<object>>(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(GetBlockTestAsync),
                hash_or_height: "1",
                verbose: false);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version1 = await _blockchain.GetBlockAsync<RpcResponse<GetBlockResultV1>>(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(GetBlockTestAsync),
                hash_or_height: "1",
                verbose: 1);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version2 = await _blockchain.GetBlockAsync<RpcResponse<GetBlockResultV2>>(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(GetBlockTestAsync),
                hash_or_height: "1",
                verbose: 2);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version3 = await _blockchain.GetBlockAsync<RpcResponse<GetBlockResultV3>>(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(GetBlockTestAsync),
                hash_or_height: "1",
                verbose: 3);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version4 = await _blockchain.GetBlockAsync<RpcResponse<GetBlockResultV4>>(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(GetBlockTestAsync),
                hash_or_height: "1",
                verbose: 4);


            // Assert
            Assert.IsNull(verbose.Error);
            Assert.IsNotNull(verbose.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockVerboseResult>>(verbose);

            // Assert
            Assert.IsNull(concise.Error);
            Assert.IsNotNull(concise.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(concise);

            // Assert
            Assert.IsNull(version1.Error);
            Assert.IsNotNull(version1.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockResultV1>>(version1);

            // Assert
            Assert.IsNull(version2.Error);
            Assert.IsNotNull(version2.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockResultV2>>(version2);

            // Assert
            Assert.IsNull(version3.Error);
            Assert.IsNotNull(version3.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockResultV3>>(version3);

            // Assert
            Assert.IsNull(version4.Error);
            Assert.IsNotNull(version4.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockResultV4>>(version4);
        }

        [Test]
        public async Task GetChainTipsTestAsync()
        {
            // Act - Ask blockchain network for the tip of the longest chain
            var actual = await _blockchain.GetChainTipsAsync(_blockchain.RpcOptions.ChainName, nameof(GetChainTipsTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetChainTipsResult[]>>(actual);
        }

        [Test]
        public async Task GetDifficultyTestAsync()
        {
            // Act - Ask blockchain network for the mining difficulty rating
            var actual = await _blockchain.GetDifficultyAsync(_blockchain.RpcOptions.ChainName, nameof(GetDifficultyTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<double>>(actual);
        }

        [Test]
        public async Task GetFilterCodeTestAsync()
        {
            // Stage - Create filter
            var filter = await _wallet.CreateAsync(
                blockchainName: _wallet.RpcOptions.ChainName,
                id: nameof(GetFilterCodeTestAsync),
                entity_type: Entity.TxFilter,
                entity_name: StreamFilterEntity.GetUUID(),
                restrictions_or_open: new { },
                custom_fields: JsCode.DummyTxFilterCode);


            // Act - Retrieve filtercode by name, txid, or reference
            var actual = await _blockchain.GetFilterCodeAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(GetFilterCodeTestAsync),
                filter_identifier: filter.Result);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GetLastBlockInfoTestAsync()
        {
            // Act - Ask about recent or last blocks in the network
            var actual = await _blockchain.GetLastBlockInfoAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(GetLastBlockInfoTestAsync),
                skip: 10);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetLastBlockInfoResult>>(actual);
        }

        [Test]
        public async Task GetMemPoolInfoTestAsync()
        {
            // Act - Ask blockchain network for mempool information
            var actual = await _blockchain.GetMemPoolInfoAsync(_blockchain.RpcOptions.ChainName, id: nameof(GetMemPoolInfoTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetMemPoolInfoResult>>(actual);
        }

        [Test]
        public async Task GetRawMemPoolTestAsync()
        {
            // Act - Ask blockchain network for raw mempool information
            var actual = await _blockchain.GetRawMemPoolAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(GetRawMemPoolTestAsync),
                verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetRawMemPoolResult>>(actual);
        }

        [Test]
        public async Task GetStreamInfoTestAsync()
        {
            // Act - Fetch information about a specific blockchain stream
            var actual = await _blockchain.GetStreamInfoAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(GetStreamInfoTestAsync),
                stream_identifier: "root",
                verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetStreamInfoResult>>(actual);
        }

        [Test]
        public async Task GetTxOutTestAsync()
        {
            // Stage - Issue a new asset to the blockchain node
            var asset = await _wallet.IssueAsync(
                blockchainName: _wallet.RpcOptions.ChainName,
                id: Guid.NewGuid().ToString("N"),
                to_address: _wallet.RpcOptions.ChainAdminAddress,
                asset_params: new AssetEntity(),
                quantity: 1,
                smallest_unit: 0.1);

            // Stage - Load new asset Unspent
            var unspent = await _wallet.PrepareLockUnspentAsync(
                blockchainName: _wallet.RpcOptions.ChainName,
                id: nameof(GetTxOutTestAsync),
                asset_quantities: new Dictionary<string, decimal>
                {
                    { asset.Result, 1 }
                }, false);


            // Act - Fetch details about unspent transaction output
            var actual = await _blockchain.GetTxOutAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(GetTxOutTestAsync),
                txid: unspent.Result.Txid,
                n: unspent.Result.Vout,
                include_mem_pool: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetTxOutResult>>(actual);
        }

        [Test]
        public async Task GetTxOutSetInfoTestAsync()
        {
            // Act - Statistics about the unspent transaction output set
            RpcResponse<GetTxOutSetInfoResult> actual = await _blockchain.GetTxOutSetInfoAsync(blockchainName: _blockchain.RpcOptions.ChainName, id: nameof(GetTxOutSetInfoTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetTxOutSetInfoResult>>(actual);
        }

        [Test]
        public async Task ListAssetsTestAsync()
        {
            // Act - Information about a one or many assets
            RpcResponse<ListAssetsResult[]> actual = await _blockchain.ListAssetsAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(ListAssetsTestAsync),
                asset_identifiers: "*",
                verbose: true,
                count: 10,
                start: 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListAssetsResult[]>>(actual);
        }

        [Test]
        public async Task ListBlocksTestAsync()
        {
            // Act - Return information about one or many blocks
            RpcResponse<ListBlocksResult[]> actual = await _blockchain.ListBlocksAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(ListBlocksTestAsync),
                block_set_identifier: "1, 8",
                verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListBlocksResult[]>>(actual);
        }

        [Test]
        public async Task ListPermissionsTestAsync()
        {
            // Act - List information about one or many permissions pertaining to one or many addresses
            RpcResponse<ListPermissionsResult[]> actual = await _blockchain.ListPermissionsAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(ListPermissionsTestAsync),
                permissions: $"{Permission.Send},{Permission.Receive}",
                addresses: _blockchain.RpcOptions.ChainAdminAddress,
                verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListPermissionsResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamFiltersTestAsync()
        {
            // Act - Ask for a list of stream filters
            RpcResponse<ListStreamFiltersResult[]> actual = await _blockchain.ListStreamFiltersAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(ListStreamFiltersTestAsync),
                filter_identifers: "*",
                verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamFiltersResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamsTestAsync()
        {
            // Act - Ask for a list of streams
            RpcResponse<ListStreamsResult[]> actual = await _blockchain.ListStreamsAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(ListStreamsTestAsync),
                stream_identifiers: "*",
                verbose: true,
                count: 10,
                start: 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamsResult[]>>(actual);
        }

        [Test]
        public async Task ListTxFiltersTestAsync()
        {
            // Act - List of transaction filters
            RpcResponse<ListTxFiltersResult[]> actual = await _blockchain.ListTxFiltersAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(ListTxFiltersTestAsync),
                filter_identifiers: "*",
                verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListTxFiltersResult[]>>(actual);
        }

        [Test]
        public async Task ListUpgradesTestAsync()
        {
            // Act - List of upgrades
            RpcResponse<object> actual = await _blockchain.ListUpgradesAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(ListUpgradesTestAsync),
                upgrade_identifiers: "*");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task RunStreamFilterTestAsync()
        {
            // Stage - Create filter
            var streamFilter = await _wallet.CreateAsync(
                blockchainName: _wallet.RpcOptions.ChainName,
                id: nameof(RunStreamFilterTestAsync),
                entity_type: Entity.StreamFilter,
                entity_name: StreamFilterEntity.GetUUID(),
                restrictions_or_open: new { },
                custom_fields: JsCode.DummyStreamFilterCode);

            // Act - Execute stream filter
            var actual = await _blockchain.RunStreamFilterAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(RunStreamFilterTestAsync),
                filter_identifier: streamFilter.Result, null, 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<RunStreamFilterResult>>(actual);
        }

        [Test]
        public async Task RunTxFilterTestAsync()
        {
            // Stage - List tx filters
            var txFilter = await _blockchain.ListTxFiltersAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(RunTxFilterTestAsync),
                filter_identifiers: "*",
                verbose: true);

            // Act - Execute transaction filter
            var actual = await _blockchain.RunTxFilterAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(RunTxFilterTestAsync),
                filter_identifier: txFilter.Result.FirstOrDefault().Name, null);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<RunTxFilterResult>>(actual);
        }

        [Test]
        public async Task TestStreamFilterTestAsync()
        {
            // Act - Test stream filter
            var actual = await _blockchain.TestStreamFilterAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(TestStreamFilterTestAsync),
                restrictions: new { },
                javascript_code: JsCode.DummyStreamFilterCode, null, 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<TestStreamFilterResult>>(actual);
        }

        [Test]
        public async Task TestTxFilterTestAsync()
        {
            // Act - Test transaction filter
            RpcResponse<TestTxFilterResult> actual = await _blockchain.TestTxFilterAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(TestTxFilterTestAsync),
                restrictions: new { },
                javascript_code: JsCode.DummyTxFilterCode, null);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<TestTxFilterResult>>(actual);
        }

        [Test]
        public async Task VerifyChainTestAsync()
        {
            // Act - Verify blockchain database
            RpcResponse<bool> actual = await _blockchain.VerifyChainAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(VerifyChainTestAsync),
                check_level: 3,
                num_blocks: 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(actual);
        }

        [Test]
        public async Task VerifyPermissionTestAsync()
        {
            // Act - Verify permissions for a specific address
            RpcResponse<bool> actual = await _blockchain.VerifyPermissionAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: nameof(VerifyPermissionTestAsync),
                address: _blockchain.RpcOptions.ChainAdminAddress,
                permission: Permission.Admin);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(actual);
        }
    }
}
