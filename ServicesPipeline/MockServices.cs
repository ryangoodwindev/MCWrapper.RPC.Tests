using MCWrapper.RPC.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MCWrapper.RPC.Tests.ServicesPipeline
{
    public class ParameterlessMockServices : MockServices
    {
        public ParameterlessMockServices()
        {
            // Add MultiChain library services to the collection.
            // Parameterless constructor use indicates that we will
            // look to the local environment store for configuration values
            Services.AddMultiChainCoreRpcServices();
        }
    }

    public class ExplicitMockServices : MockServices
    {
        public ExplicitMockServices()
        {
            // Add MultiChain library services to the collection
            // Our values are null and empty since our local testing environment has the necessary
            // variables preloaded as environment variables. This just demonstrates how a 
            // consumer would go about implementing this type of service container injection. If
            // environment variables are not available then the values listed below MUST be 
            // explicitly set when using this specific and explicit extension method.
            //
            // Even though it is not necessary since there are no values to pass, we still
            // implemented a RuntimeParamOptions instance and passed it to the service container. This
            // parameter is entirely option.
            //
            Services.AddMultiChainCoreRpcServices(profile =>
            {
                profile.ChainUseSsl = false;
                profile.ChainUsername = "multichainrpc";
                profile.ChainPassword = "8qLZFDz65RBCDofXRjBDRDJoKy5y3weowZy6HQ3ej9tr"; // example password
                profile.ChainName = "Version3Chain"; // example blockchain name
                profile.ChainRpcPort = 8384; // example port
                profile.ChainAdminAddress = "1QsgeUKXKBR7hA8ey5j9qgPT1faQetLDUXi3Pn"; // example admin address
                profile.ChainHostname = "localhost";
                profile.ChainBurnAddress = "1XXXXXXXSXXXXXXXSnXXXXXXZvXXXXXXZ3Mi3Q"; // example burn address
            });
        }
    }

    public class IConfigurationMockServices : MockServices
    {
        public IConfigurationMockServices()
        {
            // fetch JSON config values
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            // Configuration interface
            IConfiguration configuration = builder.Build();

            // Add MultiChain library services to the collection
            Services.AddMultiChainCoreRpcServices(configuration: configuration);
        }
    }

    public class MockServices
    {
        private ServiceProvider _serviceProvider;
        
        public MockServices() { }

        public ServiceCollection Services 
        {
            get => _services; 
            set => _services = value; 
        }
        private ServiceCollection _services = new ServiceCollection();

        /// <summary>
        /// Locate and return service type
        /// </summary>
        /// <typeparam name="TRpc"></typeparam>
        /// <returns></returns>
        public TRpc GetRequiredService<TRpc>()
        {
            if (_serviceProvider == null)
                _serviceProvider = Services.BuildServiceProvider();

            return _serviceProvider.GetRequiredService<TRpc>();
        }
    }
}
