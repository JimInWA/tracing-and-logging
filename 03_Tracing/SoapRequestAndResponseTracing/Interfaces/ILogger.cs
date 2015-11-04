namespace SoapRequestAndResponseTracing.Interfaces
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    public interface ILogger
    {
        void Log(Message message);
    }
}
