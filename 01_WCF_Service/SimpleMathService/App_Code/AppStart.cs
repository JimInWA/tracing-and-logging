namespace SimpleMathService.App_Code
{
    using Autofac;
    using System.Linq;
    using System.Reflection;
    using System.Web.Compilation;

    /// <summary>
    /// AppStart class - configure and register Autofac IoC container to run in application start.
    /// </summary>
    public class AppStart
    {
        /// <summary>
        /// Gets or sets the container reference.
        /// </summary>
        /// <value>
        /// The container reference.
        /// </value>
        public static IContainer ContainerReference { get; private set; }

        /// <summary>
        /// AppInitialize method - register the Autofac IoC Container
        /// </summary>
        public static void AppInitialize()
        {
            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();

            // configure Autofac
            ContainerReference = ConfigureDependencyInjection(assemblies);
        }

        private static IContainer ConfigureDependencyInjection(Assembly[] assemblies)
        {
            var builder = new ContainerBuilder();

            // register modules from assemblies
            builder.RegisterAssemblyModules(assemblies);

            return builder.Build();
        }
    }
}