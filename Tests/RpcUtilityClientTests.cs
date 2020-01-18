using MCWrapper.Data.Models.Utility;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Test.ServicesPipeline;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Test.Utility
{
    [TestFixture]
    public class RpcUtilityClientTests
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

        // Create a new RpcUtilityClientTests instance
        public RpcUtilityClientTests()
        {
            _utility = _services.GetRequiredService<IMultiChainRpcUtility>();
            _wallet = _services.GetRequiredService<IMultiChainRpcWallet>();
            _address = _wallet.RpcOptions.ChainAdminAddress;
            _chainName = _wallet.RpcOptions.ChainName;
        }

        [Test]
        public async Task BinaryCacheTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act - Create a binary cache
            var expCache = await _utility.CreateBinaryCacheAsync(_chainName, id: UUID.NoHyphens);

            // Assert
            Assert.IsTrue(expCache.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(expCache);

            // Act - Append to binary cache
            var expAppend = await _utility.AppendBinaryCacheAsync(_chainName, UUID.NoHyphens, expCache.Result,
                "Some string data we can use to generate dat hex content. An then a bit more at the end".ToHex());

            // Assert
            Assert.IsTrue(expAppend.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<int>>(expAppend);

            // Act - Delete binary cache
            var expDelete = await _utility.DeleteBinaryCacheAsync(_chainName, UUID.NoHyphens, expCache.Result);

            // Assert
            Assert.IsTrue(expDelete.IsSuccess());
            Assert.IsInstanceOf<RpcResponse>(expDelete);

            /*
               Inferred blockchain name test
            */

            // Act - Create a binary cache
            var infCache = await _utility.CreateBinaryCacheAsync();

            // Assert
            Assert.IsTrue(infCache.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(infCache);

            // Act - Append to binary cache
            var infAppend = await _utility.AppendBinaryCacheAsync(infCache.Result,
                "Some string data we can use to generate dat hex content. An then a bit more at the end".ToHex());

            // Assert
            Assert.IsTrue(infAppend.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<int>>(infAppend);

            // Act - Delete binary cache
            var infDelete = await _utility.DeleteBinaryCacheAsync(infCache.Result);

            // Assert
            Assert.IsTrue(infDelete.IsSuccess());
            Assert.IsInstanceOf<RpcResponse>(infDelete);
        }

        [Test]
        public async Task CreateKeyPairsTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act - Ask network for a new key pair
            var expCreate = await _utility.CreateKeyPairsAsync(_chainName, UUID.NoHyphens, 3);

            // Assert
            Assert.IsTrue(expCreate.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<CreateKeyPairsResult>>>(expCreate);

            /*
               Inferred blockchain name test
            */

            // Act - Ask network for a new key pair
            var infCreate = await _utility.CreateKeyPairsAsync(3);

            // Assert
            Assert.IsTrue(infCreate.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<IList<CreateKeyPairsResult>>>(infCreate);
        }

        [Test]
        public async Task CreateMultiSigTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act - Create an address that requires the node's signature
            var expCreate = await _utility.CreateMultiSigAsync(_chainName, UUID.NoHyphens, 1, new string[] 
            {
                _utility.RpcOptions.ChainAdminAddress,
            });

            // Assert
            Assert.IsTrue(expCreate.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<CreateMultiSigResult>>(expCreate);

            /*
               Inferred blockchain name test
            */

            // Act - Create an address that requires the node's signature
            var infCreate = await _utility.CreateMultiSigAsync(1, new string[]
            {
                _utility.RpcOptions.ChainAdminAddress,
            });

            // Assert
            Assert.IsTrue(infCreate.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<CreateMultiSigResult>>(infCreate);
        }

        [Test]
        public async Task EstimateFeeTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act - Estimate fee according to kB and number of blocks
            var expFee = await _utility.EstimateFeeAsync(_chainName, UUID.NoHyphens, 78);

            // Assert
            Assert.IsTrue(expFee.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<long>>(expFee);

            /*
               Inferred blockchain name test
            */

            // Act - Estimate fee according to kB and number of blocks
            var infFee = await _utility.EstimateFeeAsync(78);

            // Assert
            Assert.IsTrue(infFee.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<long>>(infFee);
        }

        [Test]
        public async Task EstimatePriorityTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act - Estimate zero-fee transaction 
            var expPriority = await _utility.EstimatePriorityAsync(_chainName, UUID.NoHyphens, 78);

            // Assert
            Assert.IsTrue(expPriority.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<float>>(expPriority);

            /*
               Inferred blockchain name test
            */

            // Act - Estimate zero-fee transaction 
            var infPriority = await _utility.EstimatePriorityAsync(78);

            // Assert
            Assert.IsTrue(infPriority.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<float>>(infPriority);
        }

        [Test]
        public async Task ValidateAddressTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Act - Return information about the given address
            var expValidate = await _utility.ValidateAddressAsync(_chainName, UUID.NoHyphens, _address);

            // Assert
            Assert.IsTrue(expValidate.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<ValidateAddressResult>>(expValidate);

            /*
               Inferred blockchain name test
            */

            // Act - Return information about the given address
            var infValidate = await _utility.ValidateAddressAsync(_address);

            // Assert
            Assert.IsTrue(infValidate.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<ValidateAddressResult>>(infValidate);
        }

        [Test]
        public async Task VerifyMessageTestAsync()
        {
            /*
              Explicit blockchain name test
           */

            // Stage - Default test message
            var defaultMessage = "Some Test Message for this distributed blockchain network.";

            // Act - Sign message
            var expSignature = await _wallet.SignMessageAsync(_chainName, UUID.NoHyphens, _address, defaultMessage);

            // Assert
            Assert.IsTrue(expSignature.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(expSignature);

            // Act - Verify signed message
            var expVerify = await _utility.VerifyMessageAsync(_chainName, UUID.NoHyphens, _address, expSignature.Result, defaultMessage);

            // Assert
            Assert.IsTrue(expVerify.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<bool>>(expVerify);

            /*
               Inferred blockchain name test
            */

            // Act - Sign message
            var infSignature = await _wallet.SignMessageAsync(_address, defaultMessage);

            // Assert
            Assert.IsTrue(infSignature.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(infSignature);

            // Act - Verify signed message
            var infActual = await _utility.VerifyMessageAsync(_address, infSignature.Result, defaultMessage);

            // Assert
            Assert.IsTrue(infActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<bool>>(infActual);
        }
    }
}