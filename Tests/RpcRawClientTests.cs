using MCWrapper.Data.Models.Raw;
using MCWrapper.Data.Models.Wallet;
using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Tests.ServicesPipeline;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests
{
    [TestFixture]
    public class RpcRawClientTests
    {
        // Inject services
        private readonly IMultiChainRpcWallet _wallet;
        private readonly IMultiChainRpcRaw _raw;
        private readonly string ChainName;
        private readonly string Address;

        // Create a new RpcRawClientTests instance
        public RpcRawClientTests()
        {
            // instantiate mock service container
            var services = new ParameterlessStartup();

            // fetch services from service container
            _wallet = services.GetRequiredService<IMultiChainRpcWallet>();
            _raw = services.GetRequiredService<IMultiChainRpcRaw>();

            Address = _wallet.RpcOptions.ChainAdminAddress;
            ChainName = _wallet.RpcOptions.ChainName;
        }

        [Test]
        public async Task RawTransactionTest()
        {
            // Explicit blockchain name tests

            var infAssetModel_0 = new AssetEntity();
            var infAssetModel_1 = new AssetEntity();

            var infAsset_0 = await _wallet.IssueAsync(ChainName, UUID.NoHyphens, Address, infAssetModel_0, 100, 1, 0, new Dictionary<string, string> { { "text", "Some text in Hex".ToHex() } });

            Assert.IsNull(infAsset_0.Error);
            Assert.IsNotEmpty(infAsset_0.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infAsset_0);

            var infAsset_1 = await _wallet.IssueAsync(ChainName, UUID.NoHyphens, Address, infAssetModel_1, 100, 1, 0, new Dictionary<string, string> { { "text", "Some text in Hex".ToHex() } });

            Assert.IsNull(infAsset_1.Error);
            Assert.IsNotEmpty(infAsset_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infAsset_1);


            var infNewAddress_0 = await _wallet.GetNewAddressAsync(ChainName, UUID.NoHyphens);

            Assert.IsNull(infNewAddress_0.Error);
            Assert.IsNotEmpty(infNewAddress_0.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infNewAddress_0);

            var infNewAddress_1 = await _wallet.GetNewAddressAsync(ChainName, UUID.NoHyphens);

            Assert.IsNull(infNewAddress_1.Error);
            Assert.IsNotEmpty(infNewAddress_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infNewAddress_1);


            var infGrant = await _wallet.GrantFromAsync(ChainName, UUID.NoHyphens, Address, $"{infNewAddress_0.Result},{infNewAddress_1.Result}", $"{Permission.Receive},{Permission.Send}", 0, 1, 20000, "Comment", "CommentTo");

            Assert.IsNull(infGrant.Error);
            Assert.IsNotNull(infGrant.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infGrant);

            var infListUnspent = await _wallet.ListUnspentAsync(ChainName, UUID.NoHyphens, 0, 9999, new[] { Address });

            Assert.IsNull(infListUnspent.Error);
            Assert.IsNotNull(infListUnspent.Result);
            Assert.IsInstanceOf<RpcResponse<ListUnspentResult[]>>(infListUnspent);

            var unspentAsset_0 = infListUnspent.Result.SingleOrDefault(s => s.Assets.Any(a => a.Name == infAssetModel_0.Name));
            var unspentAsset_1 = infListUnspent.Result.SingleOrDefault(s => s.Assets.Any(a => a.Name == infAssetModel_1.Name));

            var infCreateRaw = await _raw.CreateRawTransactionAsync(ChainName, UUID.NoHyphens, new object[]
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
                    infNewAddress_0.Result, new Dictionary<string, int>
                    {
                        { infAssetModel_0.Name, 1 },
                        { infAssetModel_1.Name, 2 }
                    }
                },
                {
                    infNewAddress_1.Result, new Dictionary<string, int>
                    {
                        { infAssetModel_0.Name, 3 },
                        { infAssetModel_1.Name, 4 }
                    }
                }
            }, Array.Empty<object>(), string.Empty);

            Assert.IsNull(infCreateRaw.Error);
            Assert.IsNotNull(infCreateRaw.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(infCreateRaw);

            var infDecode = await _raw.DecodeRawTransactionAsync(ChainName, UUID.NoHyphens, $"{infCreateRaw.Result}");

            Assert.IsNull(infDecode.Error);
            Assert.IsNotNull(infDecode.Result);
            Assert.IsInstanceOf<RpcResponse<DecodeRawTransactionResult>>(infDecode);

            var infRawChange = await _raw.AppendRawChangeAsync(ChainName, UUID.NoHyphens, $"{infCreateRaw.Result}", Address, 0);

            Assert.IsNull(infRawChange.Error);
            Assert.IsNotNull(infRawChange.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infRawChange);

            var infRawData = await _raw.AppendRawDataAsync(ChainName, UUID.NoHyphens, $"{infRawChange.Result}", "Some metadta".ToHex());

            Assert.IsNull(infRawData.Error);
            Assert.IsNotNull(infRawData.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infRawData);

            var infSignRaw = await _raw.SignRawTransactionAsync(ChainName, UUID.NoHyphens, $"{infRawData.Result}");

            Assert.IsNull(infSignRaw.Error);
            Assert.IsNotNull(infSignRaw.Result);
            Assert.IsInstanceOf<RpcResponse<SignRawTransactionResult>>(infSignRaw);

            var infSendRaw = await _raw.SendRawTransactionAsync(ChainName, UUID.NoHyphens, infSignRaw.Result.Hex, false);

            Assert.IsNull(infSendRaw.Error);
            Assert.IsNotNull(infSendRaw.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(infSendRaw);


            // Inferred blockchain name tests

            var expAssetModel_0 = new AssetEntity();
            var expAssetModel_1 = new AssetEntity();

            var expAsset_0 = await _wallet.IssueAsync(Address, expAssetModel_0, 100, 1, 0, new Dictionary<string, string> { { "text", "Some text in Hex".ToHex() } });

            Assert.IsNull(expAsset_0.Error);
            Assert.IsNotEmpty(expAsset_0.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expAsset_0);

            var expAsset_1 = await _wallet.IssueAsync(Address, expAssetModel_1, 100, 1, 0, new Dictionary<string, string> { { "text", "Some text in Hex".ToHex() } });

            Assert.IsNull(expAsset_1.Error);
            Assert.IsNotEmpty(expAsset_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expAsset_1);

            var expNewAddress_0 = await _wallet.GetNewAddressAsync();

            Assert.IsNull(expNewAddress_0.Error);
            Assert.IsNotEmpty(expNewAddress_0.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expNewAddress_0);

            var expNewAddress_1 = await _wallet.GetNewAddressAsync();

            Assert.IsNull(expNewAddress_1.Error);
            Assert.IsNotEmpty(expNewAddress_1.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expNewAddress_1);


            var expGrant = await _wallet.GrantFromAsync(Address, $"{expNewAddress_0.Result},{expNewAddress_1.Result}", $"{Permission.Receive},{Permission.Send}", 0, 1, Permission.MaxEndblock, "Comment", "CommentTo");

            Assert.IsNull(expGrant.Error);
            Assert.IsNotNull(expGrant.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expGrant);

            var expListUnspent = await _wallet.ListUnspentAsync(0, 9999, new[] { Address });

            Assert.IsNull(expListUnspent.Error);
            Assert.IsNotNull(expListUnspent.Result);
            Assert.IsInstanceOf<RpcResponse<ListUnspentResult[]>>(expListUnspent);

            var expUnspentAsset_0 = expListUnspent.Result.SingleOrDefault(s => s.Assets.Any(a => a.Name == expAssetModel_0.Name));
            var expUnspentAsset_1 = expListUnspent.Result.SingleOrDefault(s => s.Assets.Any(a => a.Name == expAssetModel_1.Name));

            var expCreateRaw = await _raw.CreateRawTransactionAsync(new object[]
            {
                new Dictionary<string, object>
                {
                    { "txid", expUnspentAsset_0.Txid },
                    { "vout", expUnspentAsset_0.Vout }
                },
                new Dictionary<string, object>
                {
                    { "txid", expUnspentAsset_1.Txid },
                    { "vout", expUnspentAsset_1.Vout }
                }
            },
            assets: new Dictionary<string, Dictionary<string, int>>
            {
                {
                    expNewAddress_0.Result, new Dictionary<string, int>
                    {
                        { expAssetModel_0.Name, 1 },
                        { expAssetModel_1.Name, 2 }
                    }
                },
                {
                    expNewAddress_1.Result, new Dictionary<string, int>
                    {
                        { expAssetModel_0.Name, 3 },
                        { expAssetModel_1.Name, 4 }
                    }
                }
            }, Array.Empty<object>(), string.Empty);

            Assert.IsNull(expCreateRaw.Error);
            Assert.IsNotNull(expCreateRaw.Result);
            Assert.IsInstanceOf<RpcResponse<object>>(expCreateRaw);

            var expDecode = await _raw.DecodeRawTransactionAsync($"{expCreateRaw.Result}");

            Assert.IsNull(expDecode.Error);
            Assert.IsNotNull(expDecode.Result);
            Assert.IsInstanceOf<RpcResponse<DecodeRawTransactionResult>>(expDecode);

            var expRawChange = await _raw.AppendRawChangeAsync($"{expCreateRaw.Result}", Address, 0);

            Assert.IsNull(expRawChange.Error);
            Assert.IsNotNull(expRawChange.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expRawChange);

            var expRawData = await _raw.AppendRawDataAsync($"{expRawChange.Result}", "Some metadta".ToHex());

            Assert.IsNull(expRawData.Error);
            Assert.IsNotNull(expRawData.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expRawData);

            var expSignRaw = await _raw.SignRawTransactionAsync($"{expRawData.Result}");

            Assert.IsNull(expSignRaw.Error);
            Assert.IsNotNull(expSignRaw.Result);
            Assert.IsInstanceOf<RpcResponse<SignRawTransactionResult>>(expSignRaw);

            var expSendRaw = await _raw.SendRawTransactionAsync(expSignRaw.Result.Hex, false);

            Assert.IsNull(expSendRaw.Error);
            Assert.IsNotNull(expSendRaw.Result);
            Assert.IsInstanceOf<RpcResponse<string>>(expSendRaw);
        }
    }
}
