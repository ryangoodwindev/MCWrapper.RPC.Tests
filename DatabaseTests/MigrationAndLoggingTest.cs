using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Extensions;
using MCWrapper.RPC.Connection;
using MCWrapper.RPC.Ledger.Clients;
using MCWrapper.RPC.Test.ServicesPipeline;
using Myndblock.MultiChain.Database;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCWrapper.RPC.Tests.DatabaseTests
{
    [TestFixture]
    public class MigrationAndLoggingTest
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
        private readonly ITransactionRepo _contract;
        private readonly string _chainName;
        private readonly string _address;

        // Use mock startup service container
        private readonly ExplicitStartupWithDatabase _services = new ExplicitStartupWithDatabase();

        // Create a new RpcControlClientTests instance
        public MigrationAndLoggingTest()
        {
            _wallet = _services.GetRequiredService<IMultiChainRpcWallet>();
            _contract = _services.GetRequiredService<ITransactionRepo>();
            _address = _wallet.RpcOptions.ChainAdminAddress;
            _chainName = _wallet.RpcOptions.ChainName;
        }

        [Test]
        public async Task TransactionLogTestAsync()
        {
            /*
               Explicit blockchain name test
            */

            // Act - Ask network for blockchain params
            var exp = await _wallet.IssueAsync(_chainName, UUID.NoHyphens, _address, new AssetEntity(), 100, 1, 0, new Dictionary<string, string>() { { "text", "Test data text".ToHex() } });
            var transaction_0 = NewTransaction(_chainName, nameof(_wallet.IssueAsync), txid: exp.Result);

            // Assert
            Assert.DoesNotThrowAsync(async () => await _contract.CreateAsync(transaction_0));
            Assert.IsTrue(exp.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(exp);
            Assert.IsTrue(transaction_0.Id.HasValue);

            /*
               Inferred blockchain name test
            */

            // Act - Ask network for blockchain params
            var inf = await _wallet.IssueAsync(_address, new AssetEntity(), 100, 1, 0, new Dictionary<string, string>() { { "text", "Test data text".ToHex() } });
            var transaction_1 = NewTransaction(_chainName, nameof(_wallet.IssueAsync), txid: inf.Result);

            // Assert
            Assert.DoesNotThrowAsync(async () => await _contract.CreateAsync(transaction_1));
            Assert.IsTrue(inf.IsSuccess());
            Assert.IsInstanceOf<RpcResponse<string>>(inf);
            Assert.IsTrue(transaction_1.Id.HasValue);
        }

        private TransactionModel NewTransaction(string blockchain, string targetMethod, string txid)
        {
            return new TransactionModel(blockchain, targetMethod, txid)
            {
                CreatedBy = nameof(TransactionLogTestAsync),
                LastModifiedBy = nameof(TransactionLogTestAsync)
            };
        }
    }
}
