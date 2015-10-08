namespace SoapRequestAndResponseTracing
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    public class DebugMessageInspector
    {
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            return request;
        }
    }
}
