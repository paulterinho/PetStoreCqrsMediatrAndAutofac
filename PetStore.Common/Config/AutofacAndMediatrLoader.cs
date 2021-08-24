using Autofac;
using Serilog;
using System;
using System.Collections.Generic;
using Petstore.Swagger.Io.Common.Utils;

namespace Petstore.Swagger.Io.Common.Config
{
    /// <summary>
    /// This is a class which will act like the "Glue" of propertly configuring a Test suite or Module for properly using Autofact to load dependencies.
    /// </summary>
    public class AutofacAndMediatrLoader
    {
        /// <summary>
        /// Callback method for performing configuration before or after the rest of the Autofac Modules load. 
        /// </summary>
        public delegate void LoadPreCallback(ContainerBuilder containerBuilder);

        /// <summary>
        /// Callback method for performing configuration before or after the rest of the Autofac Modules load. 
        /// </summary>
        public delegate IContainer LoadPostCallback(ContainerBuilder containerBuilder);

        /// <summary>
        /// Expose the builder for use in the unit tests.
        /// </summary>
        public ContainerBuilder Builder;

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
        public void Load(IEnumerable<Autofac.Module> autoFacModules, LoadPreCallback preLoadConfig = null, LoadPostCallback postLoadConfig = null)
        {
            try
            {
                Builder = new ContainerBuilder();

                // Initialize the module
                if (preLoadConfig != null)
                {
                    preLoadConfig(Builder);
                }

                // 1) Add the autofact modules
                foreach (Autofac.Module module in autoFacModules)
                {
                    Builder.RegisterModule(module);
                }

                // complete the configuration
                if (postLoadConfig != null)
                {
                    Container = postLoadConfig(Builder);
                }

            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }
        }
    }
}
