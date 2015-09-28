using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Xml;

namespace SoapRequestAndResponseTracing
{
    public class DebugMessageInspector : IClientMessageInspector
    {
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var messageBuffer = request.CreateBufferedCopy(int.MaxValue);

            request = messageBuffer.CreateMessage();
            return request;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            var messageBuffer = reply.CreateBufferedCopy(int.MaxValue);

            var messageReader = new MessageReader();
            messageReader.Read(messageBuffer);

            reply = messageBuffer.CreateMessage();
        }
    }
}
