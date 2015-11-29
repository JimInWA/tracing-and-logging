namespace SoapRequestAndResponseTracing
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using System.Threading.Tasks;
    using SoapRequestAndResponseTracing.Interfaces;

    /// <summary>
    /// DebugMessageInspector class - implements IClientMessageInspector interface
    /// </summary>
    public class DebugMessageInspector : IClientMessageInspector, IMessageLogging
    {
        private readonly IHelper _helper;
        private readonly ILogger _logger;

        private const string _outgoingRequestText = "outgoing request";
        private const string _mvcClientSide = "MVC Client Side";
        private const string _incomingReplyText = "incoming reply";

        /// <summary>
        /// DebugMessageInspector constructor
        /// </summary>
        public DebugMessageInspector(IHelper helper, ILogger logger)
        {
            _helper = helper;
            _logger = logger;
        }

        /// <summary>
        /// BeforeSendRequest method
        /// When a client call to a web service is being traced and logged, this is called after the client has sent the request but before the request is processed by the service
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
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

                    result = _logger.Log(_mvcClientSide, _outgoingRequestText, urn, requestCopyForLogging);
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
        /// AfterReceiveReply method
        /// When a client call to a web service is being traced and logged, this is called after the service has processed the request but before the response is sent back to the client
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            try
            {
                var replyBuffer = reply.CreateBufferedCopy(Int32.MaxValue);
                var replyCopyForLogging = replyBuffer.CreateMessage();
                reply = replyBuffer.CreateMessage();

                // Since this is .NET 4.0, cannot use Task.Run
                // Using Task.Factory.StartNew instead
                Task.Factory.StartNew(() => StartLoggingTheReply(replyCopyForLogging)); ;
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

                    result = _logger.Log(_mvcClientSide, _incomingReplyText, urn, replyCopyForLogging);
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
