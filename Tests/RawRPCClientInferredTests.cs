﻿using MCWrapper.Data.Models.Raw;
using MCWrapper.Data.Models.Wallet;
using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServiceHelpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class RawRPCClientInferredTests
    {
        // private field
        private readonly IMultiChainRpcWallet _wallet;
        private readonly IMultiChainRpcRaw _raw;
        
        /// <summary>
        /// Create a new BlockchainServiceTests instance
        /// </summary>
        public RawRPCClientInferredTests()
        {
            // instantiate test services provider
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch service from provider
            _wallet = provider.GetService<IMultiChainRpcWallet>();
            _raw = provider.GetService<IMultiChainRpcRaw>();
        }

        [Test]
        public async Task RawTransactionTest()
        {
            // Stage - instantiate two new Assets
            var assetModel_0 = new AssetEntity();
            var assetModel_1 = new AssetEntity();

            var asset_0 = await _wallet.IssueAsync(
                to_address: _wallet.RpcOptions.ChainAdminAddress,
                asset_params: assetModel_0,
                quantity: 100,
                smallest_unit: 1,
                native_amount: 0, new { text = "Some text in Hex".ToHex() });

            Assert.IsNull(asset_0.Error);
            Assert.IsNotEmpty(asset_0.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(asset_0);

            var asset_1 = await _wallet.IssueAsync(
                to_address: _wallet.RpcOptions.ChainAdminAddress,
                asset_params: assetModel_1,
                quantity: 100,
                smallest_unit: 1,
                native_amount: 0, new { text = "Some text in Hex".ToHex() });

            Assert.IsNull(asset_1.Error);
            Assert.IsNotEmpty(asset_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(asset_1);


            var newAddress_0 = await _wallet.GetNewAddressAsync();

            Assert.IsNull(newAddress_0.Error);
            Assert.IsNotEmpty(newAddress_0.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(newAddress_0);

            var newAddress_1 = await _wallet.GetNewAddressAsync();

            Assert.IsNull(newAddress_1.Error);
            Assert.IsNotEmpty(newAddress_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(newAddress_1);


            var grant = await _wallet.GrantFromAsync(_wallet.RpcOptions.ChainAdminAddress, $"{newAddress_0.Result},{newAddress_1.Result}", $"{Permission.Receive},{Permission.Send}", 0, 1, Permission.MaxEndblock, "Comment", "CommentTo");

            Assert.IsNull(grant.Error);
            Assert.IsNotNull(grant.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(grant);

            var listUnspent = await _wallet.ListUnspentAsync(0, 9999, new[] { _wallet.RpcOptions.ChainAdminAddress });

            Assert.IsNull(listUnspent.Error);
            Assert.IsNotNull(listUnspent.Result);
            Assert.IsInstanceOf<RpcResponse<ListUnspentResult[]>>(listUnspent);

            var unspentAsset_0 = listUnspent.Result.SingleOrDefault(s => s.Assets.Any(a => a.Name == assetModel_0.Name));
            var unspentAsset_1 = listUnspent.Result.SingleOrDefault(s => s.Assets.Any(a => a.Name == assetModel_1.Name));

            var createRaw = await _raw.CreateRawTransactionAsync(
                transactions: new object[]
                {
                    new Dictionary<string, object>
                    {
                        { "txid", unspentAsset_0.Txid },
                        { "vout", unspentAsset_0.Vout }
                    },
                    new Dictionary<string, object>
                    {
                        { "txid", unspentAsset_1.Txid },
                        { "vout", unspentAsset_1.Vout }
                    }
                },
                assets: new Dictionary<string, Dictionary<string, int>>
                {
                    {
                        newAddress_0.Result, new Dictionary<string, int>
                        {
                            { assetModel_0.Name, 1 },
                            { assetModel_1.Name, 2 }
                        }
                    },
                    {
                        newAddress_1.Result, new Dictionary<string, int>
                        {
                            { assetModel_0.Name, 3 },
                            { assetModel_1.Name, 4 }
                        }
                    }
                },
                data: new object[] { },
                action: "");

            Assert.IsNull(createRaw.Error);
            Assert.IsNotNull(createRaw.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(createRaw);

            var decode = await _raw.DecodeRawTransactionAsync($"{createRaw.Result}");

            Assert.IsNull(decode.Error);
            Assert.IsNotNull(decode.Result);
            Assert.IsInstanceOf<RpcResponse<DecodeRawTransactionResult>>(decode);

            var rawChange = await _raw.AppendRawChangeAsync($"{createRaw.Result}", _raw.RpcOptions.ChainAdminAddress, 0);

            Assert.IsNull(rawChange.Error);
            Assert.IsNotNull(rawChange.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(rawChange);

            var rawData = await _raw.AppendRawDataAsync($"{rawChange.Result}", "Some metadta".ToHex());

            Assert.IsNull(rawData.Error);
            Assert.IsNotNull(rawData.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(rawData);

            var signRaw = await _raw.SignRawTransactionAsync($"{rawData.Result}");

            Assert.IsNull(signRaw.Error);
            Assert.IsNotNull(signRaw.Result);
            Assert.IsInstanceOf<RpcResponse<SignRawTransactionResult>>(signRaw);

            var sendRaw = await _raw.SendRawTransactionAsync(signRaw.Result.Hex, false);

            Assert.IsNull(sendRaw.Error);
            Assert.IsNotNull(sendRaw.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(sendRaw);
        }
    }
}