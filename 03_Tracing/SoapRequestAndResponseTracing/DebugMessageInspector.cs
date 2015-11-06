namespace SoapRequestAndResponseTracing
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using System.Text;

    /// <summary>
    /// DebugMessageInspector class - implements IClientMessageInspector interface
    /// </summary>
    public class DebugMessageInspector : IClientMessageInspector
    {
        /// <summary>
        /// BeforeSendRequest method
        /// When a client call to a web service is being traced and logged, this is called after the client has sent the request but before the request is processed by the service
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var myHelper = new Helper();

            if (myHelper.ShouldLogSoapRequestsAndResponses())
            {
                var myLogger = new Logger();

                // ToDo: Get rid of the magic strings
                var outgoingRequestText = new StringBuilder();
                outgoingRequestText.Append("outgoing request");

                if (request.Headers.MessageId != null)
                {
                    outgoingRequestText.AppendFormat(" ({0})", myHelper.StripFormattingFromHeaderRelatesTo(request.Headers.MessageId.ToString()));
                }

                request = myLogger.Log("MVC Client Side", outgoingRequestText, request);
            }

            return request;
        }

        /// <summary>
        /// AfterReceiveReply method
        /// When a client call to a web service is being traced and logged, this is called after the service has processed the request but before the response is sent back to the client
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            var myHelper = new Helper();

            if (myHelper.ShouldLogSoapRequestsAndResponses())
            {
                var myLogger = new Logger();

                // ToDo: Get rid of the magic strings
                var incomingReplyText = new StringBuilder();
                incomingReplyText.Append("incoming reply");

                if (reply.Headers.RelatesTo != null)
                {
                    incomingReplyText.AppendFormat(" ({0})", myHelper.StripFormattingFromHeaderMessageId(reply.Headers.RelatesTo.ToString()));
                }

                reply = myLogger.Log("MVC Client Side", incomingReplyText, reply);
            }
        }
    }
}
