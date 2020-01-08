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
    public class RpcGeneralClientTests
    {
        /// <summary>
        /// Remote Procedure Call (RPC) clients
        /// </summary>
        private readonly IMultiChainRpcWallet _wallet;
        private readonly IMultiChainRpcGeneral _blockchain;

        /// <summary>
        /// Create a new BlockchainServiceTests instance
        /// </summary>
        public RpcGeneralClientTests()
        {
            // instantiate test services provider
            var provider = new ParameterlessMockServices();

            _wallet = provider.GetService<IMultiChainRpcWallet>();
            _blockchain = provider.GetService<IMultiChainRpcGeneral>();
        }

        // Explicit blockchainName tests

        [Test]
        public async Task GetAssetInfoExplicitTestAsync()
        {
            // Stage - Issue a new asset
            var asset = await _wallet.IssueAsync(
                blockchainName: _wallet.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                toAddress: _wallet.RpcOptions.ChainAdminAddress,
                assetParams: new AssetEntity(),
                quantity: 1,
                smallestUnit: 0.1, 0, new Dictionary<string, string> { { "text", "Some Text in Hex".ToHex() } });

            // Act - Try to get verbose Asset information from the blockchain network
            var verbose = await _blockchain.GetAssetInfoAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                asset_identifier: asset.Result,
                verbose: true);

            // Act - Try to get precise Asset information from the blockchain network
            var nonVerbose = await _blockchain.GetAssetInfoAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
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
        public async Task GetBlockchainInfoExplicitTestAsync()
        {
            // Act - Ask the network for information about the blockchain
            var actual = await _blockchain.GetBlockchainInfoAsync(_blockchain.RpcOptions.ChainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockchainInfoResult>>(actual);
        }

        [Test]
        public async Task GetBestBlockHashExplicitTestAsync()
        {
            // Act - Ask blockchain network for the best block hash value
            var actual = await _blockchain.GetBestBlockHashAsync(_blockchain.RpcOptions.ChainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GetBlockCountExplicitTestAsync()
        {
            // Act - Ask blockchain network for block count in longest chain
            var actual = await _blockchain.GetBlockCountAsync(_blockchain.RpcOptions.ChainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<long>>(actual);
        }

        [Test]
        public async Task GetBlockHashExplicitTestAsync()
        {
            // Act - Ask blockchain network for the block hash of a specific index (block height)
            var actual = await _blockchain.GetBlockHashAsync(blockchainName: _blockchain.RpcOptions.ChainName, id: UUID.NoHyphens, index: 1);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GetBlockExplicitTestAsync()
        {
            // Act - Ask blockchain network for a block at the specific index in a specific format
            var encoded = await _blockchain.GetBlockEncodedAsync(_blockchain.RpcOptions.ChainName, UUID.NoHyphens, "1");

            // Assert
            Assert.IsNull(encoded.Error);
            Assert.IsNotNull(encoded.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(encoded);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var verbose = await _blockchain.GetBlockVerboseAsync(_blockchain.RpcOptions.ChainName, UUID.NoHyphens, "1");

            // Assert
            Assert.IsNull(verbose.Error);
            Assert.IsNotNull(verbose.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockVerboseResult>>(verbose);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version1 = await _blockchain.GetBlockV1Async(_blockchain.RpcOptions.ChainName, UUID.NoHyphens, "1");

            // Assert
            Assert.IsNull(version1.Error);
            Assert.IsNotNull(version1.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockV1Result>>(version1);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version2 = await _blockchain.GetBlockV2Async(_blockchain.RpcOptions.ChainName, UUID.NoHyphens, "1");

            // Assert
            Assert.IsNull(version2.Error);
            Assert.IsNotNull(version2.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockV2Result>>(version2);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version3 = await _blockchain.GetBlockV3Async(_blockchain.RpcOptions.ChainName, UUID.NoHyphens, "1");

            // Assert
            Assert.IsNull(version3.Error);
            Assert.IsNotNull(version3.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockV3Result>>(version3);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version4 = await _blockchain.GetBlockV4Async(_blockchain.RpcOptions.ChainName, UUID.NoHyphens, "1");

            // Assert
            Assert.IsNull(version4.Error);
            Assert.IsNotNull(version4.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockV4Result>>(version4);
        }

        [Test]
        public async Task GetChainTipsExplicitTestAsync()
        {
            // Act - Ask blockchain network for the tip of the longest chain
            var actual = await _blockchain.GetChainTipsAsync(_blockchain.RpcOptions.ChainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetChainTipsResult[]>>(actual);
        }

        [Test]
        public async Task GetDifficultyExplicitTestAsync()
        {
            // Act - Ask blockchain network for the mining difficulty rating
            var actual = await _blockchain.GetDifficultyAsync(_blockchain.RpcOptions.ChainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<double>>(actual);
        }

        [Test]
        public async Task GetFilterCodeExplicitTestAsync()
        {
            // Stage - Create filter
            var filter = await _wallet.CreateAsync(
                blockchainName: _wallet.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                entity_type: Entity.TxFilter,
                entity_name: StreamFilterEntity.GetUUID(),
                restrictions_or_open: new { },
                customFields: JsCode.DummyTxFilterCode);


            // Act - Retrieve filtercode by name, txid, or reference
            var actual = await _blockchain.GetFilterCodeAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                filter_identifier: filter.Result);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GetLastBlockInfoExplicitTestAsync()
        {
            // Act - Ask about recent or last blocks in the network
            var actual = await _blockchain.GetLastBlockInfoAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                skip: 10);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetLastBlockInfoResult>>(actual);
        }

        [Test]
        public async Task GetMemPoolInfoExplicitTestAsync()
        {
            // Act - Ask blockchain network for mempool information
            var actual = await _blockchain.GetMemPoolInfoAsync(_blockchain.RpcOptions.ChainName, id: UUID.NoHyphens);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetMemPoolInfoResult>>(actual);
        }

        [Test]
        public async Task GetRawMemPoolExplicitTestAsync()
        {
            // Act - Ask blockchain network for raw mempool information
            var actual = await _blockchain.GetRawMemPoolAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetRawMemPoolResult>>(actual);
        }

        [Test]
        public async Task GetStreamInfoExplicitTestAsync()
        {
            // Act - Fetch information about a specific blockchain stream
            var actual = await _blockchain.GetStreamInfoAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                stream_identifier: "root",
                verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetStreamInfoResult>>(actual);
        }

        [Test]
        public async Task GetTxOutExplicitTestAsync()
        {
            // Stage - Issue a new asset to the blockchain node
            var asset = await _wallet.IssueAsync(
                blockchainName: _wallet.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                toAddress: _wallet.RpcOptions.ChainAdminAddress,
                assetParams: new AssetEntity(),
                quantity: 1,
                smallestUnit: 0.1, nativeCurrencyAmount: 0, null);

            // Stage - Load new asset Unspent
            var unspent = await _wallet.PrepareLockUnspentAsync(
                blockchainName: _wallet.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                asset_quantities: new Dictionary<string, decimal>
                {
                    { asset.Result, 1 }
                }, false);


            // Act - Fetch details about unspent transaction output
            var actual = await _blockchain.GetTxOutAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                txid: unspent.Result.Txid,
                n: unspent.Result.Vout,
                include_mem_pool: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetTxOutResult>>(actual);
        }

        [Test]
        public async Task GetTxOutSetInfoExplicitTestAsync()
        {
            // Act - Statistics about the unspent transaction output set
            RpcResponse<GetTxOutSetInfoResult> actual = await _blockchain.GetTxOutSetInfoAsync(blockchainName: _blockchain.RpcOptions.ChainName, id: UUID.NoHyphens);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetTxOutSetInfoResult>>(actual);
        }

        [Test]
        public async Task ListAssetsExplicitTestAsync()
        {
            // Act - Information about a one or many assets
            RpcResponse<ListAssetsResult[]> actual = await _blockchain.ListAssetsAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
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
        public async Task ListBlocksExplicitTestAsync()
        {
            // Act - Return information about one or many blocks
            RpcResponse<ListBlocksResult[]> actual = await _blockchain.ListBlocksAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                block_set_identifier: "1, 8",
                verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListBlocksResult[]>>(actual);
        }

        [Test]
        public async Task ListPermissionsExplicitTestAsync()
        {
            // Act - List information about one or many permissions pertaining to one or many addresses
            RpcResponse<ListPermissionsResult[]> actual = await _blockchain.ListPermissionsAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                permissions: $"{Permission.Send},{Permission.Receive}",
                addresses: _blockchain.RpcOptions.ChainAdminAddress,
                verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListPermissionsResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamFiltersExplicitTestAsync()
        {
            // Act - Ask for a list of stream filters
            RpcResponse<ListStreamFiltersResult[]> actual = await _blockchain.ListStreamFiltersAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                filter_identifers: "*",
                verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamFiltersResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamsExplicitTestAsync()
        {
            // Act - Ask for a list of streams
            RpcResponse<ListStreamsResult[]> actual = await _blockchain.ListStreamsAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
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
        public async Task ListTxFiltersExplicitTestAsync()
        {
            // Act - List of transaction filters
            RpcResponse<ListTxFiltersResult[]> actual = await _blockchain.ListTxFiltersAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                filter_identifiers: "*",
                verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListTxFiltersResult[]>>(actual);
        }

        [Test]
        public async Task ListUpgradesExplicitTestAsync()
        {
            // Act - List of upgrades
            RpcResponse<ListUpgradesResult[]> actual = await _blockchain.ListUpgradesAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                upgrade_identifier: "*");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListUpgradesResult[]>>(actual);
        }

        [Test]
        public async Task RunStreamFilterExplicitTestAsync()
        {
            // Stage - Create filter
            var streamFilter = await _wallet.CreateAsync(
                blockchainName: _wallet.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                entity_type: Entity.StreamFilter,
                entity_name: StreamFilterEntity.GetUUID(),
                restrictions_or_open: new { },
                customFields: JsCode.DummyStreamFilterCode);

            // Act - Execute stream filter
            var actual = await _blockchain.RunStreamFilterAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                filter_identifier: streamFilter.Result, null, 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<RunStreamFilterResult>>(actual);
        }

        [Test]
        public async Task RunTxFilterExplicitTestAsync()
        {
            // Stage - List tx filters
            var txFilter = await _blockchain.ListTxFiltersAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                filter_identifiers: "*",
                verbose: true);

            // Act - Execute transaction filter
            var actual = await _blockchain.RunTxFilterAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                filter_identifier: txFilter.Result.FirstOrDefault().Name, null);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<RunTxFilterResult>>(actual);
        }

        [Test]
        public async Task TestStreamFilterExplicitTestAsync()
        {
            // Act - Test stream filter
            var actual = await _blockchain.TestStreamFilterAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                restrictions: new { },
                javascript_code: JsCode.DummyStreamFilterCode, null, 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<TestStreamFilterResult>>(actual);
        }

        [Test]
        public async Task TestTxFilterExplicitTestAsync()
        {
            // Act - Test transaction filter
            RpcResponse<TestTxFilterResult> actual = await _blockchain.TestTxFilterAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                restrictions: new { },
                javascript_code: JsCode.DummyTxFilterCode, null);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<TestTxFilterResult>>(actual);
        }

        [Test]
        public async Task VerifyChainExplicitTestAsync()
        {
            // Act - Verify blockchain database
            RpcResponse<bool> actual = await _blockchain.VerifyChainAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                check_level: 3,
                num_blocks: 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(actual);
        }

        [Test]
        public async Task VerifyPermissionExplicitTestAsync()
        {
            // Act - Verify permissions for a specific address
            RpcResponse<bool> actual = await _blockchain.VerifyPermissionAsync(
                blockchainName: _blockchain.RpcOptions.ChainName,
                id: UUID.NoHyphens,
                address: _blockchain.RpcOptions.ChainAdminAddress,
                permission: Permission.Admin);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(actual);
        }

        // Inferred blockchainName tests //

        [Test]
        public async Task GetAssetInfoInferredTestAsync()
        {
            // Stage - Issue a new asset to the blockchain node
            RpcResponse<string> asset = await _wallet.IssueAsync(
                toAddress: _wallet.RpcOptions.ChainAdminAddress,
                assetParams: new AssetEntity(),
                quantity: 1,
                smallestUnit: 0.1, nativeCurrencyAmount: 0, null);

            // Act - Try to get Asset information from the blockchain network
            RpcResponse<GetAssetInfoResult> verbose = await _blockchain.GetAssetInfoAsync(
                asset_identifier: asset.Result,
                verbose: true);

            // Assert
            Assert.IsNull(verbose.Error);
            Assert.IsNotNull(verbose.Result);
            Assert.IsInstanceOf<RpcResponse<GetAssetInfoResult>>(verbose);

            // Act - Try to get Asset information from the blockchain network
            RpcResponse<GetAssetInfoResult> nonVerbose = await _blockchain.GetAssetInfoAsync(
                asset_identifier: asset.Result,
                verbose: false);

            // Assert
            Assert.IsNull(nonVerbose.Error);
            Assert.IsNotNull(nonVerbose.Result);
            Assert.IsInstanceOf<RpcResponse<GetAssetInfoResult>>(nonVerbose);
        }

        [Test]
        public async Task GetBlockchainInfoInferredTestAsync()
        {
            // Act - Ask the network for information about the blockchain
            RpcResponse<GetBlockchainInfoResult> actual = await _blockchain.GetBlockchainInfoAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockchainInfoResult>>(actual);
        }

        [Test]
        public async Task GetBestBlockHashInferredTestAsync()
        {
            // Act - Ask blockchain network for the best block hash value
            RpcResponse<string> actual = await _blockchain.GetBestBlockHashAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GetBlockCountInferredTestAsync()
        {
            // Act - Ask blockchain network for block count in longest chain
            RpcResponse<long> actual = await _blockchain.GetBlockCountAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<long>>(actual);
        }

        [Test]
        public async Task GetBlockHashInferredTestAsync()
        {
            // Act - Ask blockchain network for the block hash of a specific index (block height)
            RpcResponse<string> actual = await _blockchain.GetBlockHashAsync(index: 1);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GetBlockInferredTestAsync()
        {
            // Act - Ask blockchain network for a block at the specific index in a specific format
            var encoded = await _blockchain.GetBlockEncodedAsync(hashOrHeight: "1");

            // Assert
            Assert.IsNull(encoded.Error);
            Assert.IsNotNull(encoded.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(encoded);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var verbose = await _blockchain.GetBlockVerboseAsync(hashOrHeight: "1");

            // Assert
            Assert.IsNull(verbose.Error);
            Assert.IsNotNull(verbose.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockVerboseResult>>(verbose);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version1 = await _blockchain.GetBlockV1Async(hashOrHeight: "1");

            // Assert
            Assert.IsNull(version1.Error);
            Assert.IsNotNull(version1.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockV1Result>>(version1);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version2 = await _blockchain.GetBlockV2Async(hashOrHeight: "1");

            // Assert
            Assert.IsNull(version2.Error);
            Assert.IsNotNull(version2.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockV2Result>>(version2);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version3 = await _blockchain.GetBlockV3Async(hashOrHeight: "1");

            // Assert
            Assert.IsNull(version3.Error);
            Assert.IsNotNull(version3.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockV3Result>>(version3);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var version4 = await _blockchain.GetBlockV4Async(hashOrHeight: "1");

            // Assert
            Assert.IsNull(version4.Error);
            Assert.IsNotNull(version4.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockV4Result>>(version4);
        }

        [Test]
        public async Task GetChainTipsInferredTestAsync()
        {
            // Act - Ask blockchain network for the tip of the longest chain
            RpcResponse<GetChainTipsResult[]> actual = await _blockchain.GetChainTipsAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetChainTipsResult[]>>(actual);
        }

        [Test]
        public async Task GetDifficultyInferredTestAsync()
        {
            // Act - Ask blockchain network for the mining difficulty rating
            RpcResponse<double> actual = await _blockchain.GetDifficultyAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<double>>(actual);
        }

        [Test]
        public async Task GetFilterCodeInferredTestAsync()
        {
            // Stage - Create filter
            var filter = await _wallet.CreateAsync(
                entity_type: Entity.TxFilter,
                entity_name: StreamFilterEntity.GetUUID(),
                restrictions_or_open: new { },
                customFields: JsCode.DummyTxFilterCode);


            // Act - Retrieve filtercode by name, txid, or reference
            RpcResponse<string> actual = await _blockchain.GetFilterCodeAsync(filter_identifier: filter.Result);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(actual);
        }

        [Test]
        public async Task GetLastBlockInfoInferredTestAsync()
        {
            // Act - Ask about recent or last blocks in the network
            RpcResponse<GetLastBlockInfoResult> actual = await _blockchain.GetLastBlockInfoAsync(skip: 10);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetLastBlockInfoResult>>(actual);
        }

        [Test]
        public async Task GetMemPoolInfoInferredTestAsync()
        {
            // Act - Ask blockchain network for mempool information
            RpcResponse<GetMemPoolInfoResult> actual = await _blockchain.GetMemPoolInfoAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetMemPoolInfoResult>>(actual);
        }

        [Test]
        public async Task GetRawMemPoolInferredTestAsync()
        {
            // Act - Ask blockchain network for raw mempool information
            RpcResponse<GetRawMemPoolResult> actual = await _blockchain.GetRawMemPoolAsync(verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetRawMemPoolResult>>(actual);
        }

        [Test]
        public async Task GetStreamInfoInferredTestAsync()
        {
            // Act - Fetch information about a specific blockchain stream
            RpcResponse<GetStreamInfoResult> actual = await _blockchain.GetStreamInfoAsync(
                stream_identifier: "root",
                verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetStreamInfoResult>>(actual);
        }

        [Test]
        public async Task GetTxOutInferredTestAsync()
        {
            // Stage - Issue a new asset to the blockchain node
            var asset = await _wallet.IssueAsync(
                toAddress: _wallet.RpcOptions.ChainAdminAddress,
                assetParams: new AssetEntity(),
                quantity: 1,
                smallestUnit: 0.1, nativeCurrencyAmount: 0, null);

            // Stage - Load new asset Unspent
            var unspent = await _wallet.PrepareLockUnspentAsync(
                asset_quantities: new Dictionary<string, decimal>
                {
                    { asset.Result, 1 }
                }, false);


            // Act - Fetch details about unspent transaction output
            RpcResponse<GetTxOutResult> actual = await _blockchain.GetTxOutAsync(
                txid: unspent.Result.Txid,
                n: unspent.Result.Vout,
                include_mem_pool: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetTxOutResult>>(actual);
        }

        [Test]
        public async Task GetTxOutSetInfoInferredTestAsync()
        {
            // Act - Statistics about the unspent transaction output set
            RpcResponse<GetTxOutSetInfoResult> actual = await _blockchain.GetTxOutSetInfoAsync();

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<GetTxOutSetInfoResult>>(actual);
        }

        [Test]
        public async Task ListAssetsInferredTestAsync()
        {
            // Act - Information about a one or many assets
            RpcResponse<ListAssetsResult[]> actual = await _blockchain.ListAssetsAsync(
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
        public async Task ListBlocksInferredTestAsync()
        {
            // Act - Return information about one or many blocks
            RpcResponse<ListBlocksResult[]> actual = await _blockchain.ListBlocksAsync(block_set_identifier: "1, 8", verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListBlocksResult[]>>(actual);
        }

        [Test]
        public async Task ListPermissionsInferredTestAsync()
        {
            // Act - List information about one or many permissions pertaining to one or many addresses
            RpcResponse<ListPermissionsResult[]> actual = await _blockchain.ListPermissionsAsync(
                permissions: $"{Permission.Send},{Permission.Receive}",
                addresses: _blockchain.RpcOptions.ChainAdminAddress,
                verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListPermissionsResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamFiltersInferredTestAsync()
        {
            // Act - Ask for a list of stream filters
            RpcResponse<ListStreamFiltersResult[]> actual = await _blockchain.ListStreamFiltersAsync(filter_identifers: "*", verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamFiltersResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamsInferredTestAsync()
        {
            // Act - Ask for a list of streams
            RpcResponse<ListStreamsResult[]> actual = await _blockchain.ListStreamsAsync(stream_identifiers: "*", verbose: true, count: 10, start: 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamsResult[]>>(actual);
        }

        [Test]
        public async Task ListTxFiltersInferredTestAsync()
        {
            // Act - List of transaction filters
            RpcResponse<ListTxFiltersResult[]> actual = await _blockchain.ListTxFiltersAsync(filter_identifiers: "*", verbose: true);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListTxFiltersResult[]>>(actual);
        }

        [Test]
        public async Task ListUpgradesInferredTestAsync()
        {
            // Act - List of upgrades
            RpcResponse<ListUpgradesResult[]> actual = await _blockchain.ListUpgradesAsync(upgrade_identifier: "*");

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<ListUpgradesResult[]>>(actual);
        }

        [Test]
        public async Task RunStreamFilterInferredTestAsync()
        {
            // Stage - Create filter
            var streamFilter = await _wallet.CreateAsync(
                entity_type: Entity.StreamFilter,
                entity_name: StreamFilterEntity.GetUUID(),
                restrictions_or_open: new { },
                customFields: JsCode.DummyStreamFilterCode);

            // Act - Execute stream filter
            RpcResponse<RunStreamFilterResult> actual = await _blockchain.RunStreamFilterAsync(filter_identifier: streamFilter.Result, tx_hex: null, vout: 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<RunStreamFilterResult>>(actual);
        }

        [Test]
        public async Task RunTxFilterInferredTestAsync()
        {
            // Stage - List tx filters
            var txFilter = await _blockchain.ListTxFiltersAsync(filter_identifiers: "*", verbose: true);

            // Act - Execute transaction filter
            RpcResponse<RunTxFilterResult> actual = await _blockchain.RunTxFilterAsync(filter_identifier: txFilter.Result.FirstOrDefault().Name, null);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<RunTxFilterResult>>(actual);
        }

        [Test]
        public async Task TestStreamFilterInferredTestAsync()
        {
            // Act - Test stream filter
            RpcResponse<TestStreamFilterResult> actual = await _blockchain.TestStreamFilterAsync(restrictions: new { }, javascript_code: JsCode.DummyStreamFilterCode, null, 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<TestStreamFilterResult>>(actual);
        }

        [Test]
        public async Task TestTxFilterInferredTestAsync()
        {
            // Act - Test transaction filter
            RpcResponse<TestTxFilterResult> actual = await _blockchain.TestTxFilterAsync(restrictions: new { }, javascript_code: JsCode.DummyTxFilterCode, null);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<TestTxFilterResult>>(actual);
        }

        [Test]
        public async Task VerifyChainInferredTestAsync()
        {
            // Act - Verify blockchain database
            RpcResponse<bool> actual = await _blockchain.VerifyChainAsync(check_level: 3, num_blocks: 0);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(actual);
        }

        [Test]
        public async Task VerifyPermissionInferredTestAsync()
        {
            // Act - Verify permissions for a specific address
            RpcResponse<bool> actual = await _blockchain.VerifyPermissionAsync(address: _blockchain.RpcOptions.ChainAdminAddress, permission: Permission.Admin);

            // Assert
            Assert.IsNull(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(actual);
        }
    }
}
