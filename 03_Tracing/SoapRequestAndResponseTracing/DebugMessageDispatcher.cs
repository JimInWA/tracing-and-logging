﻿namespace SoapRequestAndResponseTracing
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    public class DebugMessageDispatcher : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var myHelper = new Helper();

            if (myHelper.ShouldLogSoapRequestsAndResponses())
            {
                var myLogger = new Logger();
                // ToDo: Get rid of the magic strings
                request = myLogger.Log("WCF Server Side", "incoming request", request);
            }

            return request;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var myHelper = new Helper();

            if (myHelper.ShouldLogSoapRequestsAndResponses())
            {
                var myLogger = new Logger();
                // ToDo: Get rid of the magic strings
                reply = myLogger.Log("WCF Server Side", "outgoing reply", reply);
            }
        }
    }

}
