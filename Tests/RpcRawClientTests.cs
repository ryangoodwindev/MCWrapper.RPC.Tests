using MCWrapper.Data.Models.Raw;
using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Test.ServicesPipeline;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Test.Raw
{
    [TestFixture]
    public class RpcRawClientTests
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
        private readonly IMultiChainRpcRaw _raw;
        private readonly string _chainName;
        private readonly string _address;

        // Use mock startup service container
        private readonly ExplicitStartup _services = new ExplicitStartup();

        // Create a new RpcRawClientTests instance
        public RpcRawClientTests()
        {
            _wallet = _services.GetRequiredService<IMultiChainRpcWallet>();
            _raw = _services.GetRequiredService<IMultiChainRpcRaw>();
            _address = _wallet.RpcOptions.ChainAdminAddress;
            _chainName = _wallet.RpcOptions.ChainName;
        }

        [Test]
        public async Task RawTransactionTest()
        {
            /*
               Explicit blockchain name test
            */

            var infAssetModel_0 = new AssetEntity();
            var infAssetModel_1 = new AssetEntity();

            var infAsset_0 = await _wallet.IssueAsync(_chainName, UUID.NoHyphens, _address, infAssetModel_0, 100, 1, 0, new Dictionary<string, string> { { "text", "Some text in Hex".ToHex() } });
            var infAsset_1 = await _wallet.IssueAsync(_chainName, UUID.NoHyphens, _address, infAssetModel_1, 100, 1, 0, new Dictionary<string, string> { { "text", "Some text in Hex".ToHex() } });

            var infNewAddress_0 = await _wallet.GetNewAddressAsync(_chainName, UUID.NoHyphens);
            var infNewAddress_1 = await _wallet.GetNewAddressAsync(_chainName, UUID.NoHyphens);

            var infGrant = await _wallet.GrantFromAsync(_chainName, UUID.NoHyphens, _address, $"{infNewAddress_0.Result},{infNewAddress_1.Result}", $"{Permission.Receive},{Permission.Send}", 0, 1, 20000, "Comment", "CommentTo");

            var infListUnspent = await _wallet.ListUnspentAsync(_chainName, UUID.NoHyphens, 0, 9999, new[] { _address });

            var unspentAsset_0 = infListUnspent.Result.SingleOrDefault(s => s.Assets.Any(a => a.Name == infAssetModel_0.name));
            var unspentAsset_1 = infListUnspent.Result.SingleOrDefault(s => s.Assets.Any(a => a.Name == infAssetModel_1.name));
            var infCreateRaw = await _raw.CreateRawTransactionAsync(_chainName, UUID.NoHyphens, new object[]
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
                        { infAssetModel_0.name, 1 },
                        { infAssetModel_1.name, 2 }
                    }
                },
                {
                    infNewAddress_1.Result, new Dictionary<string, int>
                    {
                        { infAssetModel_0.name, 3 },
                        { infAssetModel_1.name, 4 }
                    }
                }
            }, Array.Empty<object>(), string.Empty);

            Assert.IsTrue(infCreateRaw.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(infCreateRaw);

            var infDecode = await _raw.DecodeRawTransactionAsync(_chainName, UUID.NoHyphens, $"{infCreateRaw.Result}");

            Assert.IsTrue(infDecode.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<DecodeRawTransactionResult>>(infDecode);

            var infRawChange = await _raw.AppendRawChangeAsync(_chainName, UUID.NoHyphens, $"{infCreateRaw.Result}", _address, 0);

            Assert.IsTrue(infRawChange.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(infRawChange);

            var infRawData = await _raw.AppendRawDataAsync(_chainName, UUID.NoHyphens, $"{infRawChange.Result}", "Some metadta".ToHex());

            Assert.IsTrue(infRawData.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(infRawData);

            var infSignRaw = await _raw.SignRawTransactionAsync(_chainName, UUID.NoHyphens, $"{infRawData.Result}");

            Assert.IsTrue(infSignRaw.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<SignRawTransactionResult>>(infSignRaw);

            var infSendRaw = await _raw.SendRawTransactionAsync(_chainName, UUID.NoHyphens, infSignRaw.Result.Hex, false);

            Assert.IsTrue(infSendRaw.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(infSendRaw);

            /*
               Inferred blockchain name test
            */

            var expAssetModel_0 = new AssetEntity();
            var expAssetModel_1 = new AssetEntity();

            var expAsset_0 = await _wallet.IssueAsync(_address, expAssetModel_0, 100, 1, 0, new Dictionary<string, string> { { "text", "Some text in Hex".ToHex() } });
            var expAsset_1 = await _wallet.IssueAsync(_address, expAssetModel_1, 100, 1, 0, new Dictionary<string, string> { { "text", "Some text in Hex".ToHex() } });

            var expNewAddress_0 = await _wallet.GetNewAddressAsync();
            var expNewAddress_1 = await _wallet.GetNewAddressAsync();

            var expGrant = await _wallet.GrantFromAsync(_address, $"{expNewAddress_0.Result},{expNewAddress_1.Result}", $"{Permission.Receive},{Permission.Send}", 0, 1, Permission.MaxEndblock, "Comment", "CommentTo");

            var expListUnspent = await _wallet.ListUnspentAsync(0, 9999, new[] { _address });

            var expUnspentAsset_0 = expListUnspent.Result.SingleOrDefault(s => s.Assets.Any(a => a.Name == expAssetModel_0.name));
            var expUnspentAsset_1 = expListUnspent.Result.SingleOrDefault(s => s.Assets.Any(a => a.Name == expAssetModel_1.name));

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
                        { expAssetModel_0.name, 1 },
                        { expAssetModel_1.name, 2 }
                    }
                },
                {
                    expNewAddress_1.Result, new Dictionary<string, int>
                    {
                        { expAssetModel_0.name, 3 },
                        { expAssetModel_1.name, 4 }
                    }
                }
            }, Array.Empty<object>(), string.Empty);

            Assert.IsTrue(expCreateRaw.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<object>>(expCreateRaw);

            var expDecode = await _raw.DecodeRawTransactionAsync($"{expCreateRaw.Result}");

            Assert.IsTrue(expDecode.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<DecodeRawTransactionResult>>(expDecode);

            var expRawChange = await _raw.AppendRawChangeAsync($"{expCreateRaw.Result}", _address, 0);

            Assert.IsTrue(expRawChange.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(expRawChange);

            var expRawData = await _raw.AppendRawDataAsync($"{expRawChange.Result}", "Some metadta".ToHex());

            Assert.IsTrue(expRawData.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(expRawData);

            var expSignRaw = await _raw.SignRawTransactionAsync($"{expRawData.Result}");

            Assert.IsTrue(expSignRaw.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<SignRawTransactionResult>>(expSignRaw);

            var expSendRaw = await _raw.SendRawTransactionAsync(expSignRaw.Result.Hex, false);

            Assert.IsTrue(expSendRaw.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(expSendRaw);
        }
    }
}
