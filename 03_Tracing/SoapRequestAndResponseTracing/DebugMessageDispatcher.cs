using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace SoapRequestAndResponseTracing
{
    public class DebugMessageDispatcher : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            return request;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }
    }

}
