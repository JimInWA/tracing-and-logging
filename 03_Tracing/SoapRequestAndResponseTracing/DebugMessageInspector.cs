namespace SoapRequestAndResponseTracing
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    public class DebugMessageInspector : IClientMessageInspector
    {
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var myHelper = new Helper();

            if (myHelper.ShouldLogSoapRequestsAndResponses())
            {
                var myLogger = new Logger();
                // ToDo: Get rid of the magic strings
                request = myLogger.Log("MVC Client Side", "outgoing request", request);
            }

            return request;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            var myHelper = new Helper();

            if (myHelper.ShouldLogSoapRequestsAndResponses())
            {
                var myLogger = new Logger();
                // ToDo: Get rid of the magic strings
                reply = myLogger.Log("MVC Client Side", "incoming reply", reply);
            }
        }
    }
}
