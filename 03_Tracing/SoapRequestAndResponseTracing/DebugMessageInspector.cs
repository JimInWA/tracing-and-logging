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
    public class DebugMessageInspector : IClientMessageInspector
    {
        private readonly IHelper _helper;
        private readonly ILogger _logger;

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
            var requestBuffer = request.CreateBufferedCopy(Int32.MaxValue);
            var requestCopyForLogging = requestBuffer.CreateMessage();
            request = requestBuffer.CreateMessage();

            try
            {
                // Since this is .NET 4.0, cannot use Task.Run
                // Using Task.Factory.StartNew instead
                Task.Factory.StartNew(() => StartLoggingTheRequest(requestCopyForLogging));
            }
            catch
            {
                // ToDo: Log an issue to the event log
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
        public void StartLoggingTheRequest(Message requestCopyForLogging)
        {
            try
            {
                if (_helper.ShouldLogSoapRequestsAndResponses())
                {
                    // ToDo: Get rid of the magic strings
                    const string outgoingRequestText = "outgoing request";

                    Guid urn;

                    if (requestCopyForLogging.Headers.MessageId != null)
                    {
                        var possibleUrn = _helper.StripFormattingFromHeaderMessageId(requestCopyForLogging.Headers.MessageId.ToString());
                        if (!Guid.TryParseExact(possibleUrn, "D", out urn))
                        {
                            urn = new Guid();
                        }
                    }
                    else
                    {
                        urn = new Guid();
                    }

                    _logger.Log("MVC Client Side", outgoingRequestText, urn, requestCopyForLogging);
                }
            }
            catch
            {
                // ToDo: Log an issue to the event log
                throw;
            }
        }

        /// <summary>
        /// AfterReceiveReply method
        /// When a client call to a web service is being traced and logged, this is called after the service has processed the request but before the response is sent back to the client
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            var replyBuffer = reply.CreateBufferedCopy(Int32.MaxValue);
            var replyCopyForLogging = replyBuffer.CreateMessage();
            reply = replyBuffer.CreateMessage();

            try
            {
                // Since this is .NET 4.0, cannot use Task.Run
                // Using Task.Factory.StartNew instead
                Task.Factory.StartNew(() => StartLoggingTheReply(replyCopyForLogging)); ;
            }
            catch
            {
                // ToDo: Log an issue to the event log
                throw;
            }
        }

        /// <summary>
        /// StartLoggingTheReply method
        /// Used to start the process of logging with a copy of the reply Message object 
        /// Exposed a public method to allow for consumption by other behavior extensions
        /// </summary>
        /// <param name="replyCopyForLogging"></param>
        public void StartLoggingTheReply(Message replyCopyForLogging)
        {
            try
            {
                if (_helper.ShouldLogSoapRequestsAndResponses())
                {
                    // ToDo: Get rid of the magic strings
                    const string incomingReplyText = "incoming reply";

                    Guid urn;

                    if (replyCopyForLogging.Headers.RelatesTo != null)
                    {
                        var possibleUrn = _helper.StripFormattingFromHeaderMessageId(replyCopyForLogging.Headers.RelatesTo.ToString());

                        if (!Guid.TryParseExact(possibleUrn, "D", out urn))
                        {
                            urn = new Guid();
                        }
                    }
                    else
                    {
                        urn = new Guid();
                    }

                    _logger.Log("MVC Client Side", incomingReplyText, urn, replyCopyForLogging);
                }
            }
            catch
            {
                throw;
                // ToDo: Log an issue to the event log
            }
        }
    }
}
