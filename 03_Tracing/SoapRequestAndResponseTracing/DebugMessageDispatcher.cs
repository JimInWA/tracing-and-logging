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
            var myHelper = new Helper();

            if (myHelper.ShouldLogSoapRequestsAndResponses())
            {
                var myLogger = new Logger();
                
                // ToDo: Get rid of the magic strings
                var incomingRequestText = new StringBuilder();
                incomingRequestText.Append("incoming request");

                if (request.Headers.MessageId != null)
                {
                    incomingRequestText.AppendFormat(" ({0})", myHelper.StripFormattingFromHeaderMessageId(request.Headers.MessageId.ToString()));
                }
                
                request = myLogger.Log("WCF Server Side", incomingRequestText, request);
            }

            return request;
        }

        /// <summary>
        /// BeforeSendReply method
        /// When a service call is being traced and logged, this is called after the service has processed the request but before the response is sent back to the client
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var myHelper = new Helper();

            if (myHelper.ShouldLogSoapRequestsAndResponses())
            {
                var myLogger = new Logger();

                // ToDo: Get rid of the magic strings
                var outgoingReplyText = new StringBuilder();
                outgoingReplyText.Append("outgoing reply");

                if (reply.Headers.RelatesTo != null)
                {
                    outgoingReplyText.AppendFormat(" ({0})", myHelper.StripFormattingFromHeaderRelatesTo(reply.Headers.RelatesTo.ToString()));
                }

                reply = myLogger.Log("WCF Server Side", outgoingReplyText, reply);
            }
        }
    }

}
