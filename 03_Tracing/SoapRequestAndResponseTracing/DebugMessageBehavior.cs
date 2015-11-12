namespace SoapRequestAndResponseTracing
{
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    /// <summary>
    /// DebugMessageBehavior class - implements IEndpointBehavior interface
    /// </summary>
    public class DebugMessageBehavior : IEndpointBehavior
    {
        private readonly IClientMessageInspector _clientMessageInspector;
        private readonly IDispatchMessageInspector _debugMessageDispatcher;
        
        /// <summary>
        /// DebugMessageBehavior constructor
        /// </summary>
        public DebugMessageBehavior(IClientMessageInspector clientMessageInspector, IDispatchMessageInspector debugMessageDispatcher)
        {
            _clientMessageInspector = clientMessageInspector;
            _debugMessageDispatcher = debugMessageDispatcher;
        }

        /// <summary>
        /// AddBindingParameters method
        /// Note: not used in this example, but needs to be defined to implement the interface
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// ApplyClientBehavior method
        /// Used when a client call is being traced and logged
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="clientRuntime"></param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            // add the inspector to the client runtime
            clientRuntime.MessageInspectors.Add(_clientMessageInspector);
        }

        /// <summary>
        /// ApplyDispatchBehavior method
        /// Used when a service call is being traced and logged
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="endpointDispatcher"></param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(_debugMessageDispatcher);
        }

        /// <summary>
        /// Validate method
        /// Note: not used in this example, but needs to be defined to implement the interface
        /// </summary>
        /// <param name="endpoint"></param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
