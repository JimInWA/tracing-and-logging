namespace SoapRequestAndResponseTracing.Configuration
{
    using Autofac;
    using System.Reflection;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using SoapRequestAndResponseTracing.Interfaces;

    /// <summary>
    /// SoapRequestAndResponseTracingIocModule class
    /// </summary>
    public class SoapRequestAndResponseTracingIocModule : Autofac.Module
    {
        /// <summary>
        /// Load method - SoapRequestAndResponseTracingIocModule's override of Module's Load method
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            // register BusisnessFacade implementation
            builder.RegisterType<Helper>()
                   .FindConstructorsWith(type => type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance))
                   .As<IHelper>();

            builder.RegisterType<Logger>()
                   .FindConstructorsWith(type => type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance))
                   .As<ILogger>();

            builder.RegisterType<DebugMessageInspector>()
                   .FindConstructorsWith(type => type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance))
                   .As<IClientMessageInspector>();

            builder.RegisterType<DebugMessageDispatcher>()
                   .FindConstructorsWith(type => type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance))
                   .As<IDispatchMessageInspector>();

            builder.RegisterType<DebugMessageBehavior>()
                   .FindConstructorsWith(type => type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance))
                   .As<IEndpointBehavior>();
        }

    }
}
