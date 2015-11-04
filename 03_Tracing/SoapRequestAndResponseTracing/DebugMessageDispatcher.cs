namespace SoapRequestAndResponseTracing
{
    using System;
    using System.Configuration;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    public class DebugMessageDispatcher : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var shouldLogSoapRequestsAndResponses = false;

            var configSetting = ConfigurationManager.AppSettings["ShouldLogSoapRequestsAndResponses"];
            if (!string.IsNullOrWhiteSpace(configSetting))
            {
                bool.TryParse(configSetting, out shouldLogSoapRequestsAndResponses);
            }

            if (shouldLogSoapRequestsAndResponses)
            {
                var myLogger = new Logger();
                myLogger.Log(request);
            }

            return request;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var shouldLogSoapRequestsAndResponses = false;

            var configSetting = ConfigurationManager.AppSettings["ShouldLogSoapRequestsAndResponses"];
            if (!string.IsNullOrWhiteSpace(configSetting))
            {
                bool.TryParse(configSetting, out shouldLogSoapRequestsAndResponses);
            }

            if (shouldLogSoapRequestsAndResponses)
            {
                var myLogger = new Logger();
                myLogger.Log(reply);
            }
        }
    }

}
