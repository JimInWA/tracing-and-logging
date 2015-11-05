namespace SoapRequestAndResponseTracing.Interfaces
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    public interface ILogger
    {
        Message Log(Message message);
    }
}
