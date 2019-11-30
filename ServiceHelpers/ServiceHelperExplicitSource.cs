using MCWrapper.RPC.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace MCWrapper.RPC.Tests.ServiceHelpers
{
    public class ServiceHelperExplicitSource
    {
        /// <summary>
        /// Service provider container; persistent between calls;
        /// </summary>
        private ServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Services container
        /// </summary>
        private ServiceCollection ServiceCollection { get; set; } 
            = new ServiceCollection();

        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceHelperExplicitSource()
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
            ServiceCollection.AddMultiChainCoreRpcServices(profile =>
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

            // build and store Service provider
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Locate and return service type
        /// </summary>
        /// <typeparam name="IMultiChainRpc"></typeparam>
        /// <returns></returns>
        public IMultiChainRpc GetService<IMultiChainRpc>() => 
            ServiceProvider.GetService<IMultiChainRpc>();
    }
}
