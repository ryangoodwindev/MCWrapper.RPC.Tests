using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.FilterHelpers;
using MCWrapper.RPC.Tests.ServicesPipeline;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class RpcWalletExtensionTests
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
        private readonly IMultiChainRpcUtility _utility;
        private readonly IMultiChainRpcWallet _wallet;
        private readonly string ChainName;

        // Create new RpcWalletExtensionTests instance
        public RpcWalletExtensionTests()
        {
            // instantiate mock services container
            var services = new ParameterlessStartup();

            // fetch service from service container
            _utility = services.GetRequiredService<IMultiChainRpcUtility>();
            _wallet = services.GetRequiredService<IMultiChainRpcWallet>();

            ChainName = _wallet.RpcOptions.ChainName;
        }

        // *** Create Stream extension tests

        [Test]
        public async Task CreateStreamInferredTest()
        {
            // Stage - initialize a new Stream Entity instance
            var stream = new StreamEntity();
            stream.AddOrUpdateCustomField("description", "Testing text input for stream custom fields");
            stream.Restrictions.AddRestriction(StreamRestrictTypes.OffChain);

            // Act - attempt to create a new Stream using the inferred blockchain name
            var createStream = await _wallet.CreateStream(stream);

            // Assert
            Assert.IsNull(createStream.Error);
            Assert.IsNotNull(createStream.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createStream);
        }

        [Test]
        public async Task CreateStreamExplicitTest()
        {
            // Stage - initialize a new Stream Entity instance
            var stream = new StreamEntity();
            stream.AddOrUpdateCustomField("description", "Testing text input for stream custom fields");
            stream.Restrictions.AddRestriction(StreamRestrictTypes.OffChain);

            // Act - attempt to create a new Stream using the explicit blockchain name
            var createStream = await _wallet.CreateStream(_wallet.RpcOptions.ChainName, nameof(CreateStreamExplicitTest), stream);

            // Assert
            Assert.IsNull(createStream.Error);
            Assert.IsNotNull(createStream.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createStream);
        }

        [Test]
        public async Task CreateStreamFromInferredTest()
        {
            // Stage - initialize a new Stream Entity instance
            var stream = new StreamEntity();
            stream.AddOrUpdateCustomField("description", "Testing text input for stream custom fields");
            stream.Restrictions.AddRestriction(StreamRestrictTypes.OffChain);

            // Act - attempt to create a new Stream using the inferred blockchain name
            var createStreamFrom = await _wallet.CreateStreamFrom(_wallet.RpcOptions.ChainAdminAddress, stream);

            // Assert
            Assert.IsNull(createStreamFrom.Error);
            Assert.IsNotNull(createStreamFrom.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createStreamFrom);
        }

        [Test]
        public async Task CreateStreamFromExplicitTest()
        {
            // Stage - initialize a new Stream Entity instance
            var stream = new StreamEntity();
            stream.AddOrUpdateCustomField("description", "Testing text input for stream custom fields");
            stream.Restrictions.AddRestriction(StreamRestrictTypes.OffChain);

            // Act - attempt to create a new Stream using the explicit blockchain name
            var createStreamFrom = await _wallet.CreateStreamFrom(_wallet.RpcOptions.ChainName, nameof(CreateStreamFromExplicitTest), _wallet.RpcOptions.ChainAdminAddress, stream);

            // Assert
            Assert.IsNull(createStreamFrom.Error);
            Assert.IsNotNull(createStreamFrom.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createStreamFrom);
        }


        // *** Create Upgrade extension tests

        [Test, Ignore("Upgrades should be tested individually from other tests")]
        public async Task CreateUpgradeInferredTest()
        {
            // Stage - Initialize new UpgradeCustomFields and UpgradeEntity instances
            var customFields = new UpgradeCustomFields(
                protocolVersion: 20010,
                paramKey: UpgradeParameterKeys.AnyoneCanConnectKey,
                paramValue: 0,
                additionalParameters: null,
                startBlock: 0);

            var upgrade = new UpgradeEntity(name: Guid.NewGuid().ToString("N"), customFields: customFields);

            // Act - attempt to create a new Upgrade using the inferred blockchain name
            var createUpgrade = await _wallet.CreateUpgrade(upgrade);

            // Assert
            Assert.IsNull(createUpgrade.Error);
            Assert.IsNotNull(createUpgrade.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createUpgrade);
        }

        [Test, Ignore("Upgrades should be tested individually from other tests")]
        public async Task CreateUpgradeExplicitTest()
        {
            // Stage - Initialize new UpgradeCustomFields and UpgradeEntity instances
            var customFields = new UpgradeCustomFields(
                protocolVersion: 20010,
                paramKey: UpgradeParameterKeys.AnyoneCanConnectKey,
                paramValue: 0,
                additionalParameters: null,
                startBlock: 0);

            var upgrade = new UpgradeEntity(name: Guid.NewGuid().ToString("N"), customFields: customFields);

            // Act - attempt to create a new Upgrade using the explicit blockchain name
            var createUpgrade = await _wallet.CreateUpgrade(_wallet.RpcOptions.ChainName, nameof(CreateUpgradeExplicitTest), upgrade);

            // Assert
            Assert.IsNull(createUpgrade.Error);
            Assert.IsNotNull(createUpgrade.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createUpgrade);
        }

        [Test, Ignore("Upgrades should be tested individually from other tests")]
        public async Task CreateUpgradeFromInferredTest()
        {
            // Stage - Initialize new UpgradeCustomFields and UpgradeEntity instances
            var customFields = new UpgradeCustomFields(
                protocolVersion: 20010,
                paramKey: UpgradeParameterKeys.AnyoneCanConnectKey,
                paramValue: 0,
                additionalParameters: null,
                startBlock: 0);

            var upgrade = new UpgradeEntity(name: Guid.NewGuid().ToString("N"), customFields: customFields);

            // Act - attempt to create a new Upgrade using the inferred blockchain name
            var createUpgrade = await _wallet.CreateUpgradeFrom(_wallet.RpcOptions.ChainAdminAddress, upgrade);

            // Assert
            Assert.IsNull(createUpgrade.Error);
            Assert.IsNotNull(createUpgrade.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createUpgrade);
        }

        [Test, Ignore("Upgrades should be tested individually from other tests")]
        public async Task CreateUpgradeFromExplicitTest()
        {
            // Stage - Initialize new UpgradeCustomFields and UpgradeEntity instances
            var customFields = new UpgradeCustomFields(
                protocolVersion: 20010,
                paramKey: UpgradeParameterKeys.AnyoneCanConnectKey,
                paramValue: 0,
                additionalParameters: null,
                startBlock: 0);

            var upgrade = new UpgradeEntity(name: Guid.NewGuid().ToString("N"), customFields: customFields);

            // Act - attempt to create a new Upgrade using the explicit blockchain name
            var createUpgrade = await _wallet.CreateUpgradeFrom(_wallet.RpcOptions.ChainName, nameof(CreateUpgradeFromExplicitTest), _wallet.RpcOptions.ChainAdminAddress, upgrade);

            // Assert
            Assert.IsNull(createUpgrade.Error);
            Assert.IsNotNull(createUpgrade.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createUpgrade);
        }


        // *** Create Stream Filter extension tests

        [Test]
        public async Task CreateStreamFilterInferredTest()
        {
            // Stage - create Stream Filter entity
            var filter = new StreamFilterEntity
            {
                Name = StreamFilterEntity.GetUUID(),
                JavaScriptCode = JsCode.DummyStreamFilterCode
            };

            // Act - attempt to create a new Stream Filter using the inferred blockchain name
            var createFilter = await _wallet.CreateStreamFilter(filter);

            // Assert
            Assert.IsNull(createFilter.Error);
            Assert.IsNotNull(createFilter.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createFilter);
        }

        [Test]
        public async Task CreateStreamFilterExplicitTest()
        {
            // Stage - create Stream Filter entity
            var filter = new StreamFilterEntity
            {
                Name = StreamFilterEntity.GetUUID(),
                JavaScriptCode = JsCode.DummyStreamFilterCode
            };

            // Act - attempt to create a new Stream Filter using the explicit blockchain name
            var createFilter = await _wallet.CreateStreamFilter(_wallet.RpcOptions.ChainName, nameof(CreateStreamFilterExplicitTest), filter);

            // Assert
            Assert.IsNull(createFilter.Error);
            Assert.IsNotNull(createFilter.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createFilter);
        }

        [Test]
        public async Task CreateStreamFilterFromInferredTest()
        {
            // Stage - create Stream Filter entity
            var filter = new StreamFilterEntity
            {
                Name = StreamFilterEntity.GetUUID(),
                JavaScriptCode = JsCode.DummyStreamFilterCode
            };

            // Act - attempt to create a new Stream Filter from an address using the inferred blockchain name
            var createFilterFrom = await _wallet.CreateStreamFilterFrom(_wallet.RpcOptions.ChainAdminAddress, filter);

            // Assert
            Assert.IsNull(createFilterFrom.Error);
            Assert.IsNotNull(createFilterFrom.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createFilterFrom);
        }

        [Test]
        public async Task CreateStreamFilterFromExplicitTest()
        {
            // Stage - create Stream Filter entity
            var filter = new StreamFilterEntity
            {
                Name = StreamFilterEntity.GetUUID(),
                JavaScriptCode = JsCode.DummyStreamFilterCode
            };

            // Act - attempt to create a new Stream Filter from an address using the explicit blockchain name
            var createFilterFrom = await _wallet.CreateStreamFilterFrom(_wallet.RpcOptions.ChainName, nameof(CreateStreamFilterFromExplicitTest), _wallet.RpcOptions.ChainAdminAddress, filter);

            // Assert
            Assert.IsNull(createFilterFrom.Error);
            Assert.IsNotNull(createFilterFrom.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createFilterFrom);
        }


        // *** Create Transaction (Tx) Filter extension tests

        [Test]
        public async Task CreateTxFilterInferredBlockchainNameTest()
        {
            // Stage - create Tx Filter entity
            var filter = new TxFilterEntity();
            filter.Restrictions._For = "root";
            filter.JavaScriptCode = JsCode.DummyTxFilterCode;

            // Act - attempt to create a new Tx Filter using the inferred blockchain name
            var createFilter = await _wallet.CreateTxFilter(filter);

            // Assert
            Assert.IsNull(createFilter.Error);
            Assert.IsNotNull(createFilter.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createFilter);
        }

        [Test]
        public async Task CreateTxFilterExplicitBlockchainNameTest()
        {
            // Stage - create Tx Filter entity
            var filter = new TxFilterEntity();
            filter.Restrictions._For = "root";
            filter.JavaScriptCode = JsCode.DummyTxFilterCode;

            // Act - attempt to create a new Tx Filter using the explicit blockchain name
            var createFilter = await _wallet.CreateTxFilter(_wallet.RpcOptions.ChainName, nameof(CreateTxFilterExplicitBlockchainNameTest), filter);

            // Assert
            Assert.IsNull(createFilter.Error);
            Assert.IsNotNull(createFilter.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createFilter);
        }

        [Test]
        public async Task CreateTxFilterFromInferredBlockchainNameTest()
        {
            // Stage - create Tx Filter entity
            var filter = new TxFilterEntity();
            filter.Restrictions._For = "root";
            filter.JavaScriptCode = JsCode.DummyTxFilterCode;

            // Act - attempt to create a new Tx Filter from an address using the inferred blockchain name
            var createFilterFrom = await _wallet.CreateTxFilterFrom(_wallet.RpcOptions.ChainAdminAddress, filter);

            // Assert
            Assert.IsNull(createFilterFrom.Error);
            Assert.IsNotNull(createFilterFrom.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createFilterFrom);
        }

        [Test]
        public async Task CreateTxFilterFromExplicitBlockchainNameTest()
        {
            // Stage - create Tx Filter entity
            var filter = new TxFilterEntity();
            filter.Restrictions._For = "root";
            filter.JavaScriptCode = JsCode.DummyTxFilterCode;

            // Act - attempt to create a new Tx Filter from an address using the explicit blockchain name
            var createFilterFrom = await _wallet.CreateTxFilterFrom(_wallet.RpcOptions.ChainName, nameof(CreateTxFilterExplicitBlockchainNameTest), _wallet.RpcOptions.ChainAdminAddress, filter);

            // Assert
            Assert.IsNull(createFilterFrom.Error);
            Assert.IsNotNull(createFilterFrom.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createFilterFrom);
        }


        // *** Publish Stream Item with a single Key or multiple keys using the inferred blockchain name extension tests

        [Test]
        public async Task PublishStreamItemHexDataInferredTest()
        {
            // Stage - create a new PublishEntity instance
            var streamItem = new PublishEntity("root", PublishEntity.GetUUID(), "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKey(streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            streamItem = new PublishEntity("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeys(streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemCachedDataInferredTest()
        {
            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            var binaryCache = await _utility.CreateBinaryCacheAsync();
            var cachedData = new DataCached(binaryCache.Result);
            var streamItem = new PublishEntity<DataCached>("root", PublishEntity.GetUUID(), cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKey(streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            binaryCache = await _utility.CreateBinaryCacheAsync();
            cachedData = new DataCached(binaryCache.Result);
            streamItem = new PublishEntity<DataCached>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeys(streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemJsonDataInferredTest()
        {
            // Stage - create a new PublishEntity instance
            var jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            var streamItem = new PublishEntity<DataJson>("root", PublishEntity.GetUUID(), jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKey(streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            streamItem = new PublishEntity<DataJson>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeys(streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemTextDataInferredTest()
        {
            // Stage - create a new PublishEntity instance
            var textData = new DataText("Some Data Text for the stream item.");
            var streamItem = new PublishEntity<DataText>("root", PublishEntity.GetUUID(), textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKey(streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            textData = new DataText("Some Data Text for the stream item.");
            streamItem = new PublishEntity<DataText>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeys(streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }


        // *** Publish Stream Item with a single Key or multiple keys using the explicit blockchain name extension tests

        [Test]
        public async Task PublishStreamItemHexDataExplicitTest()
        {
            // Stage - create a new PublishEntity instance
            var streamItem = new PublishEntity("root", PublishEntity.GetUUID(), "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKey(_wallet.RpcOptions.ChainName, nameof(PublishStreamItemHexDataExplicitTest), streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            streamItem = new PublishEntity("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeys(_wallet.RpcOptions.ChainName, nameof(PublishStreamItemHexDataExplicitTest), streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemCachedDataExplicitTest()
        {
            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            var binaryCache = await _utility.CreateBinaryCacheAsync();
            var cachedData = new DataCached(binaryCache.Result);
            var streamItem = new PublishEntity<DataCached>("root", PublishEntity.GetUUID(), cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKey(_wallet.RpcOptions.ChainName, nameof(PublishStreamItemCachedDataExplicitTest), streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            binaryCache = await _utility.CreateBinaryCacheAsync();
            cachedData = new DataCached(binaryCache.Result);
            streamItem = new PublishEntity<DataCached>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeys(_wallet.RpcOptions.ChainName, nameof(PublishStreamItemCachedDataExplicitTest), streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemJsonDataExplicitTest()
        {
            // Stage - create a new PublishEntity instance
            var jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            var streamItem = new PublishEntity<DataJson>("root", PublishEntity.GetUUID(), jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKey(_wallet.RpcOptions.ChainName, nameof(PublishStreamItemJsonDataExplicitTest), streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            streamItem = new PublishEntity<DataJson>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeys(_wallet.RpcOptions.ChainName, nameof(PublishStreamItemJsonDataExplicitTest), streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemTextDataExplicitTest()
        {
            // Stage - create a new PublishEntity instance
            var textData = new DataText("Some Data Text for the stream item.");
            var streamItem = new PublishEntity<DataText>("root", PublishEntity.GetUUID(), textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKey(_wallet.RpcOptions.ChainName, nameof(PublishStreamItemTextDataExplicitTest), streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            textData = new DataText("Some Data Text for the stream item.");
            streamItem = new PublishEntity<DataText>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeys(_wallet.RpcOptions.ChainName, nameof(PublishStreamItemTextDataExplicitTest), streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }


        // *** Publish Stream Item from an address with a single Key or multiple keys using the inferred blockchain name extension tests

        [Test]
        public async Task PublishStreamItemFromHexDataInferredTest()
        {
            // Stage - create a new PublishEntity instance
            var streamItem = new PublishEntity("root", PublishEntity.GetUUID(), "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKeyFrom(_wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            streamItem = new PublishEntity("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeysFrom(_wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemFromCachedDataInferredTest()
        {
            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            var binaryCache = await _utility.CreateBinaryCacheAsync();
            var cachedData = new DataCached(binaryCache.Result);
            var streamItem = new PublishEntity<DataCached>("root", PublishEntity.GetUUID(), cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKeyFrom(_wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            binaryCache = await _utility.CreateBinaryCacheAsync();
            cachedData = new DataCached(binaryCache.Result);
            streamItem = new PublishEntity<DataCached>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeysFrom(_wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemFromJsonDataInferredTest()
        {
            // Stage - create a new PublishEntity instance
            var jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            var streamItem = new PublishEntity<DataJson>("root", PublishEntity.GetUUID(), jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKeyFrom(_wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            streamItem = new PublishEntity<DataJson>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeysFrom(_wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemFromTextDataInferredTest()
        {
            // Stage - create a new PublishEntity instance
            var textData = new DataText("Some Data Text for the stream item.");
            var streamItem = new PublishEntity<DataText>("root", PublishEntity.GetUUID(), textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKeyFrom(_wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            textData = new DataText("Some Data Text for the stream item.");
            streamItem = new PublishEntity<DataText>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeysFrom(_wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }


        // *** Publish Stream Item from an address with a single Key or multiple keys using the explicit blockchain name extension tests

        [Test]
        public async Task PublishStreamItemFromHexDataExplicitTest()
        {
            // Stage - create a new PublishEntity instance
            var streamItem = new PublishEntity("root", PublishEntity.GetUUID(), "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKeyFrom(_wallet.RpcOptions.ChainName, nameof(PublishStreamItemHexDataExplicitTest), _wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            streamItem = new PublishEntity("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeysFrom(_wallet.RpcOptions.ChainName, nameof(PublishStreamItemHexDataExplicitTest), _wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemFromCachedDataExplicitTest()
        {
            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            var binaryCache = await _utility.CreateBinaryCacheAsync();
            var cachedData = new DataCached(binaryCache.Result);
            var streamItem = new PublishEntity<DataCached>("root", PublishEntity.GetUUID(), cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKeyFrom(_wallet.RpcOptions.ChainName, nameof(PublishStreamItemCachedDataExplicitTest), _wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            binaryCache = await _utility.CreateBinaryCacheAsync();
            cachedData = new DataCached(binaryCache.Result);
            streamItem = new PublishEntity<DataCached>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeysFrom(_wallet.RpcOptions.ChainName, nameof(PublishStreamItemCachedDataExplicitTest), _wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemFromJsonDataExplicitTest()
        {
            // Stage - create a new PublishEntity instance
            var jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            var streamItem = new PublishEntity<DataJson>("root", PublishEntity.GetUUID(), jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKeyFrom(_wallet.RpcOptions.ChainName, nameof(PublishStreamItemJsonDataExplicitTest), _wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            streamItem = new PublishEntity<DataJson>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeysFrom(_wallet.RpcOptions.ChainName, nameof(PublishStreamItemJsonDataExplicitTest), _wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemFromTextDataExplicitTest()
        {
            // Stage - create a new PublishEntity instance
            var textData = new DataText("Some Data Text for the stream item.");
            var streamItem = new PublishEntity<DataText>("root", PublishEntity.GetUUID(), textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKeyFrom(_wallet.RpcOptions.ChainName, nameof(PublishStreamItemTextDataExplicitTest), _wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            textData = new DataText("Some Data Text for the stream item.");
            streamItem = new PublishEntity<DataText>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeysFrom(_wallet.RpcOptions.ChainName, nameof(PublishStreamItemTextDataExplicitTest), _wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }


        // *** PublishMultiStreamItems using an inferred blockchain name test

        [Test]
        public async Task PublishMultiStreamItemsInferredTest()
        {
            // Stage - Create a new PublishMultiItemEntity instance
            var multi = new PublishMultiEntity
            {
                StreamIdentifier = "root",
                Options = StreamRestrictTypes.OffChain
            };

            // Stage - Create a single instance of each available Data object type
            var dataHexEntityKey = new PublishMultiItemKeyEntity
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var dataHexEntityKeys = new PublishMultiItemKeysEntity
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var cache = await _utility.CreateBinaryCacheAsync();
            var dataCachedEntityKey = new PublishMultiItemKeyEntity<DataCached>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(cache.Result)
            };

            cache = await _utility.CreateBinaryCacheAsync();
            var dataCachedEntityKeys = new PublishMultiItemKeysEntity<DataCached>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(cache.Result)
            };

            var dataJsonEntityKey = new PublishMultiItemKeyEntity<DataJson>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var dataJsonEntityKeys = new PublishMultiItemKeysEntity<DataJson>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var dataTextEntityKey = new PublishMultiItemKeyEntity<DataText>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some plain text for the DataText")
            };

            var dataTextEntityKeys = new PublishMultiItemKeysEntity<DataText>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some plain text for the DataText")
            };

            // Stage - Append each data object to an array and assign it to the multi variable
            multi.Items = (new object[]
            {
                dataHexEntityKey,
                dataHexEntityKeys,
                dataCachedEntityKey,
                dataCachedEntityKeys,
                dataJsonEntityKey,
                dataJsonEntityKeys,
                dataTextEntityKey,
                dataTextEntityKeys
            });

            // Asert - Attempt to Publish multiple items to the blockchain stream
            var publish = await _wallet.PublishMultiStreamItems(multi);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }


        // *** PublishMultiStreamItems using an explicit blockchain name test

        [Test]
        public async Task PublishMultiStreamItemsExplicitTest()
        {
            // Stage - Create a new PublishMultiItemEntity instance
            var multi = new PublishMultiEntity
            {
                StreamIdentifier = "root",
                Options = StreamRestrictTypes.OffChain
            };

            // Stage - Create a single instance of each available Data object type
            var dataHexEntityKey = new PublishMultiItemKeyEntity
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var dataHexEntityKeys = new PublishMultiItemKeysEntity
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var cache = await _utility.CreateBinaryCacheAsync();
            var dataCachedEntityKey = new PublishMultiItemKeyEntity<DataCached>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(cache.Result)
            };

            cache = await _utility.CreateBinaryCacheAsync();
            var dataCachedEntityKeys = new PublishMultiItemKeysEntity<DataCached>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(cache.Result)
            };

            var dataJsonEntityKey = new PublishMultiItemKeyEntity<DataJson>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var dataJsonEntityKeys = new PublishMultiItemKeysEntity<DataJson>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var dataTextEntityKey = new PublishMultiItemKeyEntity<DataText>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some plain text for the DataText")
            };

            var dataTextEntityKeys = new PublishMultiItemKeysEntity<DataText>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some plain text for the DataText")
            };

            // Stage - Append each data object to an array and assign it to the multi variable
            multi.Items = (new object[]
            {
                dataHexEntityKey,
                dataHexEntityKeys,
                dataCachedEntityKey,
                dataCachedEntityKeys,
                dataJsonEntityKey,
                dataJsonEntityKeys,
                dataTextEntityKey,
                dataTextEntityKeys
            });

            // Asert - Attempt to Publish multiple items to the blockchain stream
            var publish = await _wallet.PublishMultiStreamItems(_wallet.RpcOptions.ChainName, nameof(PublishMultiStreamItemsExplicitTest), multi);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }


        // *** PublishMultiStreamItemsFrom using an inferred blockchain name test

        [Test]
        public async Task PublishMultiStreamItemsFromInferredTest()
        {
            // Stage - Create a new PublishMultiItemEntity instance
            var multi = new PublishMultiEntity
            {
                StreamIdentifier = "root",
                Options = StreamRestrictTypes.OffChain
            };

            // Stage - Create a single instance of each available Data object type
            var dataHexEntityKey = new PublishMultiItemKeyEntity
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var dataHexEntityKeys = new PublishMultiItemKeysEntity
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var cache = await _utility.CreateBinaryCacheAsync();
            var dataCachedEntityKey = new PublishMultiItemKeyEntity<DataCached>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(cache.Result)
            };

            cache = await _utility.CreateBinaryCacheAsync();
            var dataCachedEntityKeys = new PublishMultiItemKeysEntity<DataCached>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(cache.Result)
            };

            var dataJsonEntityKey = new PublishMultiItemKeyEntity<DataJson>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var dataJsonEntityKeys = new PublishMultiItemKeysEntity<DataJson>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var dataTextEntityKey = new PublishMultiItemKeyEntity<DataText>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some plain text for the DataText")
            };

            var dataTextEntityKeys = new PublishMultiItemKeysEntity<DataText>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some plain text for the DataText")
            };

            // Stage - Append each data object to an array and assign it to the multi variable
            multi.Items = (new object[]
            {
                dataHexEntityKey,
                dataHexEntityKeys,
                dataCachedEntityKey,
                dataCachedEntityKeys,
                dataJsonEntityKey,
                dataJsonEntityKeys,
                dataTextEntityKey,
                dataTextEntityKeys
            });

            // Asert - Attempt to Publish multiple items to the blockchain stream
            var publish = await _wallet.PublishMultiStreamItemsFrom(_wallet.RpcOptions.ChainAdminAddress, multi);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }


        // *** PublishMultiStreamItemsFrom using an explicit blockchain name test

        [Test]
        public async Task PublishMultiStreamItemsFromExplicitTest()
        {
            // Stage - Create a new PublishMultiItemEntity instance
            var multi = new PublishMultiEntity
            {
                StreamIdentifier = "root",
                Options = StreamRestrictTypes.OffChain
            };

            // Stage - Create a single instance of each available Data object type
            var dataHexEntityKey = new PublishMultiItemKeyEntity
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var dataHexEntityKeys = new PublishMultiItemKeysEntity
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var cache = await _utility.CreateBinaryCacheAsync();
            var dataCachedEntityKey = new PublishMultiItemKeyEntity<DataCached>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(cache.Result)
            };

            cache = await _utility.CreateBinaryCacheAsync();
            var dataCachedEntityKeys = new PublishMultiItemKeysEntity<DataCached>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(cache.Result)
            };

            var dataJsonEntityKey = new PublishMultiItemKeyEntity<DataJson>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var dataJsonEntityKeys = new PublishMultiItemKeysEntity<DataJson>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var dataTextEntityKey = new PublishMultiItemKeyEntity<DataText>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some plain text for the DataText")
            };

            var dataTextEntityKeys = new PublishMultiItemKeysEntity<DataText>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some plain text for the DataText")
            };

            // Stage - Append each data object to an array and assign it to the multi variable
            multi.Items = (new object[]
            {
                    dataHexEntityKey,
                    dataHexEntityKeys,
                    dataCachedEntityKey,
                    dataCachedEntityKeys,
                    dataJsonEntityKey,
                    dataJsonEntityKeys,
                    dataTextEntityKey,
                    dataTextEntityKeys
            });

            // Asert - Attempt to Publish multiple items to the blockchain stream
            var publish = await _wallet.PublishMultiStreamItemsFrom(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, multi);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);
        }
    }
}
