using MCWrapper.RPC.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MCWrapper.RPC.Test.ServicesPipeline
{
    public class ExplicitStartup : MockStartup
    {
        /// <summary>
        /// 
        /// ExplicitMockServices adds MultiChain RPC services to the test bank's
        /// service container using the explicit .AddMultiChainCoreRpcServices(RpcOptions, RuntimeParamOptions) 
        /// extension method available from the MCWrapper.RPC.Extensions namespace.
        /// 
        /// <para>
        ///     The explicit extension method will cause MCWrapper.RPC to load the
        ///     RpcOptions configuration values explicitly from the passed Action
        ///     RpcOptions parameters.
        /// </para>
        /// <para>Your explicit MultiChain network parameters will differ from the example values listed below.</para>
        /// 
        /// </summary>
        public ExplicitStartup()
        {
            Services.AddMultiChainCoreRpcServices(rpcOptions =>
            {
                rpcOptions.ChainHostname = "localhost";
                rpcOptions.ChainRpcPort = 8384;
                rpcOptions.ChainUseSsl = true;

                rpcOptions.ChainName = "CurrencyTestCoin";
                rpcOptions.ChainUsername = "multichainrpc";
                rpcOptions.ChainPassword = "Rfy4Q2Wbf35DE3ZLvUtGYRD8erByPsjd4gKyuNFkWx9";

                rpcOptions.ChainAdminAddress = "15UxmgMF9AM7JcXZKn4JcKutuQ6q7iSNp4RHVg";
                rpcOptions.ChainBurnAddress = "1XXXXXXXatXXXXXXP9XXXXXXTdXXXXXXXMSFht";
            });
        }
    }

    public class IConfigurationStartup : MockStartup
    {
        /// <summary>
        /// 
        /// IConfigurationMockServices adds MultiChain RPC services to the test bank's
        /// service container using the IConfiguration pipeline
        /// .AddMultiChainCoreRpcServices(IConfiguration configuration, bool useSecrets = false) 
        /// extension method available from the MCWrapper.RPC.Extensions namespace.
        /// 
        /// <para>
        ///     The IConfiguration extension method will cause MCWrapper.RPC to load 
        ///     the RpcOptions configuration values from an external json setttings file.
        ///     Usually appsettings.json.
        /// </para>
        /// <para>Support for Secret Manager is attempted but not tested yet.</para>
        /// </summary>
        public IConfigurationStartup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            Services.AddMultiChainCoreRpcServices(configuration, useSecrets: false);
        }
    }

    public class SecretsManagerStartup : MockStartup
    {
        /// <summary>
        /// 
        /// IConfigurationMockServices adds MultiChain RPC services to the test bank's
        /// service container using the IConfiguration pipeline
        /// .AddMultiChainCoreRpcServices(IConfiguration configuration, bool useSecrets = false) 
        /// extension method available from the MCWrapper.RPC.Extensions namespace.
        /// 
        /// <para>
        ///     The IConfiguration extension method will cause MCWrapper.RPC to load 
        ///     the RpcOptions configuration values from an external json setttings file.
        ///     Usually appsettings.json.
        /// </para>
        /// <para>Support for Secret Manager is attempted but not tested yet.</para>
        /// </summary>
        public SecretsManagerStartup()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets("Enter your local secret manager id here or configure otherwise.");

            IConfiguration configuration = builder.Build();

            Services.AddMultiChainCoreRpcServices(configuration, useSecrets: true);
        }
    }

    public class ParameterlessStartup : MockStartup
    {
        /// <summary>
        /// 
        /// ParameterlessMockServices adds MultiChain RPC services to the test bank's
        /// service container using the parameterless .AddMultiChainCoreRpcServices() 
        /// extension method available from the MCWrapper.RPC.Extensions namespace.
        /// 
        /// <para>
        ///     The parameterless extension method 
        ///     will cause MCWrapper.RPC to look to the local machine's environment 
        ///     store for the RpcOptions configuration values.
        /// </para>
        /// 
        /// </summary>
        public ParameterlessStartup() =>
            Services.AddMultiChainCoreRpcServices();
    }

    /// <summary>
    /// Base class to MockService providers
    /// </summary>
    public abstract class MockStartup
    {
        /// <summary>
        /// Local copy of the ServiceProvider
        /// </summary>
        private ServiceProvider _serviceProvider;

        /// <summary>
        /// Base class to MockService providers
        /// </summary>
        public ServiceCollection Services 
        {
            get => _services; 
            set => _services = value; 
        }
        private ServiceCollection _services = new ServiceCollection();

        /// <summary>
        /// Locate and return service type
        /// </summary>
        /// <typeparam name="TRpc">Type of IMultiChainRpc client to GET</typeparam>
        /// <returns></returns>
        public TRpc GetRequiredService<TRpc>()
        {
            if (_serviceProvider == null)
                _serviceProvider = Services.BuildServiceProvider();

            return _serviceProvider.GetRequiredService<TRpc>();
        }
    }
}
