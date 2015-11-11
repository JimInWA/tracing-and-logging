using SoapRequestAndResponseTracing.Interfaces;

namespace SoapRequestAndResponseTracing
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using System.Text;
    using System.Threading.Tasks;

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
        public DebugMessageInspector()
        {
            _helper = new Helper();
            _logger = new Logger();
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

            // Since this is .NET 4.0, cannot use Task.Run
            // Using Task.Factory.StartNew instead
            Task.Factory.StartNew(() => StartLoggingTheRequest(requestCopyForLogging));

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
            if (_helper.ShouldLogSoapRequestsAndResponses())
            {
                // ToDo: Get rid of the magic strings
                var outgoingRequestText = new StringBuilder();
                outgoingRequestText.Append("outgoing request");

                if (requestCopyForLogging.Headers.MessageId != null)
                {
                    outgoingRequestText.AppendFormat(" ({0})",
                        _helper.StripFormattingFromHeaderRelatesTo(requestCopyForLogging.Headers.MessageId.ToString()));
                }

                _logger.Log("MVC Client Side", outgoingRequestText, requestCopyForLogging);
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

            // Since this is .NET 4.0, cannot use Task.Run
            // Using Task.Factory.StartNew instead
            Task.Factory.StartNew(() => StartLoggingTheReply(replyCopyForLogging)); ;
        }

        /// <summary>
        /// StartLoggingTheReply method
        /// Used to start the process of logging with a copy of the reply Message object 
        /// Exposed a public method to allow for consumption by other behavior extensions
        /// </summary>
        /// <param name="replyCopyForLogging"></param>
        public void StartLoggingTheReply(Message replyCopyForLogging)
        {
            if (_helper.ShouldLogSoapRequestsAndResponses())
            {
                // ToDo: Get rid of the magic strings
                var incomingReplyText = new StringBuilder();
                incomingReplyText.Append("incoming reply");

                if (replyCopyForLogging.Headers.RelatesTo != null)
                {
                    incomingReplyText.AppendFormat(" ({0})",
                        _helper.StripFormattingFromHeaderMessageId(replyCopyForLogging.Headers.RelatesTo.ToString()));
                }

                _logger.Log("MVC Client Side", incomingReplyText, replyCopyForLogging);
            }
        }
    }
}
