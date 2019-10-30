using MCWrapper.Data.Models.Blockchain;
using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.FilterHelpers;
using MCWrapper.RPC.Tests.ServiceHelpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class BlockchainRPCClientInferredTests
    {
        // private field
        private readonly WalletRpcClient Wallet;
        private readonly BlockchainRpcClient Blockchain;

        /// <summary>
        /// Create a new BlockchainServiceTests instance
        /// </summary>
        public BlockchainRPCClientInferredTests()
        {
            // instantiate test services provider
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch service from provider
            Wallet = provider.GetService<WalletRpcClient>();
            Blockchain = provider.GetService<BlockchainRpcClient>();
        }


        [Test]
        public async Task GetAssetInfoTestAsync()
        {
            // Stage - Issue a new asset to the blockchain node
            RpcResponse<string> asset = await Wallet.IssueAsync(
                to_address: Wallet.BlockchainOptions.ChainAdminAddress,
                asset_params: new AssetEntity(),
                quantity: 1,
                smallest_unit: 0.1);

            // Act - Try to get Asset information from the blockchain network
            RpcResponse<GetAssetInfoResult> verbose = await Blockchain.GetAssetInfoAsync(
                asset_identifier: asset.Result,
                verbose: true);

            // Assert
            Assert.IsNull(verbose.Error);
            Assert.IsNotNull(verbose.Result);
            Assert.IsInstanceOf<RpcResponse<GetAssetInfoResult>>(verbose);

            // Act - Try to get Asset information from the blockchain network
            RpcResponse<GetAssetInfoResult> nonVerbose = await Blockchain.GetAssetInfoAsync(
                asset_identifier: asset.Result,
                verbose: false);

            // Assert
            Assert.IsNull(nonVerbose.Error);
            Assert.IsNotNull(nonVerbose.Result);
            Assert.IsInstanceOf<RpcResponse<GetAssetInfoResult>>(nonVerbose);
        }

        [Test]
        public async Task GetBlockchainInfoTestAsync()
        {
            // Act - Ask the network for information about the blockchain
            RpcResponse<GetBlockchainInfoResult> actual = await Blockchain.GetBlockchainInfoAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockchainInfoResult>>(actual);
        }

        [Test]
        public async Task GetBestBlockHashTestAsync()
        {
            // Act - Ask blockchain network for the best block hash value
            RpcResponse<string> actual = await Blockchain.GetBestBlockHashAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GetBlockCountTestAsync()
        {
            // Act - Ask blockchain network for block count in longest chain
            RpcResponse<long> actual = await Blockchain.GetBlockCountAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<long>>(actual);
        }

        [Test]
        public async Task GetBlockHashTestAsync()
        {
            // Act - Ask blockchain network for the block hash of a specific index (block height)
            RpcResponse<string> actual = await Blockchain.GetBlockHashAsync(index: 60);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GetBlockTestAsync()
        {
            // Act - Ask blockchain network for a block at the specific index in a specific format
            var verbose = await Blockchain.GetBlockAsync<RpcResponse<GetBlockVerboseResult>>(
                hash_or_height: "60",
                verbose: true);

            // Assert
            Assert.IsNull(verbose.Error);
            Assert.IsNotNull(verbose.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockVerboseResult>>(verbose);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var concise = await Blockchain.GetBlockAsync<RpcResponse<object>>(
                hash_or_height: "60",
                verbose: false);

            // Assert
            Assert.IsNull(concise.Error);
            Assert.IsNotNull(concise.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(concise);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version1 = await Blockchain.GetBlockAsync<RpcResponse<GetBlockResultV1>>(
                hash_or_height: "60",
                verbose: 1);

            // Assert
            Assert.IsNull(version1.Error);
            Assert.IsNotNull(version1.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockResultV1>>(version1);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version2 = await Blockchain.GetBlockAsync<RpcResponse<GetBlockResultV2>>(
                hash_or_height: "60",
                verbose: 2);

            // Assert
            Assert.IsNull(version2.Error);
            Assert.IsNotNull(version2.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockResultV2>>(version2);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version3 = await Blockchain.GetBlockAsync<RpcResponse<GetBlockResultV3>>(
                hash_or_height: "60",
                verbose: 3);

            // Assert
            Assert.IsNull(version3.Error);
            Assert.IsNotNull(version3.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockResultV3>>(version3);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version4 = await Blockchain.GetBlockAsync<RpcResponse<GetBlockResultV4>>(
                hash_or_height: "60",
                verbose: 4);

            // Assert
            Assert.IsNull(version4.Error);
            Assert.IsNotNull(version4.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockResultV4>>(version4);
        }

        [Test]
        public async Task GetChainTipsTestAsync()
        {
            // Act - Ask blockchain network for the tip of the longest chain
            RpcResponse<GetChainTipsResult[]> actual = await Blockchain.GetChainTipsAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetChainTipsResult[]>>(actual);
        }

        [Test]
        public async Task GetDifficultyTestAsync()
        {
            // Act - Ask blockchain network for the mining difficulty rating
            RpcResponse<double> actual = await Blockchain.GetDifficultyAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<double>>(actual);
        }

        [Test]
        public async Task GetFilterCodeTestAsync()
        {
            // Stage - Create filter
            var filter = await Wallet.CreateAsync(
                entity_type: Entity.TxFilter,
                entity_name: StreamFilterEntity.GetUUID(),
                restrictions_or_open: new { },
                custom_fields: JsCode.DummyTxFilterCode);


            // Act - Retrieve filtercode by name, txid, or reference
            RpcResponse<string> actual = await Blockchain.GetFilterCodeAsync(filter_identifier: filter.Result);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GetLastBlockInfoTestAsync()
        {
            // Act - Ask about recent or last blocks in the network
            RpcResponse<GetLastBlockInfoResult> actual = await Blockchain.GetLastBlockInfoAsync(skip: 10);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetLastBlockInfoResult>>(actual);
        }

        [Test]
        public async Task GetMemPoolInfoTestAsync()
        {
            // Act - Ask blockchain network for mempool information
            RpcResponse<GetMemPoolInfoResult> actual = await Blockchain.GetMemPoolInfoAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetMemPoolInfoResult>>(actual);
        }

        [Test]
        public async Task GetRawMemPoolTestAsync()
        {
            // Act - Ask blockchain network for raw mempool information
            RpcResponse<GetRawMemPoolResult> actual = await Blockchain.GetRawMemPoolAsync(verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetRawMemPoolResult>>(actual);
        }

        [Test]
        public async Task GetStreamInfoTestAsync()
        {
            // Act - Fetch information about a specific blockchain stream
            RpcResponse<GetStreamInfoResult> actual = await Blockchain.GetStreamInfoAsync(
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
            var asset = await Wallet.IssueAsync(
                to_address: Wallet.BlockchainOptions.ChainAdminAddress,
                asset_params: new AssetEntity(),
                quantity: 1,
                smallest_unit: 0.1);

            // Stage - Load new asset Unspent
            var unspent = await Wallet.PrepareLockUnspentAsync(
                asset_quantities: new Dictionary<string, decimal>
                {
                    { asset.Result, 1 }
                }, false);


            // Act - Fetch details about unspent transaction output
            RpcResponse<GetTxOutResult> actual = await Blockchain.GetTxOutAsync(
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
            RpcResponse<GetTxOutSetInfoResult> actual = await Blockchain.GetTxOutSetInfoAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetTxOutSetInfoResult>>(actual);
        }

        [Test]
        public async Task ListAssetsTestAsync()
        {
            // Act - Information about a one or many assets
            RpcResponse<ListAssetsResult[]> actual = await Blockchain.ListAssetsAsync(
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
            RpcResponse<ListBlocksResult[]> actual = await Blockchain.ListBlocksAsync(
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
            RpcResponse<ListPermissionsResult[]> actual = await Blockchain.ListPermissionsAsync(
                permissions: $"{Permission.Send},{Permission.Receive}",
                addresses: Blockchain.BlockchainOptions.ChainAdminAddress,
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
            RpcResponse<ListStreamFiltersResult[]> actual = await Blockchain.ListStreamFiltersAsync(
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
            RpcResponse<ListStreamsResult[]> actual = await Blockchain.ListStreamsAsync(
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
            RpcResponse<ListTxFiltersResult[]> actual = await Blockchain.ListTxFiltersAsync(
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
            RpcResponse<object> actual = await Blockchain.ListUpgradesAsync(upgrade_identifiers: "*");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(actual);
        }

        [Test]
        public async Task RunStreamFilterTestAsync()
        {
            // Stage - Create filter
            var streamFilter = await Wallet.CreateAsync(
                entity_type: Entity.StreamFilter,
                entity_name: StreamFilterEntity.GetUUID(),
                restrictions_or_open: new { },
                custom_fields: JsCode.DummyStreamFilterCode);

            // Act - Execute stream filter
            RpcResponse<RunStreamFilterResult> actual = await Blockchain.RunStreamFilterAsync(
                filter_identifier: streamFilter.Result,
                tx_hex: null,
                vout: 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<RunStreamFilterResult>>(actual);
        }

        [Test]
        public async Task RunTxFilterTestAsync()
        {
            // Stage - List tx filters
            var txFilter = await Blockchain.ListTxFiltersAsync(
                filter_identifiers: "*",
                verbose: true);

            // Act - Execute transaction filter
            RpcResponse<RunTxFilterResult> actual = await Blockchain.RunTxFilterAsync(
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
            RpcResponse<TestStreamFilterResult> actual = await Blockchain.TestStreamFilterAsync(
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
            RpcResponse<TestTxFilterResult> actual = await Blockchain.TestTxFilterAsync(
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
            RpcResponse<bool> actual = await Blockchain.VerifyChainAsync(
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
            RpcResponse<bool> actual = await Blockchain.VerifyPermissionAsync(
                address: Blockchain.BlockchainOptions.ChainAdminAddress,
                permission: Permission.Admin);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(actual);
        }
    }
}
