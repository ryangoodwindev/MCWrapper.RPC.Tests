using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Constants;
using MCWrapper.RPC.Extensions;
using MCWrapper.RPC.Ledger.Entities;
using MCWrapper.RPC.Ledger.Factory;
using MCWrapper.RPC.Ledger.Models.Blockchain;
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
        private readonly RpcClientFactory Factory;

        /// <summary>
        /// Create a new BlockchainServiceTests instance
        /// </summary>
        public BlockchainRPCClientExplicitTests()
        {
            // instantiate test services provider
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch service from provider
            Factory = provider.GetService<RpcClientFactory>();
        }

        [Test]
        public async Task GetAssetInfoTestAsync()
        {
            // Stage - Fetch RPC clients
            var wallet = Factory.GetWalletRpcClient();
            var blockchain = Factory.GetBlockchainRpcClient();

            // Stage - Issue a new asset
            var asset = await wallet.IssueAsync(
                blockchainName: wallet.BlockchainOptions.ChainName,
                id: Guid.NewGuid().ToString("N"),
                to_address: wallet.BlockchainOptions.ChainAdminAddress,
                asset_params: new AssetEntity(),
                quantity: 1,
                smallest_unit: 0.1, 0, new { text = "Some Text in Hex".ToHex() });

            // Act - Try to get verbose Asset information from the blockchain network
            var verbose = await blockchain.GetAssetInfoAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
                id: nameof(GetAssetInfoTestAsync),
                asset_identifier: asset.Result,
                verbose: true);

            // Act - Try to get precise Asset information from the blockchain network
            var nonVerbose = await blockchain.GetAssetInfoAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
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
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Ask the network for information about the blockchain
            var actual = await blockchain.GetBlockchainInfoAsync(blockchain.BlockchainOptions.ChainName, nameof(GetBlockchainInfoTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockchainInfoResult>>(actual);
        }

        [Test]
        public async Task GetBestBlockHashTestAsync()
        {
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Ask blockchain network for the best block hash value
            var actual = await blockchain.GetBestBlockHashAsync(blockchain.BlockchainOptions.ChainName, nameof(GetBestBlockHashTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GetBlockCountTestAsync()
        {
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Ask blockchain network for block count in longest chain
            var actual = await blockchain.GetBlockCountAsync(blockchain.BlockchainOptions.ChainName, nameof(GetBlockCountTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<long>>(actual);
        }

        [Test]
        public async Task GetBlockHashTestAsync()
        {
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Ask blockchain network for the block hash of a specific index (block height)
            var actual = await blockchain.GetBlockHashAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
                id: nameof(GetBlockHashTestAsync),
                index: 60);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GetBlockTestAsync()
        {
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var verbose = await blockchain.GetBlockAsync<RpcResponse<GetBlockVerboseResult>>(
                blockchainName: blockchain.BlockchainOptions.ChainName,
                id: nameof(GetBlockTestAsync),
                hash_or_height: "60",
                verbose: true);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var concise = await blockchain.GetBlockAsync<RpcResponse<object>>(
                blockchainName: blockchain.BlockchainOptions.ChainName,
                id: nameof(GetBlockTestAsync),
                hash_or_height: "60",
                verbose: false);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version1 = await blockchain.GetBlockAsync<RpcResponse<GetBlockResultV1>>(
                blockchainName: blockchain.BlockchainOptions.ChainName,
                id: nameof(GetBlockTestAsync),
                hash_or_height: "60",
                verbose: 1);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version2 = await blockchain.GetBlockAsync<RpcResponse<GetBlockResultV2>>(
                blockchainName: blockchain.BlockchainOptions.ChainName,
                id: nameof(GetBlockTestAsync),
                hash_or_height: "60",
                verbose: 2);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version3 = await blockchain.GetBlockAsync<RpcResponse<GetBlockResultV3>>(
                blockchainName: blockchain.BlockchainOptions.ChainName,
                id: nameof(GetBlockTestAsync),
                hash_or_height: "60",
                verbose: 3);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version4 = await blockchain.GetBlockAsync<RpcResponse<GetBlockResultV4>>(
                blockchainName: blockchain.BlockchainOptions.ChainName,
                id: nameof(GetBlockTestAsync),
                hash_or_height: "60",
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
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Ask blockchain network for the tip of the longest chain
            var actual = await blockchain.GetChainTipsAsync(blockchain.BlockchainOptions.ChainName, nameof(GetChainTipsTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetChainTipsResult[]>>(actual);
        }

        [Test]
        public async Task GetDifficultyTestAsync()
        {
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Ask blockchain network for the mining difficulty rating
            var actual = await blockchain.GetDifficultyAsync(blockchain.BlockchainOptions.ChainName, nameof(GetDifficultyTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<double>>(actual);
        }

        [Test]
        public async Task GetFilterCodeTestAsync()
        {
            // Stage - Fetch RPC clients
            var wallet = Factory.GetWalletRpcClient();
            var blockchain = Factory.GetBlockchainRpcClient();

            // Stage - Create filter
            var filter = await wallet.CreateAsync(
                blockchainName: wallet.BlockchainOptions.ChainName,
                id: nameof(GetFilterCodeTestAsync),
                entity_type: Entity.TxFilter,
                entity_name: StreamFilterEntity.GetUUID(),
                restrictions_or_open: new { },
                custom_fields: JsCode.DummyTxFilterCode);


            // Act - Retrieve filtercode by name, txid, or reference
            var actual = await blockchain.GetFilterCodeAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
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
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Ask about recent or last blocks in the network
            var actual = await blockchain.GetLastBlockInfoAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
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
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Ask blockchain network for mempool information
            var actual = await blockchain.GetMemPoolInfoAsync(blockchainName: blockchain.BlockchainOptions.ChainName, id: nameof(GetMemPoolInfoTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetMemPoolInfoResult>>(actual);
        }

        [Test]
        public async Task GetRawMemPoolTestAsync()
        {
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Ask blockchain network for raw mempool information
            var actual = await blockchain.GetRawMemPoolAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
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
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Fetch information about a specific blockchain stream
            var actual = await blockchain.GetStreamInfoAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
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
            // Stage - Fetch RPC clients
            var wallet = Factory.GetWalletRpcClient();
            var blockchain = Factory.GetBlockchainRpcClient();

            // Stage - Issue a new asset to the blockchain node
            var asset = await wallet.IssueAsync(
                blockchainName: wallet.BlockchainOptions.ChainName,
                id: Guid.NewGuid().ToString("N"),
                to_address: wallet.BlockchainOptions.ChainAdminAddress,
                asset_params: new AssetEntity(),
                quantity: 1,
                smallest_unit: 0.1);

            // Stage - Load new asset Unspent
            var unspent = await wallet.PrepareLockUnspentAsync(
                blockchainName: wallet.BlockchainOptions.ChainName,
                id: nameof(GetTxOutTestAsync),
                asset_quantities: new Dictionary<string, decimal>
                {
                    { asset.Result, 1 }
                }, false);


            // Act - Fetch details about unspent transaction output
            var actual = await blockchain.GetTxOutAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
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
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Statistics about the unspent transaction output set
            RpcResponse<GetTxOutSetInfoResult> actual = await blockchain.GetTxOutSetInfoAsync(blockchainName: blockchain.BlockchainOptions.ChainName, id: nameof(GetTxOutSetInfoTestAsync));

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetTxOutSetInfoResult>>(actual);
        }

        [Test]
        public async Task ListAssetsTestAsync()
        {
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Information about a one or many assets
            RpcResponse<ListAssetsResult[]> actual = await blockchain.ListAssetsAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
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
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Return information about one or many blocks
            RpcResponse<ListBlocksResult[]> actual = await blockchain.ListBlocksAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
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
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - List information about one or many permissions pertaining to one or many addresses
            RpcResponse<ListPermissionsResult[]> actual = await blockchain.ListPermissionsAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
                id: nameof(ListPermissionsTestAsync),
                permissions: $"{Permission.Send},{Permission.Receive}",
                addresses: blockchain.BlockchainOptions.ChainAdminAddress,
                verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListPermissionsResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamFiltersTestAsync()
        {
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Ask for a list of stream filters
            RpcResponse<ListStreamFiltersResult[]> actual = await blockchain.ListStreamFiltersAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
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
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Ask for a list of streams
            RpcResponse<ListStreamsResult[]> actual = await blockchain.ListStreamsAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
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
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - List of transaction filters
            RpcResponse<ListTxFiltersResult[]> actual = await blockchain.ListTxFiltersAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
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
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - List of upgrades
            RpcResponse<object> actual = await blockchain.ListUpgradesAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
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
            // Stage - Fetch RPC clients
            var wallet = Factory.GetWalletRpcClient();
            var blockchain = Factory.GetBlockchainRpcClient();

            // Stage - Create filter
            var streamFilter = await wallet.CreateAsync(
                blockchainName: wallet.BlockchainOptions.ChainName,
                id: nameof(RunStreamFilterTestAsync),
                entity_type: Entity.StreamFilter,
                entity_name: StreamFilterEntity.GetUUID(),
                restrictions_or_open: new { },
                custom_fields: JsCode.DummyStreamFilterCode);

            // Act - Execute stream filter
            var actual = await blockchain.RunStreamFilterAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
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
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Stage - List tx filters
            var txFilter = await blockchain.ListTxFiltersAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
                id: nameof(RunTxFilterTestAsync),
                filter_identifiers: "*",
                verbose: true);

            // Act - Execute transaction filter
            var actual = await blockchain.RunTxFilterAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
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
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Test stream filter
            var actual = await blockchain.TestStreamFilterAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
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
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Test transaction filter
            RpcResponse<TestTxFilterResult> actual = await blockchain.TestTxFilterAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
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
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Verify blockchain database
            RpcResponse<bool> actual = await blockchain.VerifyChainAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
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
            // Stage - Fetch RPC client
            var blockchain = Factory.GetBlockchainRpcClient();

            // Act - Verify permissions for a specific address
            RpcResponse<bool> actual = await blockchain.VerifyPermissionAsync(
                blockchainName: blockchain.BlockchainOptions.ChainName,
                id: nameof(VerifyPermissionTestAsync),
                address: blockchain.BlockchainOptions.ChainAdminAddress,
                permission: Permission.Admin);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(actual);
        }
    }
}
