namespace SoapRequestAndResponseTracing.Interfaces
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Text;

    /// <summary>
    /// ILogger interface
    /// Methods for logging the request and response
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log method - orchestrates the logging of the request or response
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="stepName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        void Log(string sourceType, StringBuilder stepName, Message message);
    }
}
