using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace SoapRequestAndResponseTracing
{
    public class DebugMessageDispatcher : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var messageBuffer = request.CreateBufferedCopy(int.MaxValue);

            var messageReader = new MessageReader();
            messageReader.Read(messageBuffer);

            request = messageBuffer.CreateMessage();
            return request;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            // var messageBuffer = reply.CreateBufferedCopy(int.MaxValue);

            //var messageReader = new MessageReader();
            //messageReadermr.Read(messageBuffer);

            //reply = messageBuffer.CreateMessage();
        }
    }

}
