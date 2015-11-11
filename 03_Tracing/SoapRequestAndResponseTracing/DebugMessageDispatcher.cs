using System;
using System.Threading.Tasks;

namespace SoapRequestAndResponseTracing
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using System.Text;

    /// <summary>
    /// DebugMessageDispatcher class - implements IDispatchMessageInspector interface
    /// </summary>
    public class DebugMessageDispatcher : IDispatchMessageInspector
    {
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
            var myHelper = new Helper();

            if (myHelper.ShouldLogSoapRequestsAndResponses())
            {
                var myLogger = new Logger();

                // ToDo: Get rid of the magic strings
                var incomingRequestText = new StringBuilder();
                incomingRequestText.Append("incoming request");

                if (requestCopyForLogging.Headers.MessageId != null)
                {
                    incomingRequestText.AppendFormat(" ({0})",
                        myHelper.StripFormattingFromHeaderMessageId(requestCopyForLogging.Headers.MessageId.ToString()));
                }

                myLogger.Log("WCF Server Side", incomingRequestText, requestCopyForLogging);
            }
        }

        /// <summary>
        /// BeforeSendReply method
        /// When a service call is being traced and logged, this is called after the service has processed the request but before the response is sent back to the client
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var replyBuffer = reply.CreateBufferedCopy(Int32.MaxValue);
            var replyCopyForLogging = replyBuffer.CreateMessage();
            reply = replyBuffer.CreateMessage();

            // Since this is .NET 4.0, cannot use Task.Run
            // Using Task.Factory.StartNew instead
            Task.Factory.StartNew(() => StartLoggingTheReply(replyCopyForLogging));
        }

        /// <summary>
        /// StartLoggingTheReply method
        /// Used to start the process of logging with a copy of the reply Message object 
        /// Exposed a public method to allow for consumption by other behavior extensions
        /// </summary>
        /// <param name="replyCopyForLogging"></param>
        public void StartLoggingTheReply(Message replyCopyForLogging)
        {
            var myHelper = new Helper();

            if (myHelper.ShouldLogSoapRequestsAndResponses())
            {
                var myLogger = new Logger();

                // ToDo: Get rid of the magic strings
                var outgoingReplyText = new StringBuilder();
                outgoingReplyText.Append("outgoing reply");

                if (replyCopyForLogging.Headers.RelatesTo != null)
                {
                    outgoingReplyText.AppendFormat(" ({0})",
                        myHelper.StripFormattingFromHeaderRelatesTo(replyCopyForLogging.Headers.RelatesTo.ToString()));
                }

                myLogger.Log("WCF Server Side", outgoingReplyText, replyCopyForLogging);
            }
        }
    }

}
