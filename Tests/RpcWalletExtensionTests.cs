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
        private readonly string _chainName;
        private readonly string _address;

        // Use mock startup service container
        private readonly ExplicitStartup _services = new ExplicitStartup();

        // Create new RpcWalletExtensionTests instance
        public RpcWalletExtensionTests()
        {
            _utility = _services.GetRequiredService<IMultiChainRpcUtility>();
            _wallet = _services.GetRequiredService<IMultiChainRpcWallet>();
            _address = _wallet.RpcOptions.ChainAdminAddress;
            _chainName = _wallet.RpcOptions.ChainName;
        }

        [Test]
        public async Task CreateStreamTest()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - initialize a new Stream Entity instance
            var expStream = new StreamEntity();
            expStream.AddOrUpdateCustomField("description", "Testing text input for stream custom fields");
            expStream.Restrictions.AddRestriction(StreamRestrictTypes.OffChain);

            // Act - attempt to create a new Stream using the explicit blockchain name
            var expCreateStream = await _wallet.CreateStream(_chainName, UUID.NoHyphens, expStream);

            // Assert
            Assert.IsNull(expCreateStream.Error);
            Assert.IsNotNull(expCreateStream.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expCreateStream);

            /*
              Inferred blockchain name test
           */

            // Stage - initialize a new Stream Entity instance
            var infStream = new StreamEntity();
            infStream.AddOrUpdateCustomField("description", "Testing text input for stream custom fields");
            infStream.Restrictions.AddRestriction(StreamRestrictTypes.OffChain);

            // Act - attempt to create a new Stream using the inferred blockchain name
            var infCreateStream = await _wallet.CreateStream(infStream);

            // Assert
            Assert.IsNull(infCreateStream.Error);
            Assert.IsNotNull(infCreateStream.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infCreateStream);
        }

        [Test]
        public async Task CreateStreamFromTest()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - initialize a new Stream Entity instance
            var expStream = new StreamEntity();
            expStream.AddOrUpdateCustomField("description", "Testing text input for stream custom fields");
            expStream.Restrictions.AddRestriction(StreamRestrictTypes.OffChain);

            // Act - attempt to create a new Stream using the explicit blockchain name
            var expCreateStreamFrom = await _wallet.CreateStreamFrom(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _address, expStream);

            // Assert
            Assert.IsNull(expCreateStreamFrom.Error);
            Assert.IsNotNull(expCreateStreamFrom.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expCreateStreamFrom);

            /*
              Inferred blockchain name test
           */

            // Stage - initialize a new Stream Entity instance
            var infStream = new StreamEntity();
            infStream.AddOrUpdateCustomField("description", "Testing text input for stream custom fields");
            infStream.Restrictions.AddRestriction(StreamRestrictTypes.OffChain);

            // Act - attempt to create a new Stream using the inferred blockchain name
            var infCreateStreamFrom = await _wallet.CreateStreamFrom(_wallet.RpcOptions.ChainAdminAddress, infStream);

            // Assert
            Assert.IsNull(infCreateStreamFrom.Error);
            Assert.IsNotNull(infCreateStreamFrom.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infCreateStreamFrom);
        }

        [Test, Ignore("Upgrades should be tested individually from other tests")]
        public async Task CreateUpgradeTest()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - Initialize new UpgradeCustomFields and UpgradeEntity instances
            var expCustomFields = new UpgradeCustomFields(
                protocolVersion: 20011,
                paramKey: UpgradeParameterKeys.AnyoneCanConnectKey,
                paramValue: 0,
                additionalParameters: null,
                startBlock: 0);

            var expUpgrade = new UpgradeEntity(name: Guid.NewGuid().ToString("N"), customFields: expCustomFields);

            // Act - attempt to create a new Upgrade using the explicit blockchain name
            var expCreateUpgrade = await _wallet.CreateUpgrade(_chainName, UUID.NoHyphens, expUpgrade);

            // Assert
            Assert.IsNull(expCreateUpgrade.Error);
            Assert.IsNotNull(expCreateUpgrade.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expCreateUpgrade);

            /*
               Inferred blockchain name test
            */

            // Stage - Initialize new UpgradeCustomFields and UpgradeEntity instances
            var infCustomFields = new UpgradeCustomFields(
                protocolVersion: 20011,
                paramKey: UpgradeParameterKeys.AnyoneCanConnectKey,
                paramValue: 0,
                additionalParameters: null,
                startBlock: 0);

            var infUpgrade = new UpgradeEntity(name: Guid.NewGuid().ToString("N"), customFields: infCustomFields);

            // Act - attempt to create a new Upgrade using the inferred blockchain name
            var infCreateUpgrade = await _wallet.CreateUpgrade(infUpgrade);

            // Assert
            Assert.IsNull(infCreateUpgrade.Error);
            Assert.IsNotNull(infCreateUpgrade.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infCreateUpgrade);
        }

        [Test, Ignore("Upgrades should be tested individually from other tests")]
        public async Task CreateUpgradeFromTest()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - Initialize new UpgradeCustomFields and UpgradeEntity instances
            var expCustomFields = new UpgradeCustomFields(
                protocolVersion: 20010,
                paramKey: UpgradeParameterKeys.AnyoneCanConnectKey,
                paramValue: 0,
                additionalParameters: null,
                startBlock: 0);

            var expUpgrade = new UpgradeEntity(name: Guid.NewGuid().ToString("N"), customFields: expCustomFields);

            // Act - attempt to create a new Upgrade using the explicit blockchain name
            var expCreateUpgrade = await _wallet.CreateUpgradeFrom(_chainName, UUID.NoHyphens, _address, expUpgrade);

            // Assert
            Assert.IsNull(expCreateUpgrade.Error);
            Assert.IsNotNull(expCreateUpgrade.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expCreateUpgrade);

            /*
               Inferred blockchain name test
            */

            // Stage - Initialize new UpgradeCustomFields and UpgradeEntity instances
            var infCustomFields = new UpgradeCustomFields(
                protocolVersion: 20010,
                paramKey: UpgradeParameterKeys.AnyoneCanConnectKey,
                paramValue: 0,
                additionalParameters: null,
                startBlock: 0);

            var infUpgrade = new UpgradeEntity(name: Guid.NewGuid().ToString("N"), customFields: infCustomFields);

            // Act - attempt to create a new Upgrade using the inferred blockchain name
            var infCreateUpgrade = await _wallet.CreateUpgradeFrom(_address, infUpgrade);

            // Assert
            Assert.IsNull(infCreateUpgrade.Error);
            Assert.IsNotNull(infCreateUpgrade.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infCreateUpgrade);
        }

        [Test]
        public async Task CreateStreamFilterTest()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - create Stream Filter entity
            var filter = new StreamFilterEntity
            {
                Name = StreamFilterEntity.GetUUID(),
                JavaScriptCode = JsCode.DummyStreamFilterCode
            };

            // Act - attempt to create a new Stream Filter using the explicit blockchain name
            var createFilter = await _wallet.CreateStreamFilter(_wallet.RpcOptions.ChainName, UUID.NoHyphens, filter);

            // Assert
            Assert.IsNull(createFilter.Error);
            Assert.IsNotNull(createFilter.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createFilter);

            /*
               Inferred blockchain name test
            */

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
        public async Task CreateStreamFilterFromTest()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - create Stream Filter entity
            var filter = new StreamFilterEntity
            {
                Name = StreamFilterEntity.GetUUID(),
                JavaScriptCode = JsCode.DummyStreamFilterCode
            };

            // Act - attempt to create a new Stream Filter from an address using the explicit blockchain name
            var createFilterFrom = await _wallet.CreateStreamFilterFrom(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, filter);

            // Assert
            Assert.IsNull(createFilterFrom.Error);
            Assert.IsNotNull(createFilterFrom.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createFilterFrom);

            /*
               Inferred blockchain name test
            */

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
        public async Task CreateTxFilterTest()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - create Tx Filter entity
            var filter = new TxFilterEntity();
            filter.Restrictions._For = "root";
            filter.JavaScriptCode = JsCode.DummyTxFilterCode;

            // Act - attempt to create a new Tx Filter using the explicit blockchain name
            var createFilter = await _wallet.CreateTxFilter(_wallet.RpcOptions.ChainName, UUID.NoHyphens, filter);

            // Assert
            Assert.IsNull(createFilter.Error);
            Assert.IsNotNull(createFilter.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createFilter);

            /*
               Inferred blockchain name test
            */

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
        public async Task CreateTxFilterFromTest()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - create Tx Filter entity
            var filter = new TxFilterEntity();
            filter.Restrictions._For = "root";
            filter.JavaScriptCode = JsCode.DummyTxFilterCode;

            // Act - attempt to create a new Tx Filter from an address using the explicit blockchain name
            var createFilterFrom = await _wallet.CreateTxFilterFrom(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, filter);

            // Assert
            Assert.IsNull(createFilterFrom.Error);
            Assert.IsNotNull(createFilterFrom.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(createFilterFrom);

            /*
               Inferred blockchain name test
            */

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
        public async Task PublishStreamItemHexDataTest()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - create a new PublishEntity instance
            var streamItem = new PublishEntity("root", PublishEntity.GetUUID(), "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKey(_wallet.RpcOptions.ChainName, UUID.NoHyphens, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            streamItem = new PublishEntity("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeys(_wallet.RpcOptions.ChainName, UUID.NoHyphens, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            /*
               Inferred blockchain name test
            */

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
        public async Task PublishStreamItemCachedDataTest()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            var binaryCache = await _utility.CreateBinaryCacheAsync();
            var cachedData = new DataCached(binaryCache.Result);
            var streamItem = new PublishEntity<DataCached>("root", PublishEntity.GetUUID(), cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKey(_wallet.RpcOptions.ChainName, UUID.NoHyphens, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            binaryCache = await _utility.CreateBinaryCacheAsync();
            cachedData = new DataCached(binaryCache.Result);
            streamItem = new PublishEntity<DataCached>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeys(_wallet.RpcOptions.ChainName, UUID.NoHyphens, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            /*
               Inferred blockchain name test
            */

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
        public async Task PublishStreamItemJsonDataTest()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - create a new PublishEntity instance
            var jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            var streamItem = new PublishEntity<DataJson>("root", PublishEntity.GetUUID(), jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKey(_wallet.RpcOptions.ChainName, UUID.NoHyphens, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            streamItem = new PublishEntity<DataJson>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeys(_wallet.RpcOptions.ChainName, UUID.NoHyphens, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            /*
               Inferred blockchain name test
            */

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
        public async Task PublishStreamItemTextDataTest()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - create a new PublishEntity instance
            var textData = new DataText("Some Data Text for the stream item.");
            var streamItem = new PublishEntity<DataText>("root", PublishEntity.GetUUID(), textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKey(_wallet.RpcOptions.ChainName, UUID.NoHyphens, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            textData = new DataText("Some Data Text for the stream item.");
            streamItem = new PublishEntity<DataText>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeys(_wallet.RpcOptions.ChainName, UUID.NoHyphens, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            /*
               Inferred blockchain name test
            */

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

        [Test]
        public async Task PublishStreamItemFromHexDataTest()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - create a new PublishEntity instance
            var streamItem = new PublishEntity("root", PublishEntity.GetUUID(), "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKeyFrom(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            streamItem = new PublishEntity("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeysFrom(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            /*
               Inferred blockchain name test
            */

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
        public async Task PublishStreamItemFromCachedDataTest()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            var binaryCache = await _utility.CreateBinaryCacheAsync();
            var cachedData = new DataCached(binaryCache.Result);
            var streamItem = new PublishEntity<DataCached>("root", PublishEntity.GetUUID(), cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKeyFrom(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            binaryCache = await _utility.CreateBinaryCacheAsync();
            cachedData = new DataCached(binaryCache.Result);
            streamItem = new PublishEntity<DataCached>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeysFrom(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            /*
               Inferred blockchain name test
            */

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
        public async Task PublishStreamItemFromJsonDataTest()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - create a new PublishEntity instance
            var jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            var streamItem = new PublishEntity<DataJson>("root", PublishEntity.GetUUID(), jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKeyFrom(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            streamItem = new PublishEntity<DataJson>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeysFrom(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            /*
               Inferred blockchain name test
            */

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
        public async Task PublishStreamItemFromTextDataTest()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - create a new PublishEntity instance
            var textData = new DataText("Some Data Text for the stream item.");
            var streamItem = new PublishEntity<DataText>("root", PublishEntity.GetUUID(), textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await _wallet.PublishStreamItemKeyFrom(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            textData = new DataText("Some Data Text for the stream item.");
            streamItem = new PublishEntity<DataText>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await _wallet.PublishStreamItemKeysFrom(_wallet.RpcOptions.ChainName, UUID.NoHyphens, _wallet.RpcOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsNull(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(publish);

            /*
               Inferred blockchain name test
            */

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


        [Test]
        public async Task PublishMultiStreamItemsTest()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - Create a new PublishMultiItemEntity instance
            var expMulti = new PublishMultiEntity
            {
                StreamIdentifier = "root",
                Options = StreamRestrictTypes.OffChain
            };

            // Stage - Create a single instance of each available Data object type
            var expDataHexEntityKey = new PublishMultiItemKeyEntity
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var expDataHexEntityKeys = new PublishMultiItemKeysEntity
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var expCache = await _utility.CreateBinaryCacheAsync();
            var expDataCachedEntityKey = new PublishMultiItemKeyEntity<DataCached>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(expCache.Result)
            };

            expCache = await _utility.CreateBinaryCacheAsync();
            var expDataCachedEntityKeys = new PublishMultiItemKeysEntity<DataCached>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(expCache.Result)
            };

            var expDataJsonEntityKey = new PublishMultiItemKeyEntity<DataJson>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var expDataJsonEntityKeys = new PublishMultiItemKeysEntity<DataJson>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var expDataTextEntityKey = new PublishMultiItemKeyEntity<DataText>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some plain text for the DataText")
            };

            var expDataTextEntityKeys = new PublishMultiItemKeysEntity<DataText>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some plain text for the DataText")
            };

            // Stage - Append each data object to an array and assign it to the multi variable
            expMulti.Items = (new object[]
            {
                expDataHexEntityKey,
                expDataHexEntityKeys,
                expDataCachedEntityKey,
                expDataCachedEntityKeys,
                expDataJsonEntityKey,
                expDataJsonEntityKeys,
                expDataTextEntityKey,
                expDataTextEntityKeys
            });

            // Asert - Attempt to Publish multiple items to the blockchain stream
            var expPublish = await _wallet.PublishMultiStreamItems(_chainName, UUID.NoHyphens, expMulti);

            // Assert
            Assert.IsNull(expPublish.Error);
            Assert.IsNotNull(expPublish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expPublish);

            /*
               Inferred blockchain name test
            */

            // Stage - Create a new PublishMultiItemEntity instance
            var infMulti = new PublishMultiEntity
            {
                StreamIdentifier = "root",
                Options = StreamRestrictTypes.OffChain
            };

            // Stage - Create a single instance of each available Data object type
            var infDataHexEntityKey = new PublishMultiItemKeyEntity
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var infDataHexEntityKeys = new PublishMultiItemKeysEntity
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var infCache = await _utility.CreateBinaryCacheAsync();
            var infDataCachedEntityKey = new PublishMultiItemKeyEntity<DataCached>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(infCache.Result)
            };

            infCache = await _utility.CreateBinaryCacheAsync();
            var infDataCachedEntityKeys = new PublishMultiItemKeysEntity<DataCached>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(infCache.Result)
            };

            var infDataJsonEntityKey = new PublishMultiItemKeyEntity<DataJson>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var infDataJsonEntityKeys = new PublishMultiItemKeysEntity<DataJson>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var infDataTextEntityKey = new PublishMultiItemKeyEntity<DataText>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some plain text for the DataText")
            };

            var infDataTextEntityKeys = new PublishMultiItemKeysEntity<DataText>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some plain text for the DataText")
            };

            // Stage - Append each data object to an array and assign it to the multi variable
            infMulti.Items = (new object[]
            {
                infDataHexEntityKey,
                infDataHexEntityKeys,
                infDataCachedEntityKey,
                infDataCachedEntityKeys,
                infDataJsonEntityKey,
                infDataJsonEntityKeys,
                infDataTextEntityKey,
                infDataTextEntityKeys
            });

            // Asert - Attempt to Publish multiple items to the blockchain stream
            var infPublish = await _wallet.PublishMultiStreamItems(infMulti);

            // Assert
            Assert.IsNull(infPublish.Error);
            Assert.IsNotNull(infPublish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infPublish);
        }

        [Test]
        public async Task PublishMultiStreamItemsFromTest()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - Create a new PublishMultiItemEntity instance
            var expMulti = new PublishMultiEntity
            {
                StreamIdentifier = "root",
                Options = StreamRestrictTypes.OffChain
            };

            // Stage - Create a single instance of each available Data object type
            var expDataHexEntityKey = new PublishMultiItemKeyEntity
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var expDataHexEntityKeys = new PublishMultiItemKeysEntity
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var expCache = await _utility.CreateBinaryCacheAsync();
            var expDataCachedEntityKey = new PublishMultiItemKeyEntity<DataCached>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(expCache.Result)
            };

            expCache = await _utility.CreateBinaryCacheAsync();
            var expDataCachedEntityKeys = new PublishMultiItemKeysEntity<DataCached>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(expCache.Result)
            };

            var expDataJsonEntityKey = new PublishMultiItemKeyEntity<DataJson>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var expDataJsonEntityKeys = new PublishMultiItemKeysEntity<DataJson>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var expDataTextEntityKey = new PublishMultiItemKeyEntity<DataText>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some plain text for the DataText")
            };

            var expDataTextEntityKeys = new PublishMultiItemKeysEntity<DataText>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some plain text for the DataText")
            };

            // Stage - Append each data object to an array and assign it to the multi variable
            expMulti.Items = new object[]
            {
                expDataHexEntityKey,
                expDataHexEntityKeys,
                expDataCachedEntityKey,
                expDataCachedEntityKeys,
                expDataJsonEntityKey,
                expDataJsonEntityKeys,
                expDataTextEntityKey,
                expDataTextEntityKeys
            };

            // Asert - Attempt to Publish multiple items to the blockchain stream
            var expPublish = await _wallet.PublishMultiStreamItemsFrom(_chainName, UUID.NoHyphens, _address, expMulti);

            // Assert
            Assert.IsNull(expPublish.Error);
            Assert.IsNotNull(expPublish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expPublish);

            /*
               Inferred blockchain name test
            */

            // Stage - Create a new PublishMultiItemEntity instance
            var infMulti = new PublishMultiEntity
            {
                StreamIdentifier = "root",
                Options = StreamRestrictTypes.OffChain
            };

            // Stage - Create a single instance of each available Data object type
            var infDataHexEntityKey = new PublishMultiItemKeyEntity
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var infDataHexEntityKeys = new PublishMultiItemKeysEntity
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var infCache = await _utility.CreateBinaryCacheAsync();
            var infDataCachedEntityKey = new PublishMultiItemKeyEntity<DataCached>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(infCache.Result)
            };

            infCache = await _utility.CreateBinaryCacheAsync();
            var infDataCachedEntityKeys = new PublishMultiItemKeysEntity<DataCached>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(infCache.Result)
            };

            var infDataJsonEntityKey = new PublishMultiItemKeyEntity<DataJson>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var infDataJsonEntityKeys = new PublishMultiItemKeysEntity<DataJson>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var infDataTextEntityKey = new PublishMultiItemKeyEntity<DataText>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some plain text for the DataText")
            };

            var infDataTextEntityKeys = new PublishMultiItemKeysEntity<DataText>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some plain text for the DataText")
            };

            // Stage - Append each data object to an array and assign it to the multi variable
            infMulti.Items = new object[]
            {
                    infDataHexEntityKey,
                    infDataHexEntityKeys,
                    infDataCachedEntityKey,
                    infDataCachedEntityKeys,
                    infDataJsonEntityKey,
                    infDataJsonEntityKeys,
                    infDataTextEntityKey,
                    infDataTextEntityKeys
            };

            // Asert - Attempt to Publish multiple items to the blockchain stream
            var infPublish = await _wallet.PublishMultiStreamItemsFrom(_address, infMulti);

            // Assert
            Assert.IsNull(infPublish.Error);
            Assert.IsNotNull(infPublish.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infPublish);
        }
    }
}
