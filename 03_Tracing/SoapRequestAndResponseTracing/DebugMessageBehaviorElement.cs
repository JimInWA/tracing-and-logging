namespace SoapRequestAndResponseTracing
{
    using System;
    using System.ServiceModel.Configuration;
    using System.ServiceModel.Dispatcher;
    using SoapRequestAndResponseTracing.Interfaces;

    /// <summary>
    /// DebugMessageBehaviorElement class - extends BehaviorExtensionElement abstract class
    /// </summary>
    public class DebugMessageBehaviorElement : BehaviorExtensionElement
    {
        private readonly IHelper _helper;
        private readonly ILogger _logger;
        private readonly IClientMessageInspector _clientMessageInspector;
        private readonly IDispatchMessageInspector _debugMessageDispatcher;

        /// <summary>
        /// DebugMessageBehavior constructor
        /// Created a parameterless constructor as the WCF Service wouldn't start if I didn't.
        /// I assume that I have something wired wrong for Autofac, but I couldn't figure it out.
        /// </summary>
        public DebugMessageBehaviorElement()
        {
            _helper = new Helper();
            _logger = new Logger();
            _clientMessageInspector = new DebugMessageInspector(_helper, _logger);
            _debugMessageDispatcher = new DebugMessageDispatcher(_helper, _logger);
        }

        /// <summary>
        /// DebugMessageBehavior constructor
        /// </summary>
        public DebugMessageBehaviorElement(IHelper helper, ILogger logger, IClientMessageInspector clientMessageInspector, IDispatchMessageInspector debugMessageDispatcher)
        {
            _helper = helper;
            _logger = logger;
            _clientMessageInspector = clientMessageInspector;
            _debugMessageDispatcher = debugMessageDispatcher;
        }

        /// <summary>
        /// CreateBehavior method, overrides base method
        /// </summary>
        /// <returns></returns>
        protected override object CreateBehavior()
        {
            return new DebugMessageBehavior(_clientMessageInspector, _debugMessageDispatcher);
        }

        /// <summary>
        /// BehaviorType public property, overrides base property
        /// </summary>
        /// <returns></returns>
        public override Type BehaviorType
        {
            get
            {
                return typeof(DebugMessageBehavior);
            }
        }
    }

}
