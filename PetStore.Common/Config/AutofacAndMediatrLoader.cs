using Autofac;
using Serilog;
using System;
using System.Collections.Generic;
using Petstore.Common.Utils;

namespace Petstore.Common.Config
{
    /// <summary>
    /// This is a class which will act like the "Glue" of propertly configuring a Test suite or Module for properly using Autofact to load dependencies.
    /// </summary>
    public class AutofacAndMediatrLoader
    {
        /// <summary>
        /// Callback method for performing configuration before or after the rest of the Autofac Modules load. 
        /// </summary>
        public delegate void LoadCallback(ContainerBuilder containerBuilder);

        /// <summary>
        /// Expose the builder for use in the unit tests.
        /// </summary>
        public ContainerBuilder ContainerBuilder;

        /// <summary>
        /// <para>Expose the builder for use in the unit tests.</para>
        /// 
        /// Example: 
        /// <code>
        ///     //Set up AutoFac
        ///     AutofacAndMediatorModuleForAPI autoFacModuule = new AutofacAndMediatorModuleForAPI(new HttpConfiguration());
        ///
        ///     // Get our IOC to return us a fresh copy of Mediator. 
        ///     var mediator = autoFacModuule.Container.Resolve<IMediator>();
        /// </code>
        /// 
        /// </summary>
        public IContainer Container;

        public AutofacAndMediatrLoader()
        {
        }

        /// <param name="config"> The Http Configuration</param>
        /// <param name="autoFacModules">The Auto Fac modules you wish to load</param>
        /// <param name="preLoadConfig">A callback that happens before the autoFacModules are loaded. </param>
        /// <param name="postLoadConfig">A callback after the autoFacModules are loaded</param>
        public void Load(IEnumerable<Autofac.Module> autoFacModules, LoadCallback preLoadConfig = null, LoadCallback postLoadConfig = null, ContainerBuilder builder = null)
        {
            try
            {
                ContainerBuilder = (builder == null) ? new ContainerBuilder() : builder;

                // Initialize the module
                if (preLoadConfig != null)
                {
                    preLoadConfig(ContainerBuilder);
                }

                // 1) Add the autofact modules
                foreach (Autofac.Module module in autoFacModules)
                {
                    ContainerBuilder.RegisterModule(module);
                }

                // complete the configuration
                if (postLoadConfig != null)
                {
                    postLoadConfig(ContainerBuilder);
                }

            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }
        }
    }
}
