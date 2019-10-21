using MCWrapper.RPC.Ledger.Clients.Wallet;
using MCWrapper.RPC.Ledger.Entities;
using MCWrapper.RPC.Tests.ServiceHelpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests.UserStories
{
    [TestFixture]
    public class BulkCreateAsset
    {
        // private fields
        private readonly WalletRpcClient Wallet;

        /// <summary>
        /// Create a new WalletServiceTests instance
        /// </summary>
        public BulkCreateAsset()
        {
            // instantiate test services provider
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch service from provider
            Wallet = provider.GetService<WalletRpcClient>();
        }

        [Test, Ignore("This test seems to really slow down the MultiChain ledger. More testing on a fresh blockchain is necessary")]
        public async Task CreateTenThousand()
        {
            var assets = new List<AssetEntity>();

            for (int i = 0; i < 10000; i++)
                assets.Add(new AssetEntity());

            foreach (var asset in assets)
                _ = await Wallet.IssueAsync(Wallet.BlockchainOptions.ChainAdminAddress, asset, 100, 1);
        }
    }
}