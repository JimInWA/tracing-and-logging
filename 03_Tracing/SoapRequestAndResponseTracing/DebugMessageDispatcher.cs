namespace SoapRequestAndResponseTracing
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using System.Threading.Tasks;
    using SoapRequestAndResponseTracing.Interfaces;

    /// <summary>
    /// DebugMessageDispatcher class - implements IDispatchMessageInspector interface
    /// </summary>
    public class DebugMessageDispatcher : IDispatchMessageInspector, IMessageLogging
    {
        private readonly IHelper _helper;
        private readonly ILogger _logger;

        private const string _incomingRequestText = "incoming request";
        private const string _wcfServerSide = "WCF Server Side";
        private const string _outgoingReplyText = "outgoing reply";

        /// <summary>
        /// DebugMessageDispatcher constructor
        /// </summary>
        public DebugMessageDispatcher(IHelper helper, ILogger logger)
        {
            _helper = helper;
            _logger = logger;
        }

        /// <summary>
        /// AfterReceiveRequest method
        /// When a service call is being traced and logged, this is called after the client has sent the request but before the request is processed by the service
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <param name="instanceContext"></param>
        /// <returns></returns>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            try
            {
                var requestBuffer = request.CreateBufferedCopy(Int32.MaxValue);
                var requestCopyForLogging = requestBuffer.CreateMessage();
                request = requestBuffer.CreateMessage();

                // Since this is .NET 4.0, cannot use Task.Run
                // Using Task.Factory.StartNew instead
                Task.Factory.StartNew(() => StartLoggingTheRequest(requestCopyForLogging));
            }
            catch (Exception ex)
            {
                _logger.Write(ex);
                throw;
            }

            return request;
        }

        /// <summary>
        /// StartLoggingTheRequest method
        /// Used to start the process of logging with a copy of the request Message object 
        /// Exposed a public method to allow for consumption by other behavior extensions
        /// </summary>
        /// <param name="requestCopyForLogging"></param>
        public bool StartLoggingTheRequest(Message requestCopyForLogging)
        {
            var result = false;

            try
            {
                if (_helper.ShouldLogSoapRequestsAndResponses())
                {
                    var urn = _helper.ProvideUrnFromHeaderMessageId(requestCopyForLogging.Headers.MessageId);

                    result = _logger.Log(_wcfServerSide, _incomingRequestText, urn, requestCopyForLogging);
                }
            }
            catch (Exception ex)
            {
                _logger.Write(ex);
                throw;
            }

            return result;
        }

        /// <summary>
        /// BeforeSendReply method
        /// When a service call is being traced and logged, this is called after the service has processed the request but before the response is sent back to the client
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            try
            {
                var replyBuffer = reply.CreateBufferedCopy(Int32.MaxValue);
                var replyCopyForLogging = replyBuffer.CreateMessage();
                reply = replyBuffer.CreateMessage();

                // Since this is .NET 4.0, cannot use Task.Run
                // Using Task.Factory.StartNew instead
                Task.Factory.StartNew(() => StartLoggingTheReply(replyCopyForLogging));
            }
            catch (Exception ex)
            {
                _logger.Write(ex);
                throw;
            }
        }

        /// <summary>
        /// StartLoggingTheReply method
        /// Used to start the process of logging with a copy of the reply Message object 
        /// Exposed a public method to allow for consumption by other behavior extensions
        /// </summary>
        /// <param name="replyCopyForLogging"></param>
        public bool StartLoggingTheReply(Message replyCopyForLogging)
        {
            var result = false;

            try
            {
                if (_helper.ShouldLogSoapRequestsAndResponses())
                {
                    var urn = _helper.ProvideUrnFromHeaderRelatesTo(replyCopyForLogging.Headers.RelatesTo);

                    result = _logger.Log(_wcfServerSide, _outgoingReplyText, urn, replyCopyForLogging);
                }
            }
            catch (Exception ex)
            {
                _logger.Write(ex);
                throw;
            }

            return result;
        }
    }
}
