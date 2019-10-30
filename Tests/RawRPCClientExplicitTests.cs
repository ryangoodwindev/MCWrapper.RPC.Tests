using MCWrapper.Data.Models.Raw;
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
    public class RawRPCClientExplicitTests
    {
        // private field
        private readonly RpcClientFactory Factory;

        /// <summary>
        /// Create a new BlockchainServiceTests instance
        /// </summary>
        public RawRPCClientExplicitTests()
        {
            // instantiate test services provider
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch service from provider
            Factory = provider.GetService<RpcClientFactory>();
        }

       [Test]
       public async Task RawTransactionTest()
        {
            var raw = Factory.RawRpcClient;
            var wallet = Factory.WalletRpcClient;

            // Stage - instantiate two new Assets
            var assetModel_0 = new AssetEntity();
            var assetModel_1 = new AssetEntity();

            var asset_0 = await wallet.IssueAsync(
                blockchainName: wallet.BlockchainOptions.ChainName,
                id: nameof(RawTransactionTest),
                to_address: wallet.BlockchainOptions.ChainAdminAddress,
                asset_params: assetModel_0,
                quantity: 100,
                smallest_unit: 1,
                native_amount: 0, new { text = "Some text in Hex".ToHex() });

            Assert.IsNull(asset_0.Error);
            Assert.IsNotEmpty(asset_0.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(asset_0);

            var asset_1 = await wallet.IssueAsync(
                blockchainName: wallet.BlockchainOptions.ChainName,
                id: nameof(RawTransactionTest),
                to_address: wallet.BlockchainOptions.ChainAdminAddress,
                asset_params: assetModel_1,
                quantity: 100,
                smallest_unit: 1,
                native_amount: 0, new { text = "Some text in Hex".ToHex() });

            Assert.IsNull(asset_1.Error);
            Assert.IsNotEmpty(asset_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(asset_1);


            var newAddress_0 = await wallet.GetNewAddressAsync(blockchainName: wallet.BlockchainOptions.ChainName, nameof(RawTransactionTest), "");

            Assert.IsNull(newAddress_0.Error);
            Assert.IsNotEmpty(newAddress_0.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(newAddress_0);

            var newAddress_1 = await wallet.GetNewAddressAsync(blockchainName: wallet.BlockchainOptions.ChainName, nameof(RawTransactionTest), "");

            Assert.IsNull(newAddress_1.Error);
            Assert.IsNotEmpty(newAddress_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(newAddress_1);


            var grant = await wallet.GrantFromAsync(wallet.BlockchainOptions.ChainName, nameof(RawTransactionTest), wallet.BlockchainOptions.ChainAdminAddress, $"{newAddress_0.Result},{newAddress_1.Result}", $"{Permission.Receive},{Permission.Send}", 0, 1, 20000, "Comment", "CommentTo");

            Assert.IsNull(grant.Error);
            Assert.IsNotNull(grant.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(grant);

            var listUnspent = await wallet.ListUnspentAsync(wallet.BlockchainOptions.ChainName, nameof(RawTransactionTest), 0, 9999, new[] { wallet.BlockchainOptions.ChainAdminAddress });

            Assert.IsNull(listUnspent.Error);
            Assert.IsNotNull(listUnspent.Result);
            Assert.IsInstanceOf<RpcResponse<ListUnspentResult[]>>(listUnspent);

            var unspentAsset_0 = listUnspent.Result.SingleOrDefault(s => s.Assets.Any(a => a.Name == assetModel_0.Name));
            var unspentAsset_1 = listUnspent.Result.SingleOrDefault(s => s.Assets.Any(a => a.Name == assetModel_1.Name));

            var createRaw = await raw.CreateRawTransactionAsync(
                blockchainName: raw.BlockchainOptions.ChainName,
                nameof(RawTransactionTest),
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
                }, new object[] { }, "");

            Assert.IsNull(createRaw.Error);
            Assert.IsNotNull(createRaw.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(createRaw);

            var decode = await raw.DecodeRawTransactionAsync(raw.BlockchainOptions.ChainName, nameof(RawTransactionTest), $"{createRaw.Result}");

            Assert.IsNull(decode.Error);
            Assert.IsNotNull(decode.Result);
            Assert.IsInstanceOf<RpcResponse<DecodeRawTransactionResult>>(decode);

            var rawChange = await raw.AppendRawChangeAsync(raw.BlockchainOptions.ChainName, nameof(RawTransactionTest), $"{createRaw.Result}", raw.BlockchainOptions.ChainAdminAddress, 0);

            Assert.IsNull(rawChange.Error);
            Assert.IsNotNull(rawChange.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(rawChange);

            var rawData = await raw.AppendRawDataAsync(raw.BlockchainOptions.ChainName, nameof(RawTransactionTest), $"{rawChange.Result}", "Some metadta".ToHex());

            Assert.IsNull(rawData.Error);
            Assert.IsNotNull(rawData.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(rawData);

            var signRaw = await raw.SignRawTransactionAsync(raw.BlockchainOptions.ChainName, nameof(RawTransactionTest), $"{rawData.Result}");

            Assert.IsNull(signRaw.Error);
            Assert.IsNotNull(signRaw.Result);
            Assert.IsInstanceOf<RpcResponse<SignRawTransactionResult>>(signRaw);

            var sendRaw = await raw.SendRawTransactionAsync(raw.BlockchainOptions.ChainName, nameof(RawTransactionTest), signRaw.Result.Hex, false);

            Assert.IsNull(sendRaw.Error);
            Assert.IsNotNull(sendRaw.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(sendRaw);
        }
    }
}
