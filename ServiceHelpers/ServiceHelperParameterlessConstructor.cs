using MCWrapper.RPC.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace MCWrapper.RPC.Tests.ServiceHelpers
{
    public class ServiceHelperParameterlessConstructor
    {
        /// <summary>
        /// Service provider container; persistent between calls;
        /// </summary>
        private ServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Services container
        /// </summary>
        private ServiceCollection ServiceCollection { get; set; } = new ServiceCollection();

        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceHelperParameterlessConstructor()
        {
            // Add MultiChain library services to the collection
            ServiceCollection.AddMultiChainCoreRpcServices();

            // build and store Service provider
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Locate and return service type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetService<T>() => ServiceProvider.GetService<T>();

        /// <summary>
        /// Managed objects
        /// </summary>
        public void Dispose()
        {
            ServiceProvider.Dispose();
            ServiceCollection.Clear();
        }
    }
}
