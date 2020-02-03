using MCWrapper.Data.Models.Wallet;
using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Test.ServicesPipeline;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Test.Create
{
    [TestFixture]
    public class RpcWalletCreateTests
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
        private readonly IMultiChainRpcWallet _wallet;
        private readonly string _chainName;
        private readonly string _address;

        // Use mock startup service container
        private readonly ExplicitStartup _services = new ExplicitStartup();

        public RpcWalletCreateTests()
        {
            _wallet = _services.GetRequiredService<IMultiChainRpcWallet>();
            _address = _wallet.RpcOptions.ChainAdminAddress;
            _chainName = _wallet.RpcOptions.ChainName;
        }

        [Test]
        public async Task CreateTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var exp = await _wallet.CreateAsync(_chainName, UUID.NoHyphens, Entity.Stream, StreamEntity.GetUUID(), true, new { });

            // Assert
            Assert.IsTrue(exp.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.CreateAsync(Entity.Stream, StreamEntity.GetUUID(), true, new { });

            // Assert
            Assert.IsTrue(inf.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(inf);
        }

        [Test]
        public async Task CreateFromTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.CreateFromAsync(_chainName, UUID.NoHyphens, _address, Entity.Stream, StreamEntity.GetUUID(), true, new { });

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(expActual);

            /*
               Inferred blockchain name test
            */

            // Act
            var infActual = await _wallet.CreateFromAsync(_address, Entity.Stream, StreamEntity.GetUUID(), true, new { });

            // Assert
            Assert.IsTrue(expActual.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(infActual);
        }

        [Test]
        public async Task CreateRawExchangeTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expPrepareLockUnspent = await _wallet.PrepareLockUnspentAsync(_chainName, UUID.NoHyphens, new Dictionary<string, int> { { "", 0 } }, true);

            // Assert
            Assert.IsTrue(expPrepareLockUnspent.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentResult>>(expPrepareLockUnspent);

            // Act
            var expRawExchange = await _wallet.CreateRawExchangeAsync(_chainName, UUID.NoHyphens, expPrepareLockUnspent.Result.Txid, expPrepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Assert
            Assert.IsTrue(expRawExchange.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(expRawExchange);

            // Act
            var expDisable = await _wallet.DisableRawTransactionAsync(_chainName, UUID.NoHyphens, expRawExchange.Result);

            // Assert
            Assert.IsTrue(expDisable.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(expDisable);

            /*
               Inferred blockchain name test
            */

            // Act
            var infPrepareLockUnspent = await _wallet.PrepareLockUnspentAsync(new Dictionary<string, int> { { "", 0 } }, true);

            // Assert
            Assert.IsTrue(infPrepareLockUnspent.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentResult>>(infPrepareLockUnspent);

            // Act
            var infRawExchange = await _wallet.CreateRawExchangeAsync(infPrepareLockUnspent.Result.Txid, infPrepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Assert
            Assert.IsTrue(infRawExchange.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(infRawExchange);

            // Act
            var infDisable = await _wallet.DisableRawTransactionAsync(infRawExchange.Result);

            // Assert
            Assert.IsTrue(infDisable.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(infDisable);
        }

        [Test]
        public async Task CreateRawSendFromTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var exp = await _wallet.CreateRawSendFromAsync(_chainName, UUID.NoHyphens, _address, new Dictionary<string, double> { { _address, 0 } }, Array.Empty<object>(), "");

            // Assert
            Assert.IsTrue(exp.IsSuccess());
            Assert.IsInstanceOf<RpcResponse>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.CreateRawSendFromAsync(_address, new Dictionary<string, double> { { _address, 0 } }, Array.Empty<object>(), "");

            // Assert
            Assert.IsTrue(inf.IsSuccess());
            Assert.IsInstanceOf<RpcResponse>(inf);
        }
    }
}
