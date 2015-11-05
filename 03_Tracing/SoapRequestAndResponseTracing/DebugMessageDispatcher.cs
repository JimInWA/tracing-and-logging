namespace SoapRequestAndResponseTracing
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using System.Xml;


    public class DebugMessageDispatcher : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            if (ShouldLogSoapRequestsAndResponses())
            {
                var myLogger = new Logger();
                // ToDo: Get rid of the magic strings
                request = myLogger.Log("WCF Server Side", "incoming request", request);
            }

            return request;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            if (ShouldLogSoapRequestsAndResponses())
            {
                var myLogger = new Logger();
                // ToDo: Get rid of the magic strings
                reply = myLogger.Log("WCF Server Side", "outgoing reply", reply);
            }
        }

        private bool ShouldLogSoapRequestsAndResponses()
        {
            var shouldLogSoapRequestsAndResponses = false;

            // ToDo: Get rid of the magic strings
            var configSetting = ConfigurationManager.AppSettings["ShouldLogSoapRequestsAndResponses"];
            if (!string.IsNullOrWhiteSpace(configSetting))
            {
                bool.TryParse(configSetting, out shouldLogSoapRequestsAndResponses);
            }

            return shouldLogSoapRequestsAndResponses;
        }
    }

}
