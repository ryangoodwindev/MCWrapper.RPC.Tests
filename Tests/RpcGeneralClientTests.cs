using MCWrapper.Data.Models.Blockchain;
using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.FilterHelpers;
using MCWrapper.RPC.Tests.ServicesPipeline;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class RpcGeneralClientTests
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
        private readonly IMultiChainRpcGeneral _blockchain;
        private readonly IMultiChainRpcWallet _wallet;
        private readonly string _chainName;
        private readonly string _address;

        // Use mock startup service container
        private readonly ExplicitStartup _services = new ExplicitStartup();

        // Create a new RpcGeneralClientTests instance
        public RpcGeneralClientTests()
        {
            _blockchain = _services.GetRequiredService<IMultiChainRpcGeneral>();
            _wallet = _services.GetRequiredService<IMultiChainRpcWallet>();
            _address = _wallet.RpcOptions.ChainAdminAddress;
            _chainName = _wallet.RpcOptions.ChainName;
        }

        [Test]
        public async Task GetAssetInfoTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Stage - Issue a new asset
            var expAsset = await _wallet.IssueAsync(_chainName, UUID.NoHyphens, _address, new AssetEntity(), 1, 0.1, 0, new Dictionary<string, string> { { "text", "Some Text in Hex".ToHex() } });

            // Act - Try to get verbose Asset information from the blockchain network
            var expVerbose = await _blockchain.GetAssetInfoAsync(_chainName, UUID.NoHyphens, expAsset.Result, true);

            // Assert
            Assert.IsNull(expVerbose.Error);
            Assert.IsNotNull(expVerbose.Result);
            Assert.IsInstanceOf<RpcResponse<GetAssetInfoResult>>(expVerbose);

            // Act - Try to get precise Asset information from the blockchain network
            var expNonVerbose = await _blockchain.GetAssetInfoAsync(_chainName, UUID.NoHyphens, expAsset.Result, false);

            // Assert
            Assert.IsNull(expNonVerbose.Error);
            Assert.IsNotNull(expNonVerbose.Result);
            Assert.IsInstanceOf<RpcResponse<GetAssetInfoResult>>(expNonVerbose);

            /*
              Inferred blockchain name test
           */

            // Stage - Issue a new asset to the blockchain node
            var infAsset = await _wallet.IssueAsync(_address, new AssetEntity(), 1, 0.1, 0, null);

            // Act - Try to get Asset information from the blockchain network
            var infVerbose = await _blockchain.GetAssetInfoAsync(infAsset.Result, true);

            // Assert
            Assert.IsNull(infVerbose.Error);
            Assert.IsNotNull(infVerbose.Result);
            Assert.IsInstanceOf<RpcResponse<GetAssetInfoResult>>(infVerbose);

            // Act - Try to get Asset information from the blockchain network
            var infNonVerbose = await _blockchain.GetAssetInfoAsync(infAsset.Result, false);

            // Assert
            Assert.IsNull(infNonVerbose.Error);
            Assert.IsNotNull(infNonVerbose.Result);
            Assert.IsInstanceOf<RpcResponse<GetAssetInfoResult>>(infNonVerbose);
        }

        [Test]
        public async Task GetBlockchainInfoTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Ask the network for information about the blockchain
            var expGet = await _blockchain.GetBlockchainInfoAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expGet.Error);
            Assert.IsNotNull(expGet.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockchainInfoResult>>(expGet);

            /*
              Inferred blockchain name test
           */

            // Act - Ask the network for information about the blockchain
            var infGet = await _blockchain.GetBlockchainInfoAsync();

            // Assert
            Assert.IsNull(infGet.Error);
            Assert.IsNotNull(infGet.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockchainInfoResult>>(infGet);
        }

        [Test]
        public async Task GetBestBlockHashTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Ask blockchain network for the best block hash value
            var expGet = await _blockchain.GetBestBlockHashAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expGet.Error);
            Assert.IsNotNull(expGet.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expGet);

            /*
              Inferred blockchain name test
           */

            // Act - Ask blockchain network for the best block hash value
            var infGet = await _blockchain.GetBestBlockHashAsync();

            // Assert
            Assert.IsNull(infGet.Error);
            Assert.IsNotNull(infGet.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infGet);
        }

        [Test]
        public async Task GetBlockCountTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Ask blockchain network for block count in longest chain
            var expGet = await _blockchain.GetBlockCountAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expGet.Error);
            Assert.IsNotNull(expGet.Result);
            Assert.IsInstanceOf<RpcResponse<long>>(expGet);

            /*
              Inferred blockchain name test
           */

            // Act - Ask blockchain network for block count in longest chain
            var infGet = await _blockchain.GetBlockCountAsync();

            // Assert
            Assert.IsNull(infGet.Error);
            Assert.IsNotNull(infGet.Result);
            Assert.IsInstanceOf<RpcResponse<long>>(infGet);
        }

        [Test]
        public async Task GetBlockHashTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Ask blockchain network for the block hash of a specific index (block height)
            var expGet = await _blockchain.GetBlockHashAsync(_chainName, UUID.NoHyphens, 1);

            // Assert
            Assert.IsNull(expGet.Error);
            Assert.IsNotNull(expGet.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expGet);

            /*
              Inferred blockchain name test
           */

            // Act - Ask blockchain network for the block hash of a specific index (block height)
            var infGet = await _blockchain.GetBlockHashAsync(1);

            // Assert
            Assert.IsNull(infGet.Error);
            Assert.IsNotNull(infGet.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infGet);
        }

        [Test]
        public async Task GetBlockTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var expEncoded = await _blockchain.GetBlockEncodedAsync(_chainName, UUID.NoHyphens, "1");

            // Assert
            Assert.IsNull(expEncoded.Error);
            Assert.IsNotNull(expEncoded.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expEncoded);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var expVerbose = await _blockchain.GetBlockVerboseAsync(_chainName, UUID.NoHyphens, "1");

            // Assert
            Assert.IsNull(expVerbose.Error);
            Assert.IsNotNull(expVerbose.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockVerboseResult>>(expVerbose);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var expVersion1 = await _blockchain.GetBlockV1Async(_chainName, UUID.NoHyphens, "1");

            // Assert
            Assert.IsNull(expVersion1.Error);
            Assert.IsNotNull(expVersion1.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockV1Result>>(expVersion1);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var expVersion2 = await _blockchain.GetBlockV2Async(_chainName, UUID.NoHyphens, "1");

            // Assert
            Assert.IsNull(expVersion2.Error);
            Assert.IsNotNull(expVersion2.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockV2Result>>(expVersion2);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var expVersion3 = await _blockchain.GetBlockV3Async(_chainName, UUID.NoHyphens, "1");

            // Assert
            Assert.IsNull(expVersion3.Error);
            Assert.IsNotNull(expVersion3.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockV3Result>>(expVersion3);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var expVersion4 = await _blockchain.GetBlockV4Async(_chainName, UUID.NoHyphens, "1");

            // Assert
            Assert.IsNull(expVersion4.Error);
            Assert.IsNotNull(expVersion4.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockV4Result>>(expVersion4);

            /*
              Inferred blockchain name test
           */

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var infEncoded = await _blockchain.GetBlockEncodedAsync("1");

            // Assert
            Assert.IsNull(infEncoded.Error);
            Assert.IsNotNull(infEncoded.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infEncoded);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var infVerbose = await _blockchain.GetBlockVerboseAsync("1");

            // Assert
            Assert.IsNull(infVerbose.Error);
            Assert.IsNotNull(infVerbose.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockVerboseResult>>(infVerbose);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var infVersion1 = await _blockchain.GetBlockV1Async("1");

            // Assert
            Assert.IsNull(infVersion1.Error);
            Assert.IsNotNull(infVersion1.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockV1Result>>(infVersion1);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var infVersion2 = await _blockchain.GetBlockV2Async("1");

            // Assert
            Assert.IsNull(infVersion2.Error);
            Assert.IsNotNull(infVersion2.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockV2Result>>(infVersion2);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var infVersion3 = await _blockchain.GetBlockV3Async("1");

            // Assert
            Assert.IsNull(infVersion3.Error);
            Assert.IsNotNull(infVersion3.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockV3Result>>(infVersion3);

            // Act - Ask blockchain network for a block at the specific index in a specific format
            var infVersion4 = await _blockchain.GetBlockV4Async("1");

            // Assert
            Assert.IsNull(infVersion4.Error);
            Assert.IsNotNull(infVersion4.Result);
            Assert.IsInstanceOf<RpcResponse<GetBlockV4Result>>(infVersion4);
        }

        [Test]
        public async Task GetChainTipsTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Ask blockchain network for the tip of the longest chain
            var expGet = await _blockchain.GetChainTipsAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expGet.Error);
            Assert.IsNotNull(expGet.Result);
            Assert.IsInstanceOf<RpcResponse<GetChainTipsResult[]>>(expGet);

            /*
              Inferred blockchain name test
           */

            // Act - Ask blockchain network for the tip of the longest chain
            var infGet = await _blockchain.GetChainTipsAsync();

            // Assert
            Assert.IsNull(infGet.Error);
            Assert.IsNotNull(infGet.Result);
            Assert.IsInstanceOf<RpcResponse<GetChainTipsResult[]>>(infGet);
        }

        [Test]
        public async Task GetDifficultyTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Ask blockchain network for the mining difficulty rating
            var expGet = await _blockchain.GetDifficultyAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expGet.Error);
            Assert.IsNotNull(expGet.Result);
            Assert.IsInstanceOf<RpcResponse<double>>(expGet);

            /*
              Inferred blockchain name test
           */

            // Act - Ask blockchain network for the mining difficulty rating
            var infGet = await _blockchain.GetDifficultyAsync();

            // Assert
            Assert.IsNull(infGet.Error);
            Assert.IsNotNull(infGet.Result);
            Assert.IsInstanceOf<RpcResponse<double>>(infGet);
        }

        [Test]
        public async Task GetFilterCodeTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Stage - Create filter
            var expFilter = await _wallet.CreateAsync(_chainName, UUID.NoHyphens, Entity.TxFilter, StreamFilterEntity.GetUUID(), new { }, JsCode.DummyTxFilterCode);


            // Act - Retrieve filtercode by name, txid, or reference
            var expGet = await _blockchain.GetFilterCodeAsync(_chainName, UUID.NoHyphens, expFilter.Result);

            // Assert
            Assert.IsNull(expGet.Error);
            Assert.IsNotNull(expGet.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expGet);

            /*
              Inferred blockchain name test
           */

            // Stage - Create filter
            var infFilter = await _wallet.CreateAsync(Entity.TxFilter, StreamFilterEntity.GetUUID(), new { }, JsCode.DummyTxFilterCode);


            // Act - Retrieve filtercode by name, txid, or reference
            var infGet = await _blockchain.GetFilterCodeAsync(infFilter.Result);

            // Assert
            Assert.IsNull(infGet.Error);
            Assert.IsNotNull(infGet.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infGet);
        }

        [Test]
        public async Task GetLastBlockInfoTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Ask about recent or last blocks in the network
            var expGet = await _blockchain.GetLastBlockInfoAsync(_chainName, UUID.NoHyphens, 10);

            // Assert
            Assert.IsNull(expGet.Error);
            Assert.IsNotNull(expGet.Result);
            Assert.IsInstanceOf<RpcResponse<GetLastBlockInfoResult>>(expGet);

            /*
              Inferred blockchain name test
           */

            // Act - Ask about recent or last blocks in the network
            var infGet = await _blockchain.GetLastBlockInfoAsync(10);

            // Assert
            Assert.IsNull(infGet.Error);
            Assert.IsNotNull(infGet.Result);
            Assert.IsInstanceOf<RpcResponse<GetLastBlockInfoResult>>(infGet);
        }

        [Test]
        public async Task GetMemPoolInfoTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Ask blockchain network for mempool information
            var expGet = await _blockchain.GetMemPoolInfoAsync(_chainName, UUID.NoHyphens);

            // Assert
            Assert.IsNull(expGet.Error);
            Assert.IsNotNull(expGet.Result);
            Assert.IsInstanceOf<RpcResponse<GetMemPoolInfoResult>>(expGet);

            /*
              Inferred blockchain name test
           */

            // Act - Ask blockchain network for mempool information
            var infGet = await _blockchain.GetMemPoolInfoAsync();

            // Assert
            Assert.IsNull(infGet.Error);
            Assert.IsNotNull(infGet.Result);
            Assert.IsInstanceOf<RpcResponse<GetMemPoolInfoResult>>(infGet);
        }

        [Test]
        public async Task GetRawMemPoolTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Ask blockchain network for raw mempool information
            var expGet = await _blockchain.GetRawMemPoolAsync(_chainName, UUID.NoHyphens, true);

            // Assert
            Assert.IsNull(expGet.Error);
            Assert.IsNotNull(expGet.Result);
            Assert.IsInstanceOf<RpcResponse<GetRawMemPoolResult>>(expGet);

            /*
              Inferred blockchain name test
           */

            // Act - Ask blockchain network for raw mempool information
            var infGet = await _blockchain.GetRawMemPoolAsync(true);

            // Assert
            Assert.IsNull(infGet.Error);
            Assert.IsNotNull(infGet.Result);
            Assert.IsInstanceOf<RpcResponse<GetRawMemPoolResult>>(infGet);
        }

        [Test]
        public async Task GetStreamInfoTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Fetch information about a specific blockchain stream
            var expGet = await _blockchain.GetStreamInfoAsync(_chainName, UUID.NoHyphens, "root", true);

            // Assert
            Assert.IsNull(expGet.Error);
            Assert.IsNotNull(expGet.Result);
            Assert.IsInstanceOf<RpcResponse<GetStreamInfoResult>>(expGet);

            /*
              Inferred blockchain name test
           */

            // Act - Fetch information about a specific blockchain stream
            var infGet = await _blockchain.GetStreamInfoAsync("root", true);

            // Assert
            Assert.IsNull(infGet.Error);
            Assert.IsNotNull(infGet.Result);
            Assert.IsInstanceOf<RpcResponse<GetStreamInfoResult>>(infGet);
        }

        [Test]
        public async Task GetTxOutExplicitTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Stage - Issue a new asset to the blockchain node
            var expIssue = await _wallet.IssueAsync(_chainName, UUID.NoHyphens, _address, new AssetEntity(), 1, 0.1, 0, null);

            // Stage - Load new asset Unspent
            var expUnspent = await _wallet.PrepareLockUnspentAsync(_chainName, UUID.NoHyphens,
                new Dictionary<string, decimal>
                {
                    { expIssue.Result, 1 }
                },
                false);

            // Act - Fetch details about unspent transaction output
            var expGet = await _blockchain.GetTxOutAsync(_chainName, UUID.NoHyphens, expUnspent.Result.Txid, expUnspent.Result.Vout, true);

            // Assert
            Assert.IsNull(expGet.Error);
            Assert.IsNotNull(expGet.Result);
            Assert.IsInstanceOf<RpcResponse<GetTxOutResult>>(expGet);

            /*
              Inferred blockchain name test
           */

            // Stage - Issue a new asset to the blockchain node
            var infAsset = await _wallet.IssueAsync(_address, new AssetEntity(), 1, 0.1, 0, null);

            // Stage - Load new asset Unspent
            var infUnspent = await _wallet.PrepareLockUnspentAsync(new Dictionary<string, decimal> { { infAsset.Result, 1 } }, false);

            // Act - Fetch details about unspent transaction output
            var infGet = await _blockchain.GetTxOutAsync(infUnspent.Result.Txid, infUnspent.Result.Vout, true);

            // Assert
            Assert.IsNull(infGet.Error);
            Assert.IsNotNull(infGet.Result);
            Assert.IsInstanceOf<RpcResponse<GetTxOutResult>>(infGet);
        }

        [Test]
        public async Task GetTxOutSetInfoTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Statistics about the unspent transaction output set
            var expGet = await _blockchain.GetTxOutSetInfoAsync(_chainName, id: UUID.NoHyphens);

            // Assert
            Assert.IsNull(expGet.Error);
            Assert.IsNotNull(expGet.Result);
            Assert.IsInstanceOf<RpcResponse<GetTxOutSetInfoResult>>(expGet);

            /*
              Inferred blockchain name test
           */

            // Act - Statistics about the unspent transaction output set
            var infGet = await _blockchain.GetTxOutSetInfoAsync();

            // Assert
            Assert.IsNull(infGet.Error);
            Assert.IsNotNull(infGet.Result);
            Assert.IsInstanceOf<RpcResponse<GetTxOutSetInfoResult>>(infGet);
        }

        [Test]
        public async Task ListAssetsTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Information about a one or many assets
            var expList = await _blockchain.ListAssetsAsync(_chainName, UUID.NoHyphens, "*", true, 10, 0);

            // Assert
            Assert.IsNull(expList.Error);
            Assert.IsNotNull(expList.Result);
            Assert.IsInstanceOf<RpcResponse<ListAssetsResult[]>>(expList);

            /*
              Inferred blockchain name test
           */

            // Act - Information about a one or many assets
            var infList = await _blockchain.ListAssetsAsync("*", true, 10, 0);

            // Assert
            Assert.IsNull(infList.Error);
            Assert.IsNotNull(infList.Result);
            Assert.IsInstanceOf<RpcResponse<ListAssetsResult[]>>(infList);
        }

        [Test]
        public async Task ListBlocksTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Return information about one or many blocks
            var expList = await _blockchain.ListBlocksAsync(_chainName, UUID.NoHyphens, "1, 8", true);

            // Assert
            Assert.IsNull(expList.Error);
            Assert.IsNotNull(expList.Result);
            Assert.IsInstanceOf<RpcResponse<ListBlocksResult[]>>(expList);

            /*
              Inferred blockchain name test
           */

            // Act - Return information about one or many blocks
            var infList = await _blockchain.ListBlocksAsync("1, 8", true);

            // Assert
            Assert.IsNull(infList.Error);
            Assert.IsNotNull(infList.Result);
            Assert.IsInstanceOf<RpcResponse<ListBlocksResult[]>>(infList);
        }

        [Test]
        public async Task ListPermissionsTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - List information about one or many permissions pertaining to one or many addresses
            var expList = await _blockchain.ListPermissionsAsync(_chainName, UUID.NoHyphens, $"{Permission.Send},{Permission.Receive}", _address, true);

            // Assert
            Assert.IsNull(expList.Error);
            Assert.IsNotNull(expList.Result);
            Assert.IsInstanceOf<RpcResponse<ListPermissionsResult[]>>(expList);

            /*
              Inferred blockchain name test
           */

            // Act - List information about one or many permissions pertaining to one or many addresses
            var infList = await _blockchain.ListPermissionsAsync($"{Permission.Send},{Permission.Receive}", _address, true);

            // Assert
            Assert.IsNull(infList.Error);
            Assert.IsNotNull(infList.Result);
            Assert.IsInstanceOf<RpcResponse<ListPermissionsResult[]>>(infList);
        }

        [Test]
        public async Task ListStreamFiltersTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Ask for a list of stream filters
            var expList = await _blockchain.ListStreamFiltersAsync(_chainName, UUID.NoHyphens, "*", true);

            // Assert
            Assert.IsNull(expList.Error);
            Assert.IsNotNull(expList.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamFiltersResult[]>>(expList);

            /*
              Inferred blockchain name test
           */

            // Act - Ask for a list of stream filters
            var infList = await _blockchain.ListStreamFiltersAsync("*", true);

            // Assert
            Assert.IsNull(infList.Error);
            Assert.IsNotNull(infList.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamFiltersResult[]>>(infList);
        }

        [Test]
        public async Task ListStreamsTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Ask for a list of streams
            var expList = await _blockchain.ListStreamsAsync(_chainName, UUID.NoHyphens, "*", true, 10, 0);

            // Assert
            Assert.IsNull(expList.Error);
            Assert.IsNotNull(expList.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamsResult[]>>(expList);

            /*
              Inferred blockchain name test
           */

            // Act - Ask for a list of streams
            var infList = await _blockchain.ListStreamsAsync("*", true, 10, 0);

            // Assert
            Assert.IsNull(infList.Error);
            Assert.IsNotNull(infList.Result);
            Assert.IsInstanceOf<RpcResponse<ListStreamsResult[]>>(infList);
        }

        [Test]
        public async Task ListTxFiltersTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - List of transaction filters
            var expList = await _blockchain.ListTxFiltersAsync(_chainName, UUID.NoHyphens, "*", true);

            // Assert
            Assert.IsNull(expList.Error);
            Assert.IsNotNull(expList.Result);
            Assert.IsInstanceOf<RpcResponse<ListTxFiltersResult[]>>(expList);

            /*
              Inferred blockchain name test
           */

            // Act - List of transaction filters
            var infList = await _blockchain.ListTxFiltersAsync("*", true);

            // Assert
            Assert.IsNull(infList.Error);
            Assert.IsNotNull(infList.Result);
            Assert.IsInstanceOf<RpcResponse<ListTxFiltersResult[]>>(infList);
        }

        [Test]
        public async Task ListUpgradesTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - List of upgrades
            var expList = await _blockchain.ListUpgradesAsync(_chainName, UUID.NoHyphens, "*");

            // Assert
            Assert.IsNull(expList.Error);
            Assert.IsNotNull(expList.Result);
            Assert.IsInstanceOf<RpcResponse<ListUpgradesResult[]>>(expList);

            /*
              Inferred blockchain name test
           */

            // Act - List of upgrades
            var infList = await _blockchain.ListUpgradesAsync(upgrade_identifier: "*");

            // Assert
            Assert.IsNull(infList.Error);
            Assert.IsNotNull(infList.Result);
            Assert.IsInstanceOf<RpcResponse<ListUpgradesResult[]>>(infList);
        }

        [Test]
        public async Task RunStreamFilterTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Stage - Create filter
            var expStreamFilter = await _wallet.CreateAsync(_chainName, UUID.NoHyphens, Entity.StreamFilter, StreamFilterEntity.GetUUID(), new { }, JsCode.DummyStreamFilterCode);

            // Act - Execute stream filter
            var expRun = await _blockchain.RunStreamFilterAsync(_chainName, UUID.NoHyphens, expStreamFilter.Result, null, 0);

            // Assert
            Assert.IsNull(expRun.Error);
            Assert.IsNotNull(expRun.Result);
            Assert.IsInstanceOf<RpcResponse<RunStreamFilterResult>>(expRun);

            /*
              Inferred blockchain name test
           */

            // Stage - Create filter
            var infStreamFilter = await _wallet.CreateAsync(Entity.StreamFilter, StreamFilterEntity.GetUUID(), new { }, JsCode.DummyStreamFilterCode);

            // Act - Execute stream filter
            var infRun = await _blockchain.RunStreamFilterAsync(infStreamFilter.Result, null, 0);

            // Assert
            Assert.IsNull(infRun.Error);
            Assert.IsNotNull(infRun.Result);
            Assert.IsInstanceOf<RpcResponse<RunStreamFilterResult>>(infRun);
        }

        [Test]
        public async Task RunTxFilterTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Stage - List tx filters
            var expList = await _blockchain.ListTxFiltersAsync(_chainName, UUID.NoHyphens, "*", true);

            // Act - Execute transaction filter
            var expRun = await _blockchain.RunTxFilterAsync(_chainName, UUID.NoHyphens, expList.Result.FirstOrDefault().Name, null);

            // Assert
            Assert.IsNull(expRun.Error);
            Assert.IsNotNull(expRun.Result);
            Assert.IsInstanceOf<RpcResponse<RunTxFilterResult>>(expRun);

            /*
              Inferred blockchain name test
           */

            // Stage - List tx filters
            var infList = await _blockchain.ListTxFiltersAsync("*", true);

            // Act - Execute transaction filter
            var infRun = await _blockchain.RunTxFilterAsync(infList.Result.FirstOrDefault().Name, null);

            // Assert
            Assert.IsNull(infRun.Error);
            Assert.IsNotNull(infRun.Result);
            Assert.IsInstanceOf<RpcResponse<RunTxFilterResult>>(infRun);
        }

        [Test]
        public async Task TestStreamFilterTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Test stream filter
            var expTest = await _blockchain.TestStreamFilterAsync(_chainName, UUID.NoHyphens, new { }, JsCode.DummyStreamFilterCode, null, 0);

            // Assert
            Assert.IsNull(expTest.Error);
            Assert.IsNotNull(expTest.Result);
            Assert.IsInstanceOf<RpcResponse<TestStreamFilterResult>>(expTest);

            /*
              Inferred blockchain name test
           */

            // Act - Test stream filter
            var infTest = await _blockchain.TestStreamFilterAsync(new { }, JsCode.DummyStreamFilterCode, null, 0);

            // Assert
            Assert.IsNull(infTest.Error);
            Assert.IsNotNull(infTest.Result);
            Assert.IsInstanceOf<RpcResponse<TestStreamFilterResult>>(infTest);
        }

        [Test]
        public async Task TestTxFilterTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Test transaction filter
            var expTest = await _blockchain.TestTxFilterAsync(_chainName, UUID.NoHyphens, new { }, JsCode.DummyTxFilterCode, null);

            // Assert
            Assert.IsNull(expTest.Error);
            Assert.IsNotNull(expTest.Result);
            Assert.IsInstanceOf<RpcResponse<TestTxFilterResult>>(expTest);

            /*
              Inferred blockchain name test
           */

            // Act - Test transaction filter
            var infTest = await _blockchain.TestTxFilterAsync(new { }, JsCode.DummyTxFilterCode, null);

            // Assert
            Assert.IsNull(infTest.Error);
            Assert.IsNotNull(infTest.Result);
            Assert.IsInstanceOf<RpcResponse<TestTxFilterResult>>(infTest);
        }

        [Test]
        public async Task VerifyChainTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Verify blockchain database
            var expVerify = await _blockchain.VerifyChainAsync(_chainName, UUID.NoHyphens, 3, 0);

            // Assert
            Assert.IsNull(expVerify.Error);
            Assert.IsNotNull(expVerify.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(expVerify);

            /*
              Inferred blockchain name test
           */

            // Act - Verify blockchain database
            var infVerify = await _blockchain.VerifyChainAsync(3, 0);

            // Assert
            Assert.IsNull(infVerify.Error);
            Assert.IsNotNull(infVerify.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(infVerify);
        }

        [Test]
        public async Task VerifyPermissionTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Verify permissions for a specific address
            var expVerify = await _blockchain.VerifyPermissionAsync(_chainName, UUID.NoHyphens, _address, Permission.Admin);

            // Assert
            Assert.IsNull(expVerify.Error);
            Assert.IsNotNull(expVerify.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(expVerify);

            /*
              Inferred blockchain name test
           */

            // Act - Verify permissions for a specific address
            var infVerify = await _blockchain.VerifyPermissionAsync(_address, Permission.Admin);

            // Assert
            Assert.IsNull(infVerify.Error);
            Assert.IsNotNull(infVerify.Result);
            Assert.IsInstanceOf<RpcResponse<bool>>(infVerify);
        }
    }
}
