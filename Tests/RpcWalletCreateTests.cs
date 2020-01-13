﻿using MCWrapper.Data.Models.Wallet;
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

namespace MCWrapper.RPC.Tests.Tests
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
        public async Task CreateFromTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act
            var expActual = await _wallet.CreateFromAsync(_chainName, UUID.NoHyphens, _address, Entity.Stream, StreamEntity.GetUUID(), true, new { });

            // Assert
            Assert.IsNull(expActual.Error);
            Assert.IsNotNull(expActual.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expActual);

            /*
               Inferred blockchain name test
            */

            // Act
            var infActual = await _wallet.CreateFromAsync(_address, Entity.Stream, StreamEntity.GetUUID(), true, new { });

            // Assert
            Assert.IsNull(infActual.Error);
            Assert.IsNotNull(infActual.Result);
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
            Assert.IsNull(expPrepareLockUnspent.Error);
            Assert.IsNotNull(expPrepareLockUnspent.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentResult>>(expPrepareLockUnspent);

            // Act
            var expRawExchange = await _wallet.CreateRawExchangeAsync(_chainName, UUID.NoHyphens, expPrepareLockUnspent.Result.Txid, expPrepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Assert
            Assert.IsNull(expRawExchange.Error);
            Assert.IsNotNull(expRawExchange.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expRawExchange);

            // Act
            var expDisable = await _wallet.DisableRawTransactionAsync(_chainName, UUID.NoHyphens, expRawExchange.Result);

            // Assert
            Assert.IsNull(expDisable.Error);
            Assert.IsNotNull(expDisable.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expDisable);

            /*
               Inferred blockchain name test
            */

            // Act
            var infPrepareLockUnspent = await _wallet.PrepareLockUnspentAsync(new Dictionary<string, int> { { "", 0 } }, true);

            // Assert
            Assert.IsNull(infPrepareLockUnspent.Error);
            Assert.IsNotNull(infPrepareLockUnspent.Result);
            Assert.IsInstanceOf<RpcResponse<PrepareLockUnspentResult>>(infPrepareLockUnspent);

            // Act
            var infRawExchange = await _wallet.CreateRawExchangeAsync(infPrepareLockUnspent.Result.Txid, infPrepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Assert
            Assert.IsNull(infRawExchange.Error);
            Assert.IsNotNull(infRawExchange.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infRawExchange);

            // Act
            var infDisable = await _wallet.DisableRawTransactionAsync(infRawExchange.Result);

            // Assert
            Assert.IsNull(infDisable.Error);
            Assert.IsNotNull(infDisable.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infDisable);
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
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.CreateRawSendFromAsync(_address, new Dictionary<string, double> { { _address, 0 } }, Array.Empty<object>(), "");

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(inf);
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
            Assert.IsNull(exp.Error);
            Assert.IsNotNull(exp.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(exp);

            /*
               Inferred blockchain name test
            */

            // Act
            var inf = await _wallet.CreateAsync(Entity.Stream, StreamEntity.GetUUID(), true, new { });

            // Assert
            Assert.IsNull(inf.Error);
            Assert.IsNotNull(inf.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(inf);
        }
    }
}
